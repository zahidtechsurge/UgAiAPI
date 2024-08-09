using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class IntroController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public IntroController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [AllowAnonymous]
        [Obsolete]
        [HttpPost("getIntros")]
        public async Task<APIResponse> GetIntros(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblIntroLanguages> intros = _repoWrapper.IntroRepo.getIntroLanguageQueryable();
            if (!string.IsNullOrEmpty(req.search))
                intros = intros.Where(x => x.Text.ToLower().Contains(req.search.ToLower()));
            //intros = intros.Where(x => x.Status == EActivityStatus.Active);
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
                filePath = x.Image
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }

        [Obsolete]
        [HttpPost("addIntro")]
        public async Task<JSONResponse> AddIntro(AddIntroAdminRequest req)
        {
            JSONResponse resp = new JSONResponse();
            if (!string.IsNullOrEmpty(req.content))
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper);
                AttachmentsDTO attachment = attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: EAttachmentType.IntroBanner);
                tblIntro intro = new tblIntro()
                {
                    Name = "Intro",
                    Status = EActivityStatus.Active,
                    IntroLanguages = new List<tblIntroLanguages>
                    {
                       new tblIntroLanguages()
                        {
                            LanguageCode = req.languageCode,
                            Image = string.Concat("/", attachment.filePath.Replace("\\", "/")),
                            Text = req.intro,
                            //Status = EActivityStatus.Active
                        }
                    }
                };
                _repoWrapper.IntroRepo.addIntro(intro);
                await _repoWrapper.SaveAsync();
                resp.message = "Intro added";
            }

            return resp;
        }

        [HttpPut("updatetIntro")]
        public async Task<JSONResponse> UpdateIntro(UpdateIntroAdminRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblIntroLanguages? il = await _repoWrapper.IntroRepo.GetIntroLanguagesByID(req.translationID);
            if (il == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            if (!string.IsNullOrEmpty(req.content))
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper);
                AttachmentsDTO attachment = attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: EAttachmentType.IntroBanner);
                il.Image = string.Concat("/", attachment.filePath.Replace("\\", "/"));
                il.LanguageCode = req.languageCode;
            }
            il.Text = req.intro;
            _repoWrapper.IntroRepo.updateIntroLanguage(il);
            await _repoWrapper.SaveAsync();
            return resp;
        }

        [Obsolete]
        [HttpDelete("deletetIntro/{translationID}")]
        public async Task<JSONResponse> DeleteIntro(int translationID)
        {
            JSONResponse resp = new JSONResponse();
            tblIntroLanguages? il = await _repoWrapper.IntroRepo.GetIntroLanguagesByID(translationID);
            if (il == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            //il.Status = EActivityStatus.DeActive;
            _repoWrapper.IntroRepo.updateIntroLanguage(il);
            await _repoWrapper.SaveAsync();
            return resp;
        }
    }
}
