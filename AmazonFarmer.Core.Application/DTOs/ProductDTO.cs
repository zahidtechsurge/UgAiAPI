using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class ProductDTO_Resp
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public string filePath { get; set; }
        public int postDeliveryIn { get; set; }
    }
    public class categoryDTO_Resp
    {
        public int categoryID { get; set;}
        public string categoryName { get; set;}
        public string filePath { get; set;}
        public List<ProductDTO_Resp> products { get; set; }
    }
    public class ProductPrices_Req
    {
        public int productID { get; set; }
        public decimal qty { get; set; }
        //public string deliveryDate { get; set; }
    }
    public class addCropPlan_Req
    {
        public int productID { get; set; }
        public decimal qty { get; set; }
        public string deliveryDate { get; set; }

    }

    public class ProductPrices_Resp
    {
        public int productID { get; set;}
        public string productName { get; set;}
        public string filePath { get; set;}
        public string productCode { get; set;}
        public decimal productPrice { get; set;}
    }
    public class GetProductPricings_Resp
    {
        public decimal totalPrice { get; set; }
        public decimal advance { get; set; }
        public decimal advancePrice { get; set; }
        public List<ProductPrices_Resp> productPricings { get; set; }
    }
}
