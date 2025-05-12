using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public ConfigurationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [Authorize]
        [HttpPost("getConfiguration")]
        public async Task<APIResponse> GetConfiguration(ReportPagination_Req Request)
        {
            var resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            List<tblConfig> tblconfig = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType();
            if (!string.IsNullOrEmpty(Request.search))
            {
                tblconfig = tblconfig.Where(x =>
                    x.Id.ToString().Contains(Request.search) ||
                    x.Name.ToLower().Contains(Request.search.ToLower()) ||
                    x.Description.ToLower().Contains(Request.search.ToLower()) ||
                    x.Value.ToLower().Contains(Request.search.ToLower())
                ).ToList();
            }

            if (!string.IsNullOrEmpty(Request.sortColumn))
            {
                if (Request.sortColumn.Contains("name"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        tblconfig = tblconfig.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        tblconfig = tblconfig.OrderByDescending(x => x.Name).ToList();
                    }
                }
                else if (Request.sortColumn.Contains("id"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        tblconfig = tblconfig.OrderBy(x => x.Id).ToList();
                    }
                    else
                    {
                        tblconfig = tblconfig.OrderByDescending(x => x.Id).ToList();
                    }
                }
                else if (Request.sortColumn.Contains("description"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        tblconfig = tblconfig.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        tblconfig = tblconfig.OrderByDescending(x => x.Description).ToList();
                    }
                }
                else if (Request.sortColumn.Contains("value"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        tblconfig = tblconfig.OrderBy(x => x.Value).ToList();
                    }
                    else
                    {
                        tblconfig = tblconfig.OrderByDescending(x => x.Value).ToList();
                    }
                }
            }
            else
            {
                tblconfig = tblconfig.OrderByDescending(x => x.Id).ToList();
            }


            InResp.totalRecord = tblconfig.Count();
            tblconfig = tblconfig.Skip(Request.pageNumber * Request.pageSize)
                         .Take(Request.pageSize).ToList();
            InResp.filteredRecord = tblconfig.Count();
            InResp.list = tblconfig
                .Select(c => new
                {
                    id = c.Id,
                    value = c.Value,
                    description = c.Description,
                    name = c.Name,
                    statusID = (int)c.Status
                })
                .ToList();
            resp.response = InResp;
            return resp;
        }

        [Authorize]
        [HttpPut("updateConfiguration")]
        public async Task<APIResponse> UpdateConfiguration(UpdateConfigRequest Request)
        {
            APIResponse response = new APIResponse();
            tblConfig? Config = await _repoWrapper.CommonRepo.GetConfigurationByID(Request.configID);
            if (Config == null) { response.isError = true; response.message = "configuration not found"; return response; }
            Config.Name = Request.name;
            Config.Value = Request.value;
            Config.Description = Request.description;
            Config.Status = (EConfigStatus)Request.status;
            _repoWrapper.CommonRepo.UpdateConfigurationValue(Config);
            await _repoWrapper.SaveAsync();
            response.message = "record updated";
            return response;
        }
    }
}
