using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class IntroController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        private readonly IAzureFileShareService _azureFileShareService;
        public IntroController(IRepositoryWrapper repoWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _azureFileShareService = azureFileShareService;
        }

        [HttpPost("addIntro")] //hassam 28/8/
        public async Task<JSONResponse> AddIntro(AddIntroRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblIntro? existingIntro = await _repoWrapper.IntroRepo.GetIntroByName(req.name);
            if (existingIntro != null)
            {
                throw new AmazonFarmerException("Intro already exists with the given name.");
            }
            tblIntro newIntro = new tblIntro
            {
                Name = req.name,
                Status = EActivityStatus.Active
            };
            _repoWrapper.IntroRepo.AddIntro(newIntro);
            await _repoWrapper.SaveAsync();

            resp.message = "Intro added successfully.";
            return resp;

        }

        [HttpPut("updateIntro")]//28/08 hassam
        public async Task<JSONResponse> UpdateIntro(updateIntroRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblIntro? updatingIntro = await _repoWrapper.IntroRepo.GetIntroByNameAndStatus(req.id);
            if (updatingIntro == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            updatingIntro.Name = req.name;
            updatingIntro.Status = (EActivityStatus)req.status;
            _repoWrapper.IntroRepo.UpdateIntro(updatingIntro);
            await _repoWrapper.SaveAsync();
            resp.message = "Intro updated successfully.";
            return resp;
        }
        [HttpPost("getIntro")]
        public async Task<APIResponse> GetIntros(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblIntro> intros = _repoWrapper.IntroRepo.getIntroQueryable();

            if (!string.IsNullOrEmpty(req.search))
                intros = intros.Where(x => x.Name.ToLower().Contains(req.search.ToLower()));
            InResp.totalRecord = intros.Count();
            intros = intros.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = intros.Count();
            InResp.list = await intros.Select(x=> new
            {
                introID = x.ID,
                name = x.Name,
                status = (int)x.Status
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }

        [HttpPost("getTranslations")]
        public async Task<APIResponse> GetIntroLanguages(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblIntroLanguages> intros = _repoWrapper.IntroRepo.getIntroLanguageQueryable();
            intros = intros.Where(i => i.IntroID == req.rootID);
            if (!string.IsNullOrEmpty(req.search))
                intros = intros.Where(x => x.Text.ToLower().Contains(req.search.ToLower()));
            InResp.totalRecord = intros.Count();
            intros = intros.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = intros.Count();
            InResp.list = await intros.OrderBy(x => x.IntroID).Select(x => new GetIntroAdminResponse
            {
                translationID = x.ID,
                languageCode = x.LanguageCode,
                language = x.Language.LanguageName,
                intro = x.Text,
                filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), "%2F", x.Image.TrimStart('/').Replace("\\", "%2F").Replace("/", "%2F").Replace(" ", "%20")),
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }

        
        [HttpPatch("syncIntroTranslation")]
        public async Task<JSONResponse> SyncIntro(UpsertIntroAdminRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblIntroLanguages? existingIntroLanguage = await _repoWrapper.IntroRepo.GetIntroLanguageByIntroIDAndLanguage(req.introID, req.languageCode);

            if (existingIntroLanguage != null)
            {
                existingIntroLanguage.Text = req.text;
                existingIntroLanguage.LanguageCode = req.languageCode;
                if (string.IsNullOrEmpty(req.filePath))
                {
                    if (!string.IsNullOrEmpty(req.content))
                    {
                        req.content = req.content.Replace("data:image/png;base64,", "");
                        AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                        AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: EAttachmentType.IntroBanner);
                        existingIntroLanguage.Image = string.Concat("/", attachment.filePath.Replace("\\", "/"));
                    }
                }
                else
                {
                    existingIntroLanguage.Image = req.filePath.Replace(string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")), "").Replace("%20", " ").Replace("%2F", "/");
                    //existingIntroLanguage.Image = req.filePath;
                }
                _repoWrapper.IntroRepo.updateIntroLanguage(existingIntroLanguage);
                resp.message = "Intro updated";
            }
            else
            {
                existingIntroLanguage = new tblIntroLanguages()
                {
                    IntroID = req.introID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = string.Empty
                };
                if (string.IsNullOrEmpty(req.filePath))
                {
                    if (!string.IsNullOrEmpty(req.content))
                    {
                        req.content = req.content.Replace("data:image/png;base64,", "");
                        AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                        AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: EAttachmentType.IntroBanner);
                        existingIntroLanguage.Image = string.Concat("/", attachment.filePath.Replace("\\", "/"));
                    }
                }
                else
                {
                    //existingIntroLanguage.Image = req.filePath; 
                    existingIntroLanguage.Image = req.filePath.Replace(string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")), "").Replace("%20", " ").Replace("%2F", "/");

                }
                _repoWrapper.IntroRepo.addIntroLanguage(existingIntroLanguage);
                resp.message = "Intro added";
            }
            await _repoWrapper.SaveAsync();
            return resp;
        }
    }
}
