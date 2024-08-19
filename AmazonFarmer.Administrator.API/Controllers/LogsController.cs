using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
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
        [HttpPost("getEventLogs")]
        public async Task<APIResponse> Get_Event_Logs(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<RequestLog> logs = _repoWrapper.LoggingRepository.GetLogs();
            resp.message = "Fetched paginated event logs";
            logs = logs.OrderByDescending(x => x.RequestId);
            if (!string.IsNullOrEmpty(req.search))
                logs = logs.Where(x => x.RequestId.ToString().Contains(req.search) || x.HttpMethod.Contains(req.search) || x.Url.Contains(req.search) || x.Body.Contains(req.search));
            InResp.totalRecord = logs.Count();
            logs = logs.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = logs.Count();
            InResp.list = await logs.Select(l => new LogDTO
            {
                recID = l.RequestId,
                method = l.HttpMethod,
                request = l.Body,
                requestDatetime = l.Timestamp,
                statusCode = l.Responses != null ? l.Responses.First().StatusCode : 0,
                response = l.Responses != null ? l.Responses.First().Body : string.Empty,
                responseDatetime = l.Responses != null ? l.Responses.First().Timestamp : null,
            }).ToListAsync();
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
