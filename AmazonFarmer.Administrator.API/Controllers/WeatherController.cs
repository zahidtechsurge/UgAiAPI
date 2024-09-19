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
    public class WeatherController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        private readonly IAzureFileShareService _azureFileShareService;


        public WeatherController(IRepositoryWrapper repoWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _azureFileShareService = azureFileShareService;
        }

        [AllowAnonymous]
        [HttpGet("GetWeatherIcons")]
        public async Task<APIResponse> GetWeatherIcons()
        {
            APIResponse resp = new APIResponse();
            AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
            string weatherIconsPath = attachmentExt.getFilePathByRequestType(EAttachmentType.Weather_Icons);
            List<GetAzureFilesResponse> files = await _azureFileShareService.GetAzureFilesByFilePath(weatherIconsPath);
            foreach (var file in files)
            {
                file.filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), "%2F", file.filePath.TrimStart('/').Replace("\\", "%2F").Replace("/", "%2F").Replace(" ", "%20"));
            }
            resp.isError = false;
            resp.message = "Weather icons retrieved successfully.";
            resp.response = files;
            return resp;
        }


        [HttpPatch("syncWeatherIcons")]
        public async Task<JSONResponse> AddWeatherIcons(UploadAttachmentRequest request)
        {
            JSONResponse resp = new JSONResponse();
            if (request == null || string.IsNullOrEmpty(request.fileName) || string.IsNullOrEmpty(request.content))
            {
                throw new AmazonFarmerException(_exceptions.fileExtensionNotValid);
            }
            byte[] fileBytes = string.IsNullOrEmpty(request.content) ? request.contentBytes : Convert.FromBase64String(request.content);
            //byte[] fileBytes = request.fileContent;
            bool fileExists = await _azureFileShareService.CheckIfFileExistsAsync(request.fileName);
            string fileNameToUpload = request.fileName;
            if (fileExists)
            {
                fileNameToUpload = await _azureFileShareService.RenameFileAsync(request.fileName);
            }
            await _azureFileShareService.UploadFileAsync(fileBytes, fileNameToUpload);
            resp.isError = false;
            resp.message = $"Error occurred";
            return resp;
        }

    }

}
