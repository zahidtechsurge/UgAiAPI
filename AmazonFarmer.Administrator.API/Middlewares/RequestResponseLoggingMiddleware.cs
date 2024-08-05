using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json.Nodes;
using AmazonFarmer.Administrator.API.Helpers;
using AmazonFarmer.Core.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Administrator.API.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, AmazonFarmerContext dbContext, IWebHostEnvironment _env)
        {

            if (context.Request.Path.Value.Contains("/swagger/"))
            {
                await _next(context);
            }
            else
            {
                var requestLog = await FormatRequest(context.Request);
                _logger.LogInformation(requestLog);

                if (context.Request.Path.Value.Contains("/api/Farmer/signin"))
                {
                    requestLog = "{\"message\": \"Not saving body of sign in request\"}";
                }

                var reqLog = dbContext.RequestLogs.Add(new RequestLog
                {
                    HttpMethod = context.Request.Method,
                    Url = context.Request.Path,
                    Body = requestLog,
                    Timestamp = DateTime.UtcNow
                }).Entity;

                await dbContext.SaveChangesAsync();

                // Temporary replace the response body to intercept the data being written to it
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionAsync(context, ex, reqLog, _env);
                    }
                    finally
                    {
                        // Copy the captured response body to the original response stream
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                        context.Response.Body = originalBodyStream;  // Restore the original stream
                    }

                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseLog = await FormatResponse(responseBody);
                    _logger.LogInformation(responseLog);

                    dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
                    foreach (var entry in dbContext.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }
                    try
                    {
                        using (var transaction = dbContext.Database.BeginTransaction())
                        {

                            dbContext.ResponseLogs.Add(new ResponseLog
                            {
                                StatusCode = context.Response.StatusCode,
                                Body = responseLog,
                                Timestamp = DateTime.UtcNow,
                                RequestId = reqLog.RequestId
                            });
                            //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT RequestLogs ON");
                            //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ResponseLogs ON");
                            await dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }

        private async Task<string> FormatResponse(Stream responseBody)
        {
            using (var reader = new StreamReader(responseBody))
            {
                string text = await reader.ReadToEndAsync();
                return text;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, RequestLog reqLog, IWebHostEnvironment _env)
        {
            context.Response.ContentType = "application/json";
            var exceptionDetails = GetExceptionDetails(exception);
            var json = JsonConvert.SerializeObject(exceptionDetails);
            string filePath = Path.Combine(_env.ContentRootPath, $"logs/Exceptions/{reqLog.RequestId}_{DateTime.UtcNow:yyyy-MM-ddThhmmss-7199222}.json");
            await WriteFileAsync(filePath, json);

            var errorResponse = new APIResponse
            {
                isError = true,
                message = "An internal error occurred."
            };

            if (exception is ApplicationException && exception.Message.Contains("Unauthorized"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.message = "You are not authorized to perform this action.";
            }
            else if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.message = "The requested resource was not found.";
            }
            else if (exception is AmazonFarmerException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                errorResponse.message = exception.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            var result = JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(result);
        }

        private ExceptionDetails GetExceptionDetails(Exception ex)
        {
            if (ex == null) return null;

            return new ExceptionDetails
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Data = ex.Data,
                InnerException = GetExceptionDetails(ex.InnerException)
            };
        }

        private async Task WriteFileAsync(string filePath, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await stream.WriteAsync(byteData, 0, byteData.Length);
            }
        }


        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return $"{body}";
        }

        
    }
}
