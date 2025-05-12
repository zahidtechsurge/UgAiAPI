using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;  // Importing necessary namespaces
using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class farm_SumitForApproval
    {
        public int farmID { get; set; }
        public bool isAllDataCorrect { get; set; }
        public bool canUseData { get; set; }
    }
    /// <summary>
    /// DTO representing farm details.
    /// </summary>
    public class FarmDTO
    {
        // Application ID associated with the farm
        public int? applicationID { get; set; }

        // Flag indicating if it's the primary farm, default is false
        //public bool isPrimary { get; set; } = false;

        // ID of the farm
        public int farmID { get; set; }
        // Name of the farm
        public string farmName { get; set; }

        // Address line 1 of the farm
        public string address1 { get; set; }

        // Address line 2 of the farm, default is an empty string
        public string address2 { get; set; } = string.Empty;

        // Address line 3 of the farm, default is an empty string
        public string? address3 { get; set; } = string.Empty;

        // ID of the city where the farm is located
        public int cityID { get; set; }

        // ID of the tehsil where the farm is located
        public int tehsilID { get; set; }

        // ID of the district where the farm is located
        public int districtID { get; set; }

        // Acreage of the farm
        public int acreage { get; set; }

        // Flag indicating if the farm is leased, default is false
        public bool isLeased { get; set; } = false;

        // Latitude of the farm (Added By Kamran)
        public double? latitude { get; set; }

        // Longitude of the farm (Added By Kamran)
        public double? longitude { get; set; }

        // Type of farm request, default is Draft
        //public EFarmStatus? requestType { get; set; } = EFarmStatus.Draft;

        // List of attachments associated with the farm
        public List<uploadAttachmentResp> attachments { get; set; }
        public string farmerComment { get; set; } = string.Empty;
    }
    /// <summary>
    /// DTO representing the address details of a farm.
    /// </summary>
    public class FarmAddressDTO
    {
        // Name associated with the farm address, default is an empty string
        public string Name { get; set; } = string.Empty;

        // Address line 1 of the farm, default is an empty string
        public string Address1 { get; set; } = string.Empty;

        // Address line 2 of the farm, default is an empty string
        public string Address2 { get; set; } = string.Empty;

        // District where the farm is located, default is an empty string
        public string District { get; set; } = string.Empty;

        // City where the farm is located, default is an empty string
        public string City { get; set; } = string.Empty;

        // Tehsil where the farm is located, default is an empty string
        public string Tehsil { get; set; } = string.Empty;
    }
    /// <summary>
    /// Request DTO for retrieving a list of farms.
    /// </summary>
    public class farmsList_Req
    {
        // List of farms to be retrieved
        public List<FarmDTO> farms { get; set; }

        // Indicates whether all information related to farms is correct
        public bool isAllInformationCorrect { get; set; }

        // Get applicationID if the application is saved already
        public string applicationID { get; set; }

        // Indicates whether the data can be used
        public bool canUseData { get; set; }

        // Specifies the request type for farms
        public EFarmStatus requestType { get; set; }
    }
    /// <summary>
    /// Response DTO for farm setup.
    /// </summary>
    public class farmSetup_Resp
    {
        /// <summary>
        /// The ID of the farm application
        /// </summary>
        public string applicationID { get; set; }
        /// <summary>
        /// request type id
        /// 1 Pending at TSO
        /// 3 Pending at Patwari
        /// 4 Approved
        /// </summary>
        public int requestTypeID { get; set; }
    }
    /// <summary>
    /// Response DTO for farms.
    /// </summary>
    public class farms_Resp
    {
        // The ID of the farm
        public int farmID { get; set; }

        // The name of the farm
        public string farmName { get; set; }
        // The address of the farm
        public string address { get; set; }

        // The acreage of the farm
        public int acreage { get; set; }
        public int rabiAcreage { get; set; } = 0;
        public int kharifAcreage { get; set; } = 0;

        // Indicates if the farm is primary
        public bool isPrimary { get; set; }

        // Indicates if the farm is approved
        public bool isApproved { get; set; }
    }
    public class farms_Request
    {
        // The ID of the Plan
        public int planID { get; set; }
    }
    public class FarmPlanAcreage
    {
        // The ID of the farm
        public int planID { get; set; }

        // The name of the farm
        public decimal Acreage { get; set; }
    }
    /// <summary>
    /// Response DTO for farm requests.
    /// </summary>
    public class farmRequest_Resp
    {
        // The ID of the request
        public int requestID { get; set; }

        // The ID of the farm
        public int? farmID { get; set; }

        // The name of the farm
        public string farmName { get; set; }

        // The acreage of the farm
        public double acreage { get; set; }

        // Indicates if the farm is primary
        public bool isPrimary { get; set; }

        // Indicates if the farm is approved
        public bool isApproved { get; set; }
    }
    /// <summary>
    /// Request DTO for farm verification.
    /// </summary>
    public class farmVerification_Req
    {
        // The ID of the farm
        public int farmID { get; set; }

        // The ID of the request
        public int requestID { get; set; }
    }

    /// <summary>
    /// Request DTO for farm approval acknowledgement.
    /// </summary>
    public class farmApprovalAcknowledgement_Req
    {
        // The application ID
        public string applicationID { get; set; }
    }

    /// <summary>
    /// DTO for farm application list.
    /// </summary>
    public class farmApplication_List
    {
        // The application ID
        public string applicationID { get; set; }

        // The name of the farmer
        public string farmerName { get; set; }

        // The name of the farmer CNIC
        public string farmerCNIC { get; set; }

        // The name of the farmer Phone Number
        public string farmerPhone { get; set; }
    }

    /// <summary>
    /// DTO for getting farm location.
    /// </summary>
    public class getFarmLocation
    {
        // The latitude coordinate of the farm
        public double latitude { get; set; }

        // The longitude coordinate of the farm
        public double longitude { get; set; }
    }

    /// <summary>
    /// Request DTO for changing farm registration status.
    /// </summary>
    public class farmRegistrationRequest_ChangeStatus_Req
    {
        // The ID of the farm
        public string farmID { get; set; }

        // The status ID
        public int statusID { get; set; }

        // The reason ID
        public int reasonID { get; set; }

        // The reverted reason (if the request is reverted)
        public string revertedReason { get; set; }
    }
    /// <summary>
    /// Response DTO for farm registration request.
    /// </summary>
    public class farmRegistrationRequest_Resp
    {
        // The application ID
        public int? applicationID { get; set; }

        // The farm ID
        public int? farmID { get; set; }

        // Indicates if the farm is primary
        public bool isPrimary { get; set; } = false;

        // The name of the farm
        public string farmName { get; set; }

        // The first address line of the farm
        public string address1 { get; set; }

        // The second address line of the farm
        public string address2 { get; set; } = string.Empty;

        // The ID of the city where the farm is located
        public int cityID { get; set; }

        // The Name of the city where the farm is located
        public string city { get; set; }

        // The ID of the tehsil where the farm is located
        public int tehsilID { get; set; }

        // The Name of the tehsil where the farm is located
        public string tehsil { get; set; }

        // The ID of the district where the farm is located
        public int districtID { get; set; }

        // The Name of the district where the farm is located
        public string district { get; set; }

        // The acreage of the farm
        public int acreage { get; set; }

        // Indicates if the farm is leased
        public bool isLeased { get; set; } = false;

        // The latitude coordinate of the farm
        public double? latitude { get; set; } // Added By Kamran

        // The longitude coordinate of the farm
        public double? longitude { get; set; }//Added By Kamran

        // The statusID of the farm
        public int statusID { get; set; }

        // The request type of the farm
        public EFarmStatus requestType { get; set; } = EFarmStatus.Draft;

        // List of attachments associated with the farm
        public List<uploadAttachmentResp> attachments { get; set; }
        //DTO of farmer Details 
        public farmerProfileDTO farmerProfile { get; set; }
    }

    /// <summary>
    /// Request DTO for getting farm Details
    /// </summary>
    public class farmDetail_Req
    {
        public int farmID { get; set; }
    }
    public class farmDetail_Resp
    {
        public bool canSetPrimary { get; set; } = false;
        public int cityID { get; set; }
        public int districtID { get; set; }
        public int tehsilID { get; set; }
        public int farmID { get; set; }
        public string farmName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string city { get; set; }
        public string tehsil { get; set; }
        public string district { get; set; }
        public int acreage { get; set; }
        public bool isLeased { get; set; }
        public bool isPrimary { get; set; }
        public int status { get; set; }
        public string statusDescription { get; set; }
        public bool isApproved { get; set; } = false;
        public string sapFarmID { get; set; }
        public string revertedReason { get; set; }
        public string farmerComment { get; set; }
        public List<uploadAttachmentResp> attachmentGUID { get; set; }
        public farmerProfileDTO? farmerProfile { get; set; }
    }
    public class farmerScreen_farmDetail_Resp
    {
        public int cityID { get; set; }
        public int districtID { get; set; }
        public int tehsilID { get; set; }
        public int farmID { get; set; }
        public string farmName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public double? latitude { get; set; } = null;
        public double? longitude { get; set; } = null;
        public string city { get; set; }
        public string tehsil { get; set; }
        public string district { get; set; }
        public int acreage { get; set; }
        public bool isLeased { get; set; }
        public bool isPrimary { get; set; }
        public EFarmStatus status { get; set; }
        public string sapFarmID { get; set; }
        public string farmerComment { get; set; }
        public farmerProfileDTO farmerProfile { get; set; }
        public List<uploadAttachmentResp> attachments { get; set; }
    }
    public class getFarmApplications_Req
    {/// <summary>search by Application ID , FarmerName </summary>
        public string search { get; set; }
        /// <summary>
        /// request type id
        /// 1 Pending at TSO
        /// 3 Pending at Patwari
        /// 4 Approved
        /// </summary>
        public int requestTypeID { get; set; } = 0;
        /// <summary>Skipping Values</summary>
        public int skip { get; set; }
        /// <summary>Taking Values</summary>
        public int take { get; set; }
        /// <summary>Start Date Template: yyyy-mm-dd</summary>
        public string startDate { get; set; }
        /// <summary>End Date Template: yyyy-mm-dd</summary>
        public string endDate { get; set; }
    }
    public class DraftedFarmDTO
    {
        // Application ID associated with the farm
        public int? applicationID { get; set; }

        // Flag indicating if it's the primary farm, default is false
        public bool isPrimary { get; set; } = false;

        // ID of the farm
        public int farmID { get; set; }
        // Name of the farm
        public string farmName { get; set; }

        // Address line 1 of the farm
        public string address1 { get; set; }

        // Address line 2 of the farm, default is an empty string
        public string address2 { get; set; } = string.Empty;

        // ID of the city where the farm is located
        public int cityID { get; set; }
        // Name of the city where the farm is located
        public string city { get; set; }

        // ID of the tehsil where the farm is located
        public int tehsilID { get; set; }
        // Name of the tehsil where the farm is located
        public string tehsil { get; set; }

        // ID of the district where the farm is located
        public int districtID { get; set; }
        // Name of the district where the farm is located
        public string district { get; set; }

        // Acreage of the farm
        public int acreage { get; set; }

        // Flag indicating if the farm is leased, default is false
        public bool isLeased { get; set; } = false;

        // List of attachments associated with the farm
        public List<uploadAttachmentResp> attachments { get; set; }
    }
}
