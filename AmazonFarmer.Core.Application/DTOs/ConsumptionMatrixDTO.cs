using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// DTO for consumption matrix
    /// </summary>
    public class ConsumptionMatrixDTO
    {
        public decimal qty { get; set; } // Property for quantity
        public string uom { get; set; } // Property for unit of measure
        public string name { get; set; } // Property for name
    }
}
