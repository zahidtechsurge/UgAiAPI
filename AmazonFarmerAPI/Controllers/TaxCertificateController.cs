using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmer.WSDL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;
using Microsoft.Extensions.Options;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class TaxCertificateController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private WsdlConfig _wsdlConfig;
        public TaxCertificateController(IRepositoryWrapper repositoryWrapper, IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repositoryWrapper;
            _wsdlConfig = wsdlConfig.Value;
        }

        [HttpGet("getCompanyCodes")]
        public APIResponse GetCompanyCodes()
        {
            var companies = new List<dynamic>
            {
                new { companyCode = "2000", companyName = "Engro Products" },
                new { companyCode = "2100", companyName = "Imported Products" }
            };

            return new APIResponse
            {
                isError = false,
                message = string.Empty,
                response = companies
            };
        }


        [HttpPost("request")]
        public async Task<JSONResponse> TaxCertificateRequest(TaxCertificateRequest request)
        {
            JSONResponse response = Validate(request);
            if (response.isError) { return response; }

            // Get the user ID from claims
            var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(UserID))
            {
                throw new AmazonFarmerException(_exceptions.userNotFound);
            }
            TblUser LoggedInUser = await _repoWrapper.UserRepo.getUserByUserID(UserID);
            TaxCertificateRequest WSDLRequest = new TaxCertificateRequest()
            {
                sapFarmerCode = LoggedInUser.FarmerProfile.First().SAPFarmerCode,
                companyCode = request.companyCode,
                startDate = ConvertToCompactDate(request.startDate),
                endDate = ConvertToCompactDate(request.endDate)
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
            var wsdlResponse = await wSDLFunctions.SendTaxCertificate(WSDLRequest);

            return new JSONResponse()
            {
                isError = (wsdlResponse == 1 || wsdlResponse > 0) ? false : true,
                message = (wsdlResponse == 1 || wsdlResponse > 0) ? $"Tax Certificate request for Company Code {request.companyCode} submitted successfully." : "an error occured"
            };
        }


        private JSONResponse Validate(TaxCertificateRequest model)
        {
            var response = new JSONResponse();
            var messages = new List<string>();

            if (string.IsNullOrWhiteSpace(model.startDate))
                messages.Add("Start Date is required");

            if (string.IsNullOrWhiteSpace(model.endDate))
                messages.Add("End Date is required");

            if (string.IsNullOrWhiteSpace(model.companyCode))
                messages.Add("Company Code is required");

            if (messages.Any())
            {
                response.isError = true;
                response.message = string.Join(", ", messages);
            }

            return response;
        }
        private string ConvertToCompactDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyyMMdd");
            }

            // Handle invalid format or return null/error message as needed
            throw new AmazonFarmerException(_exceptions.daterangeIsNotValid); // Or throw new FormatException("Invalid date format");
        }

    }
}
