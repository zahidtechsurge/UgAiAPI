using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace AmazonFarmerAPI.Extensions
{
    public class reportAPIExtension
    {
        private readonly string _apiKey;
        public reportAPIExtension()
        {
            _apiKey = ConfigExntension.GetConfigurationValue("FFM:SoilSampleReport:APIKey");
        }

        public async Task<FFMSoilSampleAPIResponse?> RequestForSoilSampleReport(SoilSampleRequest Request)
        {
            FFMSoilSampleAPIResponse? resp = new FFMSoilSampleAPIResponse();
            string apiUrl = ($"{ConfigExntension.GetConfigurationValue("FFM:SoilSampleReport:BaseURL")}");
            var jsonContent = JsonConvert.SerializeObject(Request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("APIKEY", _apiKey);
                var response = await httpClient.PostAsync(apiUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AmazonFarmerException(_exceptions.ffmSoilSampleRequestRetrieveFailure);
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<FFMSoilSampleAPIResponse>(responseBody);
            }
            return resp;
        }
    }
}
