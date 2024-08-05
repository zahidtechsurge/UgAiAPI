
/*
   This controller handles requests related to banners.
   It is responsible for fetching banners, and it requires authentication for all methods except getBanners.

   [ApiController] attribute denotes that this class is a controller and provides behavior specific to Web APIs.
   [Authorize] attribute specifies that authentication is required for accessing controller methods, using the Bearer authentication scheme.
   [Route] attribute defines the base route for all endpoints in this controller.

   The BannerController class inherits from ControllerBase, which provides common functionality for MVC controllers.
*/

using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using CustomerCreateWsdl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using AmazonFarmer.Core.Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading;
using Newtonsoft.Json;
using AmazonFarmer.WSDL.Helpers;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class BannerController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private IConfiguration _configuration;
        // Constructor injection of IRepositoryWrapper.
        public BannerController(IRepositoryWrapper repoWrapper, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _configuration = configuration;
        }

        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpPost("getBanners")]
        public async Task<APIResponse> getBanners(LanguageReq req)
        {

            await AmazonFarmer.WSDL.Class1.CreateCustomerWSDLAsync();

            APIResponse resp = new APIResponse();
            try
            {
                // Fetch banners asynchronously using repository.
                resp.response = await _repoWrapper.BannerRepo.getBanners(req);
            }
            catch (Exception ex)
            {
                // If an exception occurs, set response properties accordingly.
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpPost("callingWsdlSample")]
        public async Task<APIResponse> CallingWsdlSample(LanguageReq req)
        {

            CustomerCreateClient OutClient = new(CustomerCreateClient.EndpointConfiguration.CustomerCreateSOAP);


            string WSDLUserName = _configuration["WsdlConfig:UserName"].ToString();
            string WSDLPassword = _configuration["WsdlConfig:Password"].ToString();

            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;


            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                var request = new RequestType
                {
                    city = "KARACHI",
                    cnic = "413000000012",
                    condGrp1 = "",
                    condGrp2 = "",
                    condGrp3 = "",
                    condGrp4 = "",
                    district = "CENTRAL",
                    email = "MURSALEEN.CHAWLA@ENGRO.COM1",
                    fax = "02191574254",
                    mobileNum = "02001254221",
                    name = "y BP",
                    ntn = "",
                    phoneNum = "02196312148",
                    postalCode = "789456",
                    salePoint = "Z00796",
                    searchTerm1 = "BP",
                    searchTerm2 = "Bp",
                    street = "BRRRRRRRRRRRRR",
                    street2 = "BRRRRRRRRRRRRR",
                    street4 = "BRRRRRRRRRRRRR",
                    strn = ""
                };

                WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.AbsolutePath, "SOAP");
                var resp = OutClient.Create(request);
                await LogResponseWsdl(log, resp);
                APIResponse aPIResponse = new APIResponse();
                aPIResponse.response = resp;
                aPIResponse.isError = false;
                aPIResponse.message = "Records Fetched";
                return aPIResponse;

            }
        }

        private async Task<WSDLLog> LogRequestWsdl(RequestType request, string requestURL, string requestMethod)
        {
            var logEntry = new WSDLLog
            {
                HttpMethod = "SOAP",
                Url = requestURL,
                RequestBody = JsonConvert.SerializeObject(request),
                RequestTimestamp = DateTime.UtcNow
            };

            // Save request log to the database
            logEntry = _repoWrapper.LoggingRepository.AddLogEntry(logEntry);
            await _repoWrapper.SaveAsync();

            return logEntry;
        }

        private async Task LogResponseWsdl(WSDLLog logEntry, ResponseType response)
        {

            // Update the log entry with response details
            logEntry.Status = "Success";
            logEntry.ResponseBody = JsonConvert.SerializeObject(response);
            logEntry.ResponseTimestamp = DateTime.UtcNow;

            // Save response details to the database
            _repoWrapper.LoggingRepository.UpdateLogEntry(logEntry);
            await _repoWrapper.SaveAsync();
        }
    }
}
