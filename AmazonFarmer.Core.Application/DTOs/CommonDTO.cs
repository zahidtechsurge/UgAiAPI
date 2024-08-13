using AmazonFarmer.Core.Domain.Entities;
using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// JSON Response DTO
    /// </summary>
    public class JSONResponse
    {
        public bool isError { get; set; } = false; // Property to indicate if there is an error, default is false
        public string message { get; set; } = string.Empty; // Property for message, default is an empty string
    }

    /// <summary>
    /// API Response DTO
    /// </summary>
    public class APIResponse
    {
        public bool isError { get; set; } = false; // Property to indicate if there is an error, default is false
        public string message { get; set; } = string.Empty; // Property for message, default is an empty string
        public dynamic response { get; set; } = string.Empty; // Property for dynamic response data, default is an empty string
    }

    public class pagination_Resp
    {
        public int totalRecord { get; set; } = 0;
        public int filteredRecord { get; set; } = 0;
        public dynamic? list { get; set; }
    }

    public class ReportPagination_Req : pagination_Req
    {
        public string sortColumn { get; set; } = string.Empty;
        public string sortOrder { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for dropdown values
    /// </summary>
    public class DropDownValues
    {
        //public string languageCode { get; set; } = string.Empty; // Property for language code, default is an empty string
        public dynamic? labelFor { get; set; } // Property for dropdown label
        public int value { get; set; } // Property for dropdown value
        public string label { get; set; } = string.Empty;// Property for dropdown key
    }


    public class pagination_Req
    {
        public int? rootID { get; set; } = 0;
        public string? search { get; set; } = string.Empty;
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
