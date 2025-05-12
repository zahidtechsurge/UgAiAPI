using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Administrator.API.Controllers
{ /// <summary>
  /// Controller for managing attachment-related operations.
  /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IAzureFileShareService _azureFileShareService;
        // Constructor injection of IRepositoryWrapper.
        public AttachmentController(IRepositoryWrapper repoWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _azureFileShareService = azureFileShareService;
        }

        [AllowAnonymous]
        [HttpGet("getPublicAttachment/{filePath}")]
        public async Task<dynamic> GetPublicAttachment(string filePath)
        {
            Stream? imageOutput = await _azureFileShareService.GetFileAsync(filePath);
            return base.File(imageOutput, GetContentType(filePath), "");
        }
        private string GetContentType(string filename)
        {
            var contentTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "png", "image/png" },
                { "svg", "image/svg+xml" }
            };

            string extension = Path.GetExtension(filename).TrimStart('.');

            return contentTypes.TryGetValue(extension, out string contentType) ? contentType : null;
        }
    }
}
