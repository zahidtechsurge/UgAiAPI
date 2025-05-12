using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// DTO for consumption matrix
    /// </summary>
    public class ConsumptionMatrixDTO
    {
        public decimal qty { get; set; } // Property for quantity
        public string uom { get; set; } = string.Empty;// Property for unit of measure
        public string name { get; set; } = string.Empty; // Property for name
    }
    public class AddConsumptionMatrix
    {
        public decimal qty { get; set; } // Property for quantity
        public string uom { get; set; } = string.Empty;// Property for unit of measure
        public int productID { get; set; } // Property for name
        public int cropID { get; set; } // Property for name
        public int? territoryID { get; set; } // Property for name
    }
    public class UpdateConsumptionMatrix : AddConsumptionMatrix
    {
        public int pcmID { get; set; } // Property for ID
    }
    public class GetConsumptionMatrixResponse : UpdateConsumptionMatrix
    {
        public string product { get; set; } = string.Empty;
        public string crop { get; set; } = string.Empty;
    }
}
