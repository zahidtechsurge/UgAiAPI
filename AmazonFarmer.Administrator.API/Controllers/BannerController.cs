using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Reflection;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class BannerController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public BannerController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [Obsolete]
        [HttpPost("getBanners")]
        public async Task<APIResponse> _GetBanners(GetBannerAdminRequest req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            //IQueryable<tblBanner> banners = _repoWrapper.BannerRepo.getBannerQueryable();
            IQueryable<tblBannerLanguages> banners = _repoWrapper.BannerRepo.getBannerLanguagesQueryable();

            if (!string.IsNullOrEmpty(req.search))
                banners = banners.Where(x => x.Banner.Name.ToLower().Contains(req.search.ToLower()));
            //banners = banners.Where(x => x.Status == EActivityStatus.Active);
            InResp.totalRecord = banners.Count();
            banners = banners.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = banners.Count();
            InResp.list = await banners.OrderBy(x => x.BannerID).Select(x => new GetBannerAdminResponse
            {
                banner = x.Banner.Name,
                filePath = x.Image,
                languageCode = x.LanguageCode,
                language = x.Languages.LanguageName,
                translationID = x.ID
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpPost("_getBanners")]
        public async Task<APIResponse> GetBanners(GetBannerAdminRequest req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblBanner> banners = _repoWrapper.BannerRepo.getBannerQueryable();
            resp.response = InResp;
            return resp;
        }


        [HttpPost("addBanner")]
        public async Task<JSONResponse> AddBanner(AddBanner req)
        {
            JSONResponse resp = new JSONResponse();
            tblBanner? banner = await _repoWrapper.BannerRepo.getBannerByTypeID(req.bannerType);
            if (banner == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            if (!string.IsNullOrEmpty(req.content))
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper);
                AttachmentsDTO attachment = attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: (req.bannerType == EBannerType.homeScreen ? EAttachmentType.HomeBanner : EAttachmentType.LoginBanner));
                tblBannerLanguages bannerReq = new tblBannerLanguages()
                {
                    BannerID = banner.ID,
                    Image = string.Concat("/", attachment.filePath.Replace("\\", "/")),
                    LanguageCode = req.languageCode
                };
                _repoWrapper.BannerRepo.addBannerLanguage(bannerReq);
                await _repoWrapper.SaveAsync();
                resp.message = banner.Name + " has been added.";
            }
            return resp;
        }

        [HttpPut("updateBanner")]
        public async Task<JSONResponse> UpdateBanner(UpdateBanner req)
        {
            JSONResponse resp = new JSONResponse();
            tblBannerLanguages? bl = await _repoWrapper.BannerRepo.getBannerLanguageByID(req.translationID);
            if (bl == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            if (!string.IsNullOrEmpty(req.content))
            {
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper);
                AttachmentsDTO attachment = attachmentExt.UploadAttachment(name: req.fileName, content: req.content, requestTypeID: (req.bannerType == EBannerType.homeScreen ? EAttachmentType.HomeBanner : EAttachmentType.LoginBanner));
                bl.Image = string.Concat("/", attachment.filePath.Replace("\\", "/"));
                _repoWrapper.BannerRepo.updateBannerLanguage(bl);
                await _repoWrapper.SaveAsync();
                resp.message = bl.Banner.Name + " has been updated.";
            }
            return resp;
        }

        [Obsolete]
        [HttpDelete("deleteBanner/{translationID}")]
        public async Task<JSONResponse> DeleteBanner(int translationID)
        {
            JSONResponse resp = new JSONResponse();
            tblBannerLanguages? bl = await _repoWrapper.BannerRepo.getBannerLanguageByID(translationID);
            if (bl == null)
            {
                throw new AmazonFarmerException(_exceptions.notFound);
            }
            //bl.Status = EActivityStatus.DeActive;
            _repoWrapper.BannerRepo.updateBannerLanguage(bl);
            await _repoWrapper.SaveAsync();
            return resp;
        }
    }
}
