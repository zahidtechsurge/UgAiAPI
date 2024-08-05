using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for Tehsil information.
    /// </summary>
    public class TehsilDTO
    {
        // The ID of the city associated with the tehsil
        public int cityID { get; set; }

        // The ID of the tehsil
        public int tehsilID { get; set; }

        // The name of the tehsil
        public string tehsilName { get; set; }

        // The language code for localization
        public string languageCode { get; set; }
    }

    /// <summary>
    /// Request DTO to get Tehsil information.
    /// </summary>
    public class getTehsil_Req
    {
        // The language code for localization
        public string languageCode { get; set; }
    }
}
