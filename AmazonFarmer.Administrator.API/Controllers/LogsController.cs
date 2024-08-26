using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg.OpenPgp;

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
        public async Task<APIResponse> Get_WSDL_Logs(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<WSDLLog> logs = _repoWrapper.LoggingRepository.GetWSDLLogs();
            resp.message = "Fetched paginated WSDL logs";
            logs = logs.OrderByDescending(x => x.RequestId);
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
    }
}
