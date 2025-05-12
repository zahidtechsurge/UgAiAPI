using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Linq;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class LogsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public LogsController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [AllowAnonymous]
        [HttpPost("getEventLogs")]
        public async Task<APIResponse> Get_Event_Logs(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            List<SP_LogEntryResult> report = await _repoWrapper.LoggingRepository.GetLogs(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search);
            resp.message = "Fetched paginated event logs";
            if (report != null && report.Count() > 0)
            {
                InResp.totalRecord = report.Count > 0 ? report.First().totalRows : 0;
                InResp.filteredRecord = report.Count();
                InResp.list = report.Select(x => new LogDTO
                {
                    recID = x.requestId,
                    method = x.requestHttpMethod ?? string.Empty,
                    url = x.requestURL ?? string.Empty,
                    request = x.requestBody ?? string.Empty,
                    requestDatetime = x.requestTimestamp,
                    statusCode = x.responseStatusCode ?? 0,
                    response = x.responseBody ?? string.Empty,
                    responseDatetime = x.responseTimestamp,
                }).ToList();
            }
            else
            {
                InResp.list = new List<LogDTO>();
            }
            resp.response = InResp;
            return resp;
        }
        [HttpPost("getWSDLLogs")]
        public async Task<APIResponse> Get_WSDL_Logs(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<WSDLLog> logs = _repoWrapper.LoggingRepository.GetWSDLLogs();
            resp.message = "Fetched paginated WSDL logs";
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("recID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.RequestId);
                    else
                        logs = logs.OrderByDescending(x => x.RequestId);
                }
                else if (req.sortColumn.Contains("method"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.HttpMethod);
                    else
                        logs = logs.OrderByDescending(x => x.HttpMethod);
                }
                else if (req.sortColumn.Contains("url"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Url);
                    else
                        logs = logs.OrderByDescending(x => x.Url);
                }
                else if (req.sortColumn.Contains("request"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.RequestBody);
                    else
                        logs = logs.OrderByDescending(x => x.RequestBody);
                }
                else if (req.sortColumn.Contains("requestDatetime"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.RequestTimestamp);
                    else
                        logs = logs.OrderByDescending(x => x.RequestTimestamp);
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Status);
                    else
                        logs = logs.OrderByDescending(x => x.Status);
                }
                else if (req.sortColumn.Contains("response"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.ResponseBody);
                    else
                        logs = logs.OrderByDescending(x => x.ResponseBody);
                }
                else if (req.sortColumn.Contains("responseDatetime"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.ResponseTimestamp);
                    else
                        logs = logs.OrderByDescending(x => x.ResponseTimestamp);
                }
            }
            else
            {
                logs = logs.OrderByDescending(x => x.RequestId);
            }
            if (!string.IsNullOrEmpty(req.search))
                logs = logs.Where(x => x.RequestId.ToString().Contains(req.search) || x.HttpMethod.Contains(req.search) || x.Url.Contains(req.search) || x.RequestBody.Contains(req.search));
            InResp.totalRecord = logs.Count();
            logs = logs.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = logs.Count();
            InResp.list = await logs.Select(l => new LogDTO
            {
                recID = l.RequestId,
                method = l.HttpMethod,
                url = l.Url,
                request = l.RequestBody,
                requestDatetime = l.RequestTimestamp,
                status = l.Status ?? string.Empty,
                response = l.ResponseBody ?? string.Empty,
                responseDatetime = l.ResponseTimestamp,
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [AllowAnonymous]
        [HttpPost("getEmailLogs")]
        public async Task<APIResponse> Get_Email_Logs(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<NotificationLog> logs = _repoWrapper.LoggingRepository.GetNotificationLogs();
            logs = logs.Where(x => x.Type == ENotificationType.Email);
            resp.message = "Fetched paginated Email logs";
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("recID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Id);
                    else
                        logs = logs.OrderByDescending(x => x.Id);
                }
                else if (req.sortColumn.Contains("recipient"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Recipient);
                    else
                        logs = logs.OrderByDescending(x => x.Recipient);
                }
                else if (req.sortColumn.Contains("subject"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Subject);
                    else
                        logs = logs.OrderByDescending(x => x.Subject);
                }
                else if (req.sortColumn.Contains("message"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.Message);
                    else
                        logs = logs.OrderByDescending(x => x.Message);
                }
                else if (req.sortColumn.Contains("sentDate"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.SentDate);
                    else
                        logs = logs.OrderByDescending(x => x.SentDate);
                }
                else if (req.sortColumn.Contains("isSent"))
                {
                    if (req.sortOrder.Contains("ASC"))
                        logs = logs.OrderBy(x => x.IsSuccess);
                    else
                        logs = logs.OrderByDescending(x => x.IsSuccess);
                }
            }
            else
            {
                logs = logs.OrderByDescending(x => x.Id);
            }
            if (!string.IsNullOrEmpty(req.search))
            {
                logs = logs.Where(x =>
                    x.Id.ToString().Contains(req.search.ToLower()) ||
                    x.Recipient.ToLower().Contains(req.search.ToLower()) ||
                    x.Subject.ToLower().Contains(req.search.ToLower()) ||
                    x.Message.ToLower().Contains(req.search.ToLower())
                );
            }
            InResp.totalRecord = logs.Count();
            logs = logs.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = logs.Count();
            InResp.list = await logs.Select(l => new NotificationLogDTO
            {
                recID = l.Id,
                typeID = (int)l.Type,
                recipient = l.Recipient,
                subject = l.Subject ?? string.Empty,
                message = l.Message,
                sentDate = l.SentDate,
                isSent = l.IsSuccess
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }

    }
}
