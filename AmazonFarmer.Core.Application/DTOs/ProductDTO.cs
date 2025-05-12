namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Response Data Transfer Object (DTO) for product information.
    /// </summary>
    public class ProductDTO_Resp
    {
        // The ID of the product
        public int productID { get; set; }

        // The name of the product
        public string productName { get; set; }

        // The file path for the product
        public string filePath { get; set; }

        // The duration after delivery
        public int postDeliveryIn { get; set; }

        // The Unit of measure
        public string uom { get; set; }
    }
    /// <summary>
    /// Response Data Transfer Object (DTO) for product information.
    /// </summary>
    public class ProductListDTO
    {
        // The ID of the product
        public int productID { get; set; }

        // The name of the product
        public string productName { get; set; }

        // The name of the product
        public string productCode { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for category information.
    /// </summary>
    public class categoryDTO_Resp
    {
        // The ID of the category
        public int categoryID { get; set; }

        // The name of the category
        public string categoryName { get; set; }

        // The file path for the category
        public string filePath { get; set; }

        // List of products belonging to this category
        public List<ProductDTO_Resp> products { get; set; }
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for product prices.
    /// </summary>
    public class ProductPrices
    {
        // The ID of the product
        public int productID { get; set; }

        // The quantity of the product
        public int qty { get; set; }
    }
    public class ProductPrices_Req
    {
        public int warehouseID { get; set; }
        public List<ProductPrices_Crop> crops { get; set; }
    }
    public class ProductPrices_Request
    {
        public int warehouseID { get; set; }
        public List<int> serviceIDs { get; set; }
        public List<ProductPrices> products { get; set; }
    }
    public class ProductPrices_Crop
    {
        public int cropID { get; set; }
        public List<ProductPrices> products { get; set; }
        public List<int> serviceIDs { get; set; }
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for adding a crop plan.
    /// </summary>
    public class addCropPlan_Req
    {
        // The ID of the plan product
        public int planProductID { get; set; }
        // The ID of the product
        public int productID { get; set; }

        // The quantity of the product
        public int qty { get; set; }

        // The delivery date of the product
        public string deliveryDate { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for product prices.
    /// </summary>
    public class ProductPrices_Resp
    {
        //public int productQTY { get; set; }
        // The ID of the product
        public int productID { get; set; }

        // The name of the product
        public string productName { get; set; }

        // The file path for the product
        public string filePath { get; set; }

        // The product code
        public string productCode { get; set; }

        // The price of the product
        public decimal productPrice { get; set; }
        // The unit price of the product
        public decimal productUnitPrice { get; set; }
        public List<int> serviceIDs { get; set; }
    }
    public class PrivateFunc_GetProductPrice
    {
        public List<string> serviceCode { get; set; } = [];
        public string sapFarmerCode { get; set; } = string.Empty;
        public string? productDivision { get; set; }
        public string productCode { get; set; }
        public int productQTY { get; set; }
        public string warehouseSalePoint { get; set; }
        public string productSalesOrg { get; set; }
        public string productUOM { get; set; }
    }
    public class PrivateFunc_GetMultiProductPrices
    {
        public List<string> serviceCode { get; set; } = [];
        public string sapFarmerCode { get; set; } = string.Empty;
        public List<PrivateFunc_GetProductPrice> Products { get; set; } = [];
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for getting product pricings.
    /// </summary>
    public class GetProductPricings_Resp
    {
        // The total price
        public decimal totalPrice { get; set; }

        // The advance amount
        public decimal advance { get; set; }

        // The advance price
        public decimal advancePrice { get; set; }

        // List of product pricings
        public List<ProductPrices_Resp> productPricings { get; set; }
    }



    public class ReconcilteOrderRequest
    {
        public List<int> ProductIDs { get; set; }
    }
    public class usedProductServices
    {
        public int productID { get; set; }
        public List<int> serviceIDs { get; set; }
    }
    public class AddProductsByAdmin
    {
        public required string name { get; set; }
        public required string code { get; set; }
        public required string salesOrg { get; set; }
        public required string division { get; set; }
        public required int uomID { get; set; }
        /// <summary>
        /// set Zero if the product is for all categories
        /// </summary>
        public int categoryID { get; set; } = 0;
    }
    public class UpdateProductsByAdmin : AddProductsByAdmin
    {
        public int productID { get; set; } = 0;
    }
    public class GetProductsByAdminRequest
    {

    }
    public class GetProductsByAdminResponse
    {
        public int productID { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string salesOrg { get; set; } = string.Empty;
        public string division { get; set; } = string.Empty;
        public string uom { get; set; } = string.Empty;
        public int status { get; set; } = 0;
        public List<ProductTranslationDTO> translations = new List<ProductTranslationDTO>();
    }
    public class ProductTranslationDTO
    {
        public int translationID { get; set; }
        public int productID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
    }
    public class GetProductUnitOfMeasuresByAdminResponse
    {
        public int uomID { get; set; }
        public string name { get; set; } = string.Empty;
        public int status { get; set; }
        public List<UnitOfMeasureTranslationDTO> translations = new List<UnitOfMeasureTranslationDTO>();
    }
    public class UnitOfMeasureTranslationDTO
    {
        public int translationID { get; set; }
        public int uomID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class GetProductCategoryByAdminResponse
    {
        public int categoryID { get; set; }
        public string name { get; set; } = string.Empty;
        public int status { get; set; }
        public List<ProductCategoryTranslationDTO> translations = new List<ProductCategoryTranslationDTO>();
    }
    public class ProductCategoryTranslationDTO
    {
        public int translationID { get; set; }
        public int categoryID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class AddProductCategoryByAdminRequest
    {
        public string name { get; set; } = string.Empty;
        public bool status { get; set; }
    }
    public class UpdateProductCategoryByAdminRequest : AddProductCategoryByAdminRequest
    {
        public int categoryID { get; set; }
    }
    public class SyncProductCategoryTranslationDTO
    {
        public int categoryID { get; set; }
        public required string languageCode { get; set; }
        public required string text { get; set; }
    }
    public class ProductCategoryTranslationResponse
    {
        public int translationID { get; set; }
        public int categoryID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class AddProductTranslationRequest
    {
        public int productID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
        public string? filePath { get; set; } = string.Empty;
        public string? fileName { get; set; } = string.Empty;
        public string? content { get; set; } = string.Empty;
    }
    public class UpdateProductTranslationRequest : AddProductTranslationRequest
    {
        public int translationID { get; set; }
    }
    public class GetProductTranslationResponse : UpdateProductTranslationRequest
    {
        public string language { get; set; } = string.Empty;
    }
    public class AddProductRequest
    {
        public int categoryID { get; set; }
        public string productName { get; set; } = string.Empty;
        public string productCode { get; set; } = string.Empty;
        public int uomID { get; set; }
        public string saleOrg { get; set; } = string.Empty;
        public string division { get; set; } = string.Empty;
    }
    public class UpdateProductRequest : AddProductRequest
    {
        public int productID { get; set; }
        public int status { get; set; }
    }
    public class GetProductRequest : UpdateProductRequest
    {
        public string uom { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public List<ProductTranslationDTO> translations = new List<ProductTranslationDTO>();
    }


}
