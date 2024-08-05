using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmerAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing reason-related operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ReasonController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        // Constructor injection of IRepositoryWrapper.
        public ReasonController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getReasonsByModule/{module}")]
        public async Task<APIResponse> getReasonsByModule(EReasonOf module)
        {
            APIResponse resp = new APIResponse();

            string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
            if (string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.languageCodeRequired);
            else
            {
                List<tblReasonTranslation> dbReasons = await _repoWrapper.ReasonRepo.getReasonsByLanguageCodeAndReasonOf(languageCode, module);
                resp.response = dbReasons.Select(x=> new DropDownValues
                {
                    value = x.ReasonID,
                    label = x.Text,
                    labelFor = x.Reason.ReasonForID
                }).ToList();
            }

            return resp;
        }
    }
}
