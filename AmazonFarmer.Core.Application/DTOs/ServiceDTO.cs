using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class SoilSampleRequest
    {
        public int farmID { get; set; }
        public string selectedDate { get; set; } = string.Empty;
    }
    public class FFMSoilSampleAPIResponse
    {

    }

    /// <summary>
    /// Data Transfer Object (DTO) for Service information.
    /// </summary>
    public class ServiceDTO
    {
        // The ID of the service
        public int serviceID { get; set; }

        // The name of the service
        public string serviceName { get; set; } = string.Empty;

        // The file path for the service
        public string filePath { get; set; } = string.Empty;

        // The duration in days after delivery for the service
        public int postDeliveryIn { get; set; }
    }

    /// <summary>
    /// Request DTO for server information.
    /// </summary>
    public class Server_Req
    {
        // The ID of the plan service 
        public int planServiceID { get; set; }
        // The ID of the service
        public int serviceID { get; set; }

        // The start date of the service
        public string lastHarvestDate { get; set; } = string.Empty;
        // The start date of the service
        public string landPreparationDate { get; set; } = string.Empty;

        // The end date of the service
        public string sewingDate { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for server report.
    /// </summary>
    public class ServerReport_Resp
    {
        // The ID of the service
        public int serviceID { get; set; }
        // The name of the service
        public string service { get; set; } = string.Empty;

        // The date of the service
        public DateTime date { get; set; }

        // The status of the service
        public string status { get; set; } = string.Empty;
    }
    /// <summary>
    /// Request DTO for server report Detail.
    /// </summary>
    public class ServerReportDetail_Req
    {
        // The ID of the service report
        public int serviceReportID { get; set; }
    }
    public class ServerReportDetail_Resp
    {
        // The URL of the service report
        public string serviceReportURL { get; set; } = string.Empty;
    }
    public class agriliftServiceReq
    {
        public string farmerId { get; set; } = string.Empty;
        public int farmId { get; set; }
        public string planId { get; set; } = string.Empty;
    }
    public class agriliftInitialRequest : agriliftServiceReq
    {
        public string password { get; set; } = string.Empty;
        public string userEmail { get; set; } = string.Empty;
    }
    public class farmDarServiceReq
    {
        public string token { get; set; } = string.Empty;
        public int farmId { get; set; }
        public int planId { get; set; }
    }
    public class farmDarInitialRequest
    {
        public string FarmerId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
    public class LoginServiceResp
    {
        /// <summary>
        /// 111 : Success
        /// </summary>
        public string statusCode { get; set; } = string.Empty;
        public string messageCode { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public loginDataObject? dataObject { get; set; }
        public string? fileName { get; set; }
    }
    public class loginDataObject
    {
        public string token { get; set; } = string.Empty;
        public string redirectionURL { get; set; } = string.Empty;
    }
    public class getServiceReportResponse
    {
        public string planID { get; set; } = string.Empty;
        public int statusID { get; set; }
        public List<getServiceReport_ServiceList> services { get; set; } = new List<getServiceReport_ServiceList>();
    }
    public class getServiceReport_ServiceList
    {
        public int serviceID { get; set; }
        public string service { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public int status { get; set; }
        public DateTime? completeDate { get; set; }
        public DateTime? scheduleDate { get; set; }
        public string remarks { get; set; } = string.Empty;
        public int? requestStatus { get; set; }
    }
    public class getServiceReportRequest
    {
        public int skip { get; set; }
        public int take { get; set; }
        public string planID { get; set; } = string.Empty;
    }
    public class getFarmdarReportByPlanIDResponse
    {
        public string url { get; set; } = string.Empty;
    }
    public class getSoilSampleList
    {
        public string fileName { get; set; } = string.Empty;
        public string fileActualName { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public string fileType { get; set; } = string.Empty;
        public string modifiedOn { get; set; } = string.Empty;
    }
    public class AzureServiceReportFiles
    {
        public string fileName { get; set; } = string.Empty;
        public string reportDate { get; set; } = string.Empty;
    }
    public class getServicesRequestDTO : LanguageReq
    {
        public string basePath { get; set; } = string.Empty;
    }

    public class AddServiceRequest
    {
        public string serviceName { get; set; } = string.Empty;
        public string serviceCode { get; set; } = string.Empty;
    }
    public class UpdateServiceRequest : AddServiceRequest
    {
        public int serviceID { get; set; }
        public int status { get; set; }
    }
    public class AddServiceTranslationRequest
    {
        public int serviceID { get; set; }
        public string? content { get; set; }
        public string? fileName { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class UpdateServiceTranslationRequest : AddServiceTranslationRequest
    {
        public int translationID { get; set; }
        public string? filePath { get; set; }
    }
    public class GetServiceTranslationResponse : UpdateServiceTranslationRequest
    {
        public string language { get; set; } = string.Empty;
    }
}
