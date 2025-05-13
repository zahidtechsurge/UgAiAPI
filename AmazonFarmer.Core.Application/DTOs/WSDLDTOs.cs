using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class PriceSimulateInput
    {
        public string SalesOrg { get; set; }
        //Customer Code  Of Farmer
        public string CustomerNumber { get; set; }
        public string CustomerReference { get; set; }
        public string MaterialCode { get; set; }
        public string UOM { get; set; }
        public string SalesDistrict { get; set; }
        public string Service1 { get; set; }
        public string Service2 { get; set; }
        public string Service3 { get; set; }
        public string Service4 { get; set; }
    }
    public class PlanCropProductPrice
    {
        public int PlanCropId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitTax { get; set; }
        public decimal UnitTotalAmount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string ProductCode { get; set; }
        public decimal AdvancePercentValue { get; set; }
        public List<PlanCropProductPrice_Services> Services { get; set; }
    }

    public class PlanCropProductPrice_Services
    {
        public string ServiceCode { get; set; }
    }
    public class PlanCropProductPrice_ServicesInt
    {
        public int ServiceID{ get; set; }
    }
    public class ProductPrice
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitTax { get; set; }
        public decimal UnitTotalAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }


    public partial class TaxCertificateRequest
    {
        public string? sapFarmerCode { get; set; } = string.Empty;
    }
    
    public class SalesTaxCertificateRequest
    {
        public string? InvoiceNumber { get; set; } = string.Empty;
        public string? OrderNo { get; set; } = string.Empty;
        public string? SAPFarmerCode { get; set; } = string.Empty;
    }

    public class ProductSalesOrgInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = "7th The Harbor Front Building, HC # 3, Marine Drive, Block 4";
        public string LogoBase { get; set; } = string.Empty;
    }

    public class SalesTaxCertificateResponse
    {
        
    }


}
