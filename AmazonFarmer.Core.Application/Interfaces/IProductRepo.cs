using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IProductRepo // Defining the interface for product repository
    {
        Task<List<categoryDTO_Resp>> getProductsByLangugageID(GetProductDTO_Internal_req req, int postDeliveryIn); // Method signature for retrieving products by language ID and post delivery in
        Task<ProductPrices_Resp?> getProductByProductID(int productID, string languageCode); // Method signature for retrieving product by product ID and language code
        Task<List<TblProduct>> getProductsByProductIDs(List<int> productIDs, string languageCode); // Method signature for retrieving product by product ID and language code
        Task<List<ProductListDTO>> getAllProducts();
        Task<List<TblOrderProducts>> getAllOrdersForProducts(List<int> productIds);
        IQueryable<TblProduct> getProducts();
        Task<List<tblUnitOfMeasure>> getUOMs();
        IQueryable<tblProductCategory> getCategories();
        IQueryable<tblProductCategoryTranslation> getCategoriesTranslation();
        Task<List<tblProductCategoryTranslation>> GetCategoryTranslationsByCatID(int CatID);
        void addCategory(tblProductCategory req);
        Task<tblProductCategory?> GetCategoryByID(int id);
        void updateCategory(tblProductCategory req);
        Task<tblProductCategoryTranslation?> getProductCategoryTranslationByCatID(int catID, string languageCode);
        Task<List<tblProductTranslation>> GetProductTranslationsByProductID(int productID);
        void UpdateProductTranslation(tblProductTranslation translation);
        void AddProductTranslation(tblProductTranslation translation);
        Task<tblProductTranslation?> GetProductTranslationById(int transID);
        Task<tblProductTranslation?> GetProductTranslationById(int productID, string languageCode);
        Task<TblProduct?> GetProductByID(int productID);
        Task<TblProduct?> GetProductByNameOrCode(string productName, string productCode);
        void AddProduct(TblProduct product);
        void UpdateProduct(TblProduct product);
    }
}
