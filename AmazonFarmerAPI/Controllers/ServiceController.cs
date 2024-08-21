using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmerAPI.Extensions;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Google.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System; // Added to use Exception class
using System.Globalization;
using System.IdentityModel.Claims;
using System.Numerics;
using System.Text;
using System.Threading.Tasks; // Added to use Task

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor to inject IRepositoryWrapper dependency
        public ServiceController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Action method to get services
        [HttpGet("getServices")]
        public async Task<APIResponse> getServices()
        {
            APIResponse resp = new APIResponse();
                // Create LanguageReq object with language code obtained from User's claims
                //LanguageReq req = new LanguageReq() { languageCode = User.FindFirst("languageCode")?.Value };
                getServicesRequestDTO req = new getServicesRequestDTO()
                {
                    languageCode = User.FindFirst("languageCode")?.Value,
                    basePath = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")
                };

                // Call repository method to get services by language ID and specified configuration value
                resp.response = await _repoWrapper.ServiceRepo.getServicesByLanguageID(req, Convert.ToInt32(ConfigExntension.GetConfigurationValue("productSettings:ServicePostDeliveryIn")));
            return resp;
        }



        [HttpPost("getServiceReport")]
        public async Task<APIResponse> getServiceReport(getServiceReportRequest req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp pagResp = new pagination_Resp();

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var languageCode = User.FindFirst("languageCode")?.Value;
            if (userID == null || languageCode == null || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
            IQueryable<tblPlan> plans = await _repoWrapper.PlanRepo.getPlanOrderServices(userID);

            pagResp.totalRecord = plans.Count();
            plans = plans.OrderByDescending(x => x.ID).Skip(req.skip).Take(req.take);
            pagResp.filteredRecord = plans.Count();

            var planListing = plans.ToList();

            pagResp.list = planListing
                .Select(plan => new getServiceReportResponse
                {
                    // Create getServiceReportRequest with padded planID and status
                    planID = plan.ID.ToString().PadLeft(10, '0'),  // Convert plan ID to a 10-character string
                    statusID = (int)plan.Status,                   // Convert plan status to integer

                    // Map order services to a list of getServiceReport_ServiceList
                    services = plan.OrderServices.GroupBy(g => g.ServiceID).Select(os => new getServiceReport_ServiceList
                    {
                        serviceID = os.Max(g => g.ServiceID),
                        service = os.First().Service.ServiceTranslations
                            .Where(x => x.LanguageCode == languageCode)
                            .FirstOrDefault()?.Text,
                        filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), os.First().Service.ServiceTranslations
                            .Where(x => x.LanguageCode == languageCode)
                            .FirstOrDefault()?.Image.Replace("/", "%2F").Replace(" ", "%20")),
                        completeDate = os.Max(g => g.CompletedDate),          // Service completion date
                        scheduleDate = os.Max(g => g.ScehduledDate),          // Service scheduled date
                        remarks = os.Max(g => g.Remarks) ?? string.Empty,     // Service remarks, defaulting to empty string if null
                        requestStatus = os.Max(g => g.RequestStatus) != null ? (int)os.Max(g => g.RequestStatus.Value) : 0, // Convert request status to integer
                        status = os.Max(g => g.VendorStatus) != null ? (int)os.Max(g => g.VendorStatus.Value) : 0                   // Convert service status to integer
                    }).ToList()
                })
                .ToList();


            resp.response = pagResp;




            return resp;
        }

        [HttpGet("getFarmdarReportByPlanID/{planID}")]
        public async Task<APIResponse> getFarmdarReport(string planID)
        {
            APIResponse resp = new APIResponse();
            getFarmdarReportByPlanIDResponse inResp = new getFarmdarReportByPlanIDResponse() { url = string.Empty };
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(planID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanForServiceReportByPlanID(Convert.ToInt32(planID), userID);
            if (plan != null)
            {
                string token = await loginFarmdar(farmerId: plan.Farm.Users.Id);
                if (!string.IsNullOrEmpty(token))
                {
                    inResp.url = generateFarmdarWebPageURL(plan.FarmID, plan.ID, token);
                }
            }


            resp.response = inResp;
            return resp;
        }

        [HttpGet("getAgriliftReportByPlanID/{planID}")]
        public async Task<APIResponse> getAgriliftReportByPlanID(string planID)
        {
            APIResponse resp = new APIResponse();
            getFarmdarReportByPlanIDResponse inResp = new getFarmdarReportByPlanIDResponse() { url = "" };
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(planID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanForServiceReportByPlanID(Convert.ToInt32(planID), userID);
            if (plan != null)
            {
                LoginServiceResp? loginResp = await loginAgrilift(
                    new agriliftServiceReq
                    {
                        farmerId = plan.Farm.Users.FarmerProfile.First().SAPFarmerCode,
                        farmId = plan.Farm.FarmID,
                        planId = plan.ID.ToString()
                    }
                    );
                if (loginResp != null && loginResp.dataObject != null && !string.IsNullOrEmpty(loginResp.dataObject.redirectionURL))
                {
                    inResp.url = loginResp.dataObject.redirectionURL;
                }
            }


            resp.response = inResp;
            return resp;
        }
        [HttpGet("getSoilSampleReportByPlanID/{planID}")]
        public async Task<APIResponse> getSoilSampleReportByPlanID(string planID)
        {
            APIResponse apiResp = new APIResponse();
            pagination_Resp pagResp = new pagination_Resp();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            tblPlan plan = await _repoWrapper.PlanRepo.getPlanForServiceReportByPlanID(Convert.ToInt32(planID), userID);
            if (plan != null)
            {
                string startsWith = string.Concat(
                    plan.Farm.Users.FarmerProfile.FirstOrDefault().SAPFarmerCode,
                    "_",
                    plan.Farm.FarmID,
                    "_",
                    plan.ID,
                    "_"
                    );
                pagResp = await getServiceReportListing(startsWith);
                apiResp.response = pagResp;
            }

            return apiResp;
        }
        [HttpGet("")]
        /// <summary>
        /// login Afrilift for Drone Footage
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Return a nullable object of <see cref="LoginServiceResp"/> if the request is successfully loggedIn.</returns>
        /// <exception cref="AmazonFarmerException"></exception>
        private async Task<LoginServiceResp?> loginAgrilift(agriliftServiceReq req)
        {
            LoginServiceResp? resp = new();
            string apiUrl = ConfigExntension.GetConfigurationValue("agrilift:loginURL");

            var payload = new agriliftInitialRequest()
            {
                farmerId = req.farmerId,
                planId = req.planId,
                farmId = req.farmId,
                userEmail = ConfigExntension.GetConfigurationValue("agrilift:username"),
                password = ConfigExntension.GetConfigurationValue("agrilift:password")
            };

            var jsonContent = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AmazonFarmerException(_exceptions.agriliftRetrieveFailure);
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<LoginServiceResp>(responseBody);
                if (resp == null)
                    throw new AmazonFarmerException(_exceptions.agriliftDeserializationError);
                else if (resp.statusCode == "116")
                    throw new AmazonFarmerException(_exceptions.agrilift116);
                else if (resp.statusCode != "111")
                    throw new AmazonFarmerException(resp.message);
            }
            return resp;
        }

        /// <summary>
        /// login Farmdar for Geofencing
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns>Will be returning token if farmerID is valid</returns>
        /// <exception cref="AmazonFarmerException"></exception>
        private async Task<string> loginFarmdar(string farmerId)
        {
            string token = string.Empty;
            string apiUrl = ConfigExntension.GetConfigurationValue("farmdar:loginURL");
            var payload = new farmDarInitialRequest()
            {
                FarmerId = farmerId,
                Username = ConfigExntension.GetConfigurationValue("farmdar:username"),
                Password = ConfigExntension.GetConfigurationValue("farmdar:password")
            };
            var jsonContent = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AmazonFarmerException(_exceptions.farmdarLoginFailure);
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<loginDataObject>(responseBody);
                if (resp != null && !string.IsNullOrEmpty(resp.token))
                {
                    token = resp.token;
                }
            }
            return token;
        }

        /// <summary>
        /// Get farmdar URL for webview
        /// </summary>
        /// <param name="farmId"></param>
        /// <param name="planId"></param>
        /// <param name="token"></param>
        /// <remarks>
        /// concating the URL with token, farmID, planID
        /// </remarks>
        /// <returns><see cref="string"/> URL</returns>
        private string generateFarmdarWebPageURL(int farmId, int planId, string token)
        {
            return string.Concat(ConfigExntension.GetConfigurationValue("farmdar:webviewURL"), $"?token={token}&farmId={farmId}&planId={planId}");
        }
        /// <summary>
        /// Retrieves soil sample file list from an SFTP server and returns paginated file metadata.
        /// </summary>
        /// <param name="skip">Number of files to skip for pagination.</param>
        /// <param name="take">Number of files to take for pagination.</param>
        /// <returns>Paginated response <see cref="pagination_Resp"/> containing a list of soil sample files.</returns>
        private async Task<pagination_Resp> getServiceReportListing(string startsWith)
        {
            // Initialize the pagination response object
            pagination_Resp pagResp = new pagination_Resp();
            List<getSoilSampleList> lst = new List<getSoilSampleList>();
            // Retrieve SFTP connection details from configuration
            //var connectionString = ConfigExntension.GetConfigurationValue("AzureFileStorage:ConnectionString");
            //var shareName = ConfigExntension.GetConfigurationValue("AzureFileStorage:ShareName");
            //var directoryName = ConfigExntension.GetConfigurationValue("AzureFileStorage:ServiceReportDirectory");
            try
            {
                //ShareClient shareClient = new ShareClient(connectionString, shareName);
                //ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(directoryName);

                //lst = await ListFilesAndDirectoriesAsync(directoryClient, startsWith, directoryName);
                lst = ListFilesAndDirectoriesFromFTP(startsWith);

                pagResp.list = lst;
                pagResp.totalRecord = lst.Count();
                pagResp.filteredRecord = lst.Count();
            }
            catch (Exception)
            {
                throw;
            }

            return pagResp;
        }

        private static List<getSoilSampleList> ListFilesAndDirectoriesFromFTP(string startsWith)
        {
            List<getSoilSampleList> reportFiles = new List<getSoilSampleList>();
            string currentDirectory = Directory.GetCurrentDirectory();
            var inputDirectory = currentDirectory + ConfigExntension.GetConfigurationValue("FTPFileStorage:InputDirectory");
            var archiveDirectory = currentDirectory + ConfigExntension.GetConfigurationValue("FTPFileStorage:ArchiveDirectory");

            var host = ConfigExntension.GetConfigurationValue("FTPFileStorage:host");
            var port = ConfigExntension.GetConfigurationValue("FTPFileStorage:port");
            var username = ConfigExntension.GetConfigurationValue("FTPFileStorage:username");
            var password = ConfigExntension.GetConfigurationValue("FTPFileStorage:password");
            var remoteDirectory = ConfigExntension.GetConfigurationValue("FTPFileStorage:ServerPath");
            var remoteArchiveDirectory = ConfigExntension.GetConfigurationValue("FTPFileStorage:ServerArchive");

            using (var sftp = new SftpClient(host, Convert.ToInt32(port), username, password))
            {
                // Connect to the SFTP server
                sftp.Connect();
                // Download a file
                var remoteFiles = sftp.ListDirectory(remoteDirectory)
                            .Where(file => file.IsRegularFile && file.Name.EndsWith(".pdf"))
                            .ToList();
                foreach (var file in remoteFiles)
                {
                    if (file.Name.ToLower().StartsWith(startsWith.ToLower()) && file.Name.ToLower().EndsWith(".pdf"))
                    {
                        getSoilSampleList ServiceReport = new getSoilSampleList()
                        {
                            fileName = file.Name.Replace((startsWith), "").Split("_")[0],
                            filePath = string.Concat(remoteArchiveDirectory, "/", file.Name),
                            fileType = ".pdf",
                            fileActualName = file.Name,
                            modifiedOn = ReturnDateTime(file.Name)
                        };
                        reportFiles.Add(ServiceReport);
                    }

                }
            }
            return reportFiles;

        }
        private static async Task<List<getSoilSampleList>> ListFilesAndDirectoriesAsync
            (ShareDirectoryClient directoryClient, string startsWith, string filePath)
        {
            List<getSoilSampleList> reportFiles = new List<getSoilSampleList>();
            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {

                if (!item.IsDirectory)
                {
                    if (item.Name.ToLower().StartsWith(startsWith.ToLower()) && item.Name.ToLower().EndsWith(".pdf"))
                    {
                        getSoilSampleList ServiceReport = new getSoilSampleList()
                        {
                            fileName = item.Name.Replace((startsWith), "").Split("_")[0],
                            filePath = string.Concat(filePath, "/", item.Name),
                            fileType = ".pdf",
                            fileActualName = item.Name,
                            modifiedOn = ReturnDateTime(item.Name)
                        };
                        reportFiles.Add(ServiceReport);
                    }
                }
            }
            return reportFiles;
        }
        private static  string ReturnDateTime(string FileName)
        {
            string formattedDateTime = string.Empty;
            if (FileName.Contains("_"))
            {
                string dateTimePart = FileName.Split('_')[^1].Split('.')[0];

                if (DateTime.TryParseExact(dateTimePart, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    formattedDateTime = dateTime.ToString("dd-MM-yyyy HH:mm:ss");
                }
                else
                {
                    return DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss");
                }
            }
            return formattedDateTime;

        }

    }
}
