
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmer.WSDL;
using AmazonFarmer.NotificationServices.Services;
using DetailsInvoice;
using Microsoft.Extensions.Options;
using AmazonFarmerAPI.Extensions;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class BannerController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        private IConfiguration _configuration;
        private readonly NotificationService _notificationService;
        private WsdlConfig _wsdlConfig;
        public BannerController(IRepositoryWrapper repoWrapper, IConfiguration configuration,
                NotificationService notificationService, IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _configuration = configuration;
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
        }

        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpPost("getBanners")]
        public async Task<APIResponse> getBanners(LanguageReq req)
        {

            //await AmazonFarmer.WSDL.Class1.CreateCustomerWSDLAsync();

            APIResponse resp = new APIResponse();
            try
            {
                // Fetch banners asynchronously using repository.
                List<tblBanner> bannersList = await _repoWrapper.BannerRepo.getBanners();
                if (bannersList != null && bannersList.Count() > 0)
                {
                    //Setting Data on DTO
                    resp.response = bannersList.Where(x => x.BannerType == EBannerType.loginScreen && x.Status == EActivityStatus.Active)
                        .FirstOrDefault()
                        .BannerLanguages
                        .Where(x => x.LanguageCode == req.languageCode && x.Status == EActivityStatus.Active)
                        .Select(x => new BannerDTO
                        {
                            bannerName = string.Empty,
                            filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:AdminBaseURL"),x.Image)
                        }).ToList();
                    //resp.response = await _repoWrapper.BannerRepo.getBanners(req);
                }
                else
                {
                    resp.response = new List<tblBanner>();
                }

            }
            catch (Exception ex)
            {
                // If an exception occurs, set response properties accordingly.
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        //private async Task DoSomething()
        //{
        //    Thread.Sleep(60000);
        //    return;
        //}
        //private async Task DoSomethingAgain()
        //{
        //    Thread.Sleep(60000);
        //    return;
        //}

        //// Endpoint for retrieving banners. It allows anonymous access.
        //[AllowAnonymous]
        //[HttpGet("ThreadTestToBeRemoved")]
        //public async Task<APIResponse> ThreadTestToBeRemoved()
        //{
        //    Thread email = new(delegate ()
        //    {
        //        DoSomething();

        //        int id = 1;
        //        if(id == 2)
        //        {

        //        }
        //        DoSomething();

        //    });
        //    email.IsBackground = true;
        //    email.Start();
        //    return new APIResponse();
        //}

        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpGet("CallingCustomerCreateWsdlSample")]
        public async Task<APIResponse> CallingCustomerCreateWsdlSample()
        { 
            var request = new ZSD_AMAZ_ORDER_INV_DETAILS
            {
               I_AUBEL = "257750",
               I_VBELN = ""
            };

            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            ZSD_AMAZ_ORDER_INV_DETAILSResponse? wsdlResponse = await wSDLFunctions.InvoiceDetailsRequest(request);


            APIResponse aPIResponse = new APIResponse();
            aPIResponse.response = wsdlResponse;
            aPIResponse.isError = false;
            aPIResponse.message = "Records Fetched";
            return aPIResponse;
        }

        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpGet("CallingChangeCustomerWsdlSample")]
        public async Task<APIResponse> CallingChangeCustomerWsdlSample()
        {


            var profile = new
            {
                CNICNumber = "42101-5207174-7",
                District = new { Name = "HYDERABAD" },
                CellNumber = "+92 321-9277411",
                NTNNumber = "123456",
                Address1 = "Badar Commercial",
                Address2 = "Defence Phase 6",
                STRNNumber = "123456",
                SAPCode = ""
            };

            var user = new
            {
                FirstName = "Zahid",
                LastName = "Hussain",
                Email = "Zahid.hussain@techsurgeinc.com",
                PhoneNumber = "+92 321-9277411"
            };

            var request = new ChangeCustomer.RequestType
            {
                city = "ZZ",
                condGrp1 = "ZZ",
                condGrp2 = "ZZ",
                condGrp3 = "ZZ",
                condGrp4 = "ZZ",
                district = profile.District.Name,
                email = user.Email,
                fax = "ZZ",
                mobileNum = profile.CellNumber,
                name = user.FirstName + " " + user.LastName,
                phoneNum = user.PhoneNumber,
                postalCode = "ZZ",
                salePoint = "ZZ",
                searchTerm1 = "ZZ",
                searchTerm2 = "ZZ",
                street = profile.Address1,
                street2 = profile.Address2,
                street4 = "",
                custNum = profile.SAPCode

            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            ChangeCustomer.ResponseType? wsdlResponse = await wSDLFunctions.ChangeCustomerWSDLAsync(request);


            APIResponse aPIResponse = new APIResponse();
            aPIResponse.response = wsdlResponse;
            aPIResponse.isError = false;
            aPIResponse.message = "Records Fetched";
            return aPIResponse;

        }
        // Endpoint for retrieving banners. It allows anonymous access.
        [AllowAnonymous]
        [HttpGet("CallingPriveSimulateWsdlSample")]
        public async Task<APIResponse> CallingPriveSimulateWsdlSample() 
        {

            SimulatePrice.RequestType request = new()
            {
                condGp1 = "",
                condGp2 = "",
                condGp3 = "",
                condGp4 = "",
                custNum = "45000037",
                custRef = "Created Plan for Farm",
                division = "02",
                matNum = "10000001",
                saleDistict = "Z00239",
                salesOrg = "2000",
                saleUnit = "BAG"
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            SimulatePrice.ResponseType? wsdlResponse = await wSDLFunctions.PriceSimluate(request);


            APIResponse aPIResponse = new APIResponse();
            aPIResponse.response = wsdlResponse;
            aPIResponse.isError = false;
            aPIResponse.message = "Records Fetched";
            return aPIResponse;

        }

    }
}
