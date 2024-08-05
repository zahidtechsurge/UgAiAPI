using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Administrator.API.Helpers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public LoggingHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper ?? throw new ArgumentNullException(nameof(repoWrapper));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logEntry = new WSDLLog
            {
                HttpMethod = request.Method.Method,
                Url = request.RequestUri?.AbsoluteUri,
                RequestBody = await request.Content.ReadAsStringAsync(),
                RequestTimestamp = DateTime.UtcNow
            };

            // Save request log to the database
            logEntry = await _repoWrapper.LoggingRepository.AddLogEntry(logEntry);

            // Call the inner handler to continue the request
            var response = await base.SendAsync(request, cancellationToken);

            // Update the log entry with response details
            logEntry.Status = response.StatusCode.ToString();
            logEntry.ResponseBody = await response.Content.ReadAsStringAsync();
            logEntry.ResponseTimestamp = DateTime.UtcNow;

            // Save response details to the database
            await _repoWrapper.LoggingRepository.UpdateLogEntry(logEntry);

            return response;
        }
    }
}
