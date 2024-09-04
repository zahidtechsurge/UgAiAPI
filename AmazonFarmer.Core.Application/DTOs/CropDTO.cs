using System; // Importing necessary namespaces
using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Request DTO for retrieving crops
    /// </summary>
    public class CropDTO_req
    {
        public int seasonID { get; set; } // Property for season ID
        public int farmID { get; set; } // Property for farm ID
    }

    /// <summary>
    /// Internal request DTO for retrieving crops
    /// </summary>
    public class GetCropDTO_Internal_req
    {
        public int seasonID { get; set; } // Property for season ID
        public int farmID { get; set; } // Property for farm ID
        public string languageCode { get; set; } = string.Empty; // Property for language code
        public string basePath { get; set; } = string.Empty; // Property for language code
    }

    /// <summary>
    /// Response DTO for crops
    /// </summary>
    public class Crops_Res
    {
        public int cropID { get; set; } // Property for crop ID
        public string cropName { get; set; } = string.Empty; // Property for crop name
        public string filePath { get; set; } = string.Empty; // Property for file path
        public List<ConsumptionMatrixDTO> suggestion { get; set; } = []; // Property for suggestion list
    }

    /// <summary>
    /// Request DTO for planning crops
    /// </summary>
    public class PlanCrop_Req
    {
        public int planCropID { get; set; } // Property for plan crop ID
        //public int cropID { get; set; } // Property for crop ID
        public List<int> cropIDs { get; set; } = [];// Property for crop ID
        public double crop_acreage { get; set; } // Property for crop acreage
        public List<addCropPlan_Req> products { get; set; } = []; // Property for products list
        public List<Server_Req> services { get; set; } = []; // Property for services list
    }
    public class AddCropRequest
    {
        public string cropName { get; set; } = string.Empty;
        public int status { get; set; }
    }
    public class UpdateCropRequest: AddCropRequest
    {
        public int cropID { get; set; }
    }
    public class GetCropTranslationsResponse : UpdateCropTranslationRequest
    {
        public string language { get; set; } = string.Empty;
    }
    public class AddCropTranslationRequest
    {
        public int cropID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public string? fileName { get; set; } = string.Empty;
        public string? content { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class UpdateCropTranslationRequest : AddCropTranslationRequest
    {
        public int translationID { get; set; }
    }
    public class GetCropTimingsResponse : UpdateCropTiming
    {
        public string cropName { get; set; } = string.Empty;
        public string seasonName { get; set; } = string.Empty;
        public string districtName { get; set; } = string.Empty;
    }
    public class AddCropTiming : CropTimingValues
    {
        public int seasonID { get; set; }
        public int districtID { get; set; }
    }
    public class UpdateCropTiming : AddCropTiming
    {
        public int recID { get; set; }
        public int statusID { get; set; }
    }
    public class CropTimingValues
    {
        public int cropID { get; set; }
        public int fromMonth { get; set; }
        public int toMonth { get; set; }

    }


}
