using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Net.Mail;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/Service")]
    public class ServiceController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IAzureFileShareService _azureFileShareService;
        public ServiceController(IRepositoryWrapper repositoryWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repositoryWrapper;
            _azureFileShareService = azureFileShareService;
        }

        #region Service Module
        [HttpPost("addService")]
        public async Task<JSONResponse> AddService(AddServiceRequest req)
        {
            tblService? service = await _repoWrapper.ServiceRepo.GetServiceByID(req.serviceName, req.serviceCode);
            if (service == null)
            {
                JSONResponse resp = new JSONResponse();
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                service = new tblService()
                {
                    Name = req.serviceName,
                    Code = req.serviceCode,
                    CreatedByID = userID,
                    CreatedDate = DateTime.UtcNow,
                    Active = EActivityStatus.Active
                };
                _repoWrapper.ServiceRepo.AddService(service);
                await _repoWrapper.SaveAsync();
                resp.message = "Service Added";
                return resp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.serviceAlreadyExist);
            }
        }
        [HttpPost("getService")]
        public async Task<APIResponse> GetService(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            resp.message = "Fetched paginated Services";
            IQueryable<tblService> services = _repoWrapper.ServiceRepo.GetService();

            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("serviceID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        services = services.OrderBy(x => x.ID);
                    }
                    else
                    {
                        services = services.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("serviceName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        services = services.OrderBy(x => x.Name);
                    }
                    else
                    {
                        services = services.OrderByDescending(x => x.Name);
                    }
                }
                else if (req.sortColumn.Contains("serviceCode"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        services = services.OrderBy(x => x.Code);
                    }
                    else
                    {
                        services = services.OrderByDescending(x => x.Code);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        services = services.OrderBy(x => x.Active);
                    }
                    else
                    {
                        services = services.OrderByDescending(x => x.Active);
                    }
                }
            }
            else
            {
                services = services.OrderByDescending(x => x.ID);
            }
            if (!string.IsNullOrEmpty(req.search))
                services = services.Where(x => (x.Name ?? string.Empty).Contains(req.search) || (x.Code ?? string.Empty).Contains(req.search));
            InResp.totalRecord = services.Count();


            services = services.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = services.Count();
            InResp.list = await services.Select(x => new UpdateServiceRequest
            {
                serviceID = x.ID,
                serviceName = x.Name ?? string.Empty,
                serviceCode = x.Code ?? string.Empty,
                status = (int)x.Active
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpPut("updateService")]
        public async Task<JSONResponse> UpdateService(UpdateServiceRequest req)
        {
            tblService? service = await _repoWrapper.ServiceRepo.GetServiceByID(req.serviceID);
            if (service != null)
            {
                JSONResponse resp = new JSONResponse();
                service.Name = req.serviceName;
                service.Code = req.serviceCode;
                service.Active = (EActivityStatus)req.status;
                _repoWrapper.ServiceRepo.UpdateService(service);
                await _repoWrapper.SaveAsync();
                resp.message = "Service Updated";
                return resp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.serviceNotFound);
            }
        }
        #endregion


        #region Service Translation
        [HttpPost("addServiceTranslation")]
        public async Task<JSONResponse> AddServiceTranslation(AddServiceTranslationRequest req)
        {
            tblServiceTranslation? serviceTranslation = await _repoWrapper.ServiceRepo.GetServiceTranslationByID(req.text, req.languageCode);
            if (serviceTranslation == null)
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                AttachmentsDTO attachment = new AttachmentsDTO();
                if (!string.IsNullOrEmpty(req.content))
                    attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "service.svg"), content: req.content, requestTypeID: EAttachmentType.Service);

                JSONResponse resp = new JSONResponse();
                serviceTranslation = new tblServiceTranslation()
                {
                    ServiceID = req.serviceID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = attachment.filePath ?? string.Empty
                };
                _repoWrapper.ServiceRepo.AddServiceTranslation(serviceTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Service Translation Added";
                return resp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.serviceTranslationAlreadyExist);
            }
        }
        [AllowAnonymous]
        [HttpGet("getTranslations/{serviceID}")]
        public async Task<APIResponse> GetTranslations(int serviceID)
        {
            APIResponse resp = new APIResponse();
            List<tblServiceTranslation> serviceTranslations = await _repoWrapper.ServiceRepo.GetServiceTranslationByServiceID(serviceID);
            resp.response = serviceTranslations.Select(x => new GetServiceTranslationResponse
            {
                translationID = x.ID,
                serviceID = x.ServiceID,
                text = x.Text,
                filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), "%2F", x.Image.TrimStart('/').Replace("\\", "%2F").Replace("/", "%2F").Replace(" ", "%20")),
                languageCode = x.LanguageCode,
                language = x.Language.LanguageName,
            }).ToList();
            return resp;
        }
        [AllowAnonymous]
        [HttpPatch("syncServiceTranslation")]
        public async Task<JSONResponse> SyncServiceTranslation(UpdateServiceTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblServiceTranslation? serviceTranslation = await _repoWrapper.ServiceRepo.GetServiceTranslationByID(req.serviceID, req.languageCode);
            if (serviceTranslation != null)
            {
                if (string.IsNullOrEmpty(req.filePath))
                {
                    req.content = req.content.Replace("data:image/png;base64,", "");
                    req.content = req.content.Replace("data:image/svg+xml;base64,", "");
                    AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                    AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "service.svg"), content: req.content, requestTypeID: EAttachmentType.Service);
                    serviceTranslation.Image = attachment.filePath.Replace("\\","/");
                }
                else
                    serviceTranslation.Image = req.filePath.Replace(string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")),"").Replace("%20", " ").Replace("%2F", "/");

                serviceTranslation.ServiceID = req.serviceID;
                serviceTranslation.Text = req.text;
                _repoWrapper.ServiceRepo.UpdateServiceTranslation(serviceTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Service Translation Updated";
            }
            else
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                AttachmentsDTO attachment = new AttachmentsDTO();
                if (!string.IsNullOrEmpty(req.content))
                {
                    req.content = req.content.Replace("data:image/png;base64,", "");
                    req.content = req.content.Replace("data:image/svg+xml;base64,", "");
                    attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "service.svg"), content: req.content, requestTypeID: EAttachmentType.Service);
                }

                serviceTranslation = new tblServiceTranslation()
                {
                    ServiceID = req.serviceID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = attachment.filePath.Replace("\\", "/") ?? string.Empty
                };
                _repoWrapper.ServiceRepo.AddServiceTranslation(serviceTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Service Translation Added";
                //throw new AmazonFarmerException(_exceptions.serviceTranslationNotFound);
            }
            return resp;
        }

        #endregion
    }
}
