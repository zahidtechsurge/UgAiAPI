using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities; 

namespace AmazonFarmer.Core.Application.DTOs
{
    public class FarmDTO
    {
        public int? applicationID { get; set; }
        public bool isPrimary { get; set; } = false;
        public string farmName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; } = string.Empty;
        public int cityID { get; set; }
        public int tehsilID { get; set; }
        public int districtID { get; set; }
        public int acreage { get; set; }
        public bool isLeased { get; set; } = false;
        public double? latitude { get; set; } // Added By Kamran
        public double? longitude { get; set; }//Added By Kamran
        public EFarmStatus requestType { get; set; } = EFarmStatus.Draft;
        public List<farmAttachment> attachments { get; set; }
    }
    public class FarmAddressDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Tehsil { get; set; } = string.Empty;
    } 
    public class farmsList_Req
    {
        public List<FarmDTO> farms { get; set; }
        public bool isAllInformationCorrect { get; set; }
        public bool canUseData { get; set; }
        public EFarmStatus requestType { get; set; }
    }
    public class farmSetup_Resp
    {
        public string applicationID { get; set; }
    }
    public class farms_Resp
    {
        public int farmID { get; set; }
        public string farmName { get; set; }
        public int acreage { get; set; }
        public bool isPrimary { get; set; }
        public bool isApproved { get; set; }
    }
    public class farmRequest_Resp
    {
        public int requestID { get; set; }
        public int? farmID { get; set; }
        public string farmName { get; set; }
        public double acreage { get; set; }
        public bool isPrimary { get; set; }
        public bool isApproved { get; set; }
    }
    public class farmVerification_Req
    {
        public int farmID { get; set; }
        public int requestID { get; set; }
    }
    public class farmApprovalAcknowledgement_Req
    {
        public string applicationID { get; set; }
    }
    public class farmApplication_List
    {
        public string applicationID { get; set; }
        public string farmerName { get; set; }
    }
    public class getFarmLocation
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class farmRegistrationRequest_ChangeStatus_Req
    {
        public string farmID { get; set; }
        public int statusID { get; set; }
    }
    public class farmRegistrationRequest_Resp
    {
        public int? applicationID { get; set; }
        public int? farmID { get; set; }
        public bool isPrimary { get; set; } = false;
        public string farmName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; } = string.Empty;
        public int cityID { get; set; }
        public int tehsilID { get; set; }
        public int districtID { get; set; }
        public int acreage { get; set; }
        public bool isLeased { get; set; } = false;
        public double? latitude { get; set; } // Added By Kamran
        public double? longitude { get; set; }//Added By Kamran
        public EFarmStatus requestType { get; set; } = EFarmStatus.Draft;
        public List<farmAttachment> attachments { get; set; }
    }
}
