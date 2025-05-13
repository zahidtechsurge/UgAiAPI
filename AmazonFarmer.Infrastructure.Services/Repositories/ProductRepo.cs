/*
   This class implements the IProductRepo interface and provides methods for retrieving product-related data from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private AmazonFarmerContext _context;

        // Constructor to initialize the ProductRepo with an instance of the AmazonFarmerContext
        public ProductRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to retrieve products by language ID along with their categories
        public async Task<List<categoryDTO_Resp>> getProductsByLangugageID(GetProductDTO_Internal_req req, int postDeliveryIn)
        {
            return await _context.ProductCategoryTranslation
                .Include(x => x.ProductCategory)
                    .ThenInclude(x => x.Products.Where(p => p.Active == EActivityStatus.Active))
                        .ThenInclude(x => x.ProductTranslations.Where(x => x.LanguageCode == req.languageCode))
                .Include(x => x.ProductCategory)
                    .ThenInclude(x => x.Products.Where(p => p.Active == EActivityStatus.Active))
                        .ThenInclude(x => x.UOM)
                            .ThenInclude(x => x.UnitOfMeasureTranslation.Where(x => x.LanguageCode == req.languageCode))
                .Where(x => 
                    x.LanguageCode == req.languageCode && 
                    x.ProductCategory.Status == EActivityStatus.Active && 
                    x.ProductCategory.Products.Where(p=>p.Active == EActivityStatus.Active).Count() > 0
                )
                .Select(x => new categoryDTO_Resp
                {
                    categoryID = x.ProductCategoryID,
                    categoryName = x.Text,
                    filePath = x.Image,
                    products = x.ProductCategory.Products.Where(pp=>pp.Active == EActivityStatus.Active).Select(x => new ProductDTO_Resp
                    {
                        postDeliveryIn = postDeliveryIn,
                        productID = x.ID,
                        filePath = string.Concat(req.basePath,
                        x.ProductTranslations.Where(pt => pt.LanguageCode == req.languageCode).FirstOrDefault().Image.Replace("/", "%2F").Replace(" ", "%20")), // Get product image icon from ProductTranslations
                        productName = x.ProductTranslations.Where(pt => pt.LanguageCode == req.languageCode).FirstOrDefault().Text, // Get product name from ProductTranslations
                        uom = x.UOM.UnitOfMeasureTranslation.FirstOrDefault().Text, // Get product name from ProductTranslations
                    }).ToList()

                }).ToListAsync();
        }
        // Method to retrieve products by language ID along with their categories
        public async Task<List<ProductListDTO>> getAllProducts()
        {
            return await _context.Products.Select(x => new ProductListDTO
            {
                productID = x.ID,
                productName = x.Name,
                productCode = x.ProductCode
            }).ToListAsync();
        }
        public async Task<List<TblProduct>> getProductsByProductIDs(List<int> productIDs, string languageCode)
        {
            return await _context.Products
                .Include(x => x.UOM)
                .Include(x => x.ProductTranslations.Where(x => x.LanguageCode == languageCode))
                .Where(x => productIDs.Contains(x.ID))
                .ToListAsync();
        }

        // Method to retrieve product details by product ID and language code
        public async Task<ProductPrices_Resp?> getProductByProductID(int productID, string languageCode)
        {
            return await _context.ProductTranslations
                .Include(x => x.Product)
                .Where(x => x.ProductID == productID && x.LanguageCode == languageCode)
                .Select(x => new ProductPrices_Resp
                {
                    productID = x.ProductID,
                    productCode = x.Product.ProductCode,
                    filePath = x.Image,
                    productName = x.Text,
                    productPrice = Convert.ToDecimal(10) // Placeholder value for product price
                }).FirstOrDefaultAsync();
        }

        public async Task<List<TblOrderProducts>> getAllOrdersForProducts(List<int> productIds)
        {
            return await _context.OrderProducts
                  .Include(op => op.Order).ThenInclude(o => o.Plan).ThenInclude(p => p.OrderServices).ThenInclude(os => os.Service)
                  .Include(op => op.Order).ThenInclude(o => o.Warehouse)
                  .Include(op => op.Order).ThenInclude(o => o.ChildOrders.Where(co => co.PaymentStatus == EOrderPaymentStatus.NonPaid))
                  .Include(op => op.Order).ThenInclude(o => o.User).ThenInclude(u => u.FarmerProfile)
                  .Include(op => op.Product).ThenInclude(p => p.UOM)
                  .Where(op => productIds.Contains(op.ProductID)
                      && (op.Order.PaymentStatus != EOrderPaymentStatus.NonPaid && op.Order.PaymentStatus != EOrderPaymentStatus.Refund)
                      && op.Order.OrderStatus != EOrderStatus.Deleted
                      && op.Order.DeliveryStatus != EDeliveryStatus.ShipmentComplete
                      ).ToListAsync();
        }
        public IQueryable<TblProduct> getProducts()
        {
            return _context.Products
                .Include(x => x.ProductTranslations)
                .Include(x => x.UOM)
                .Include(x => x.Category);
        }
        public async Task<List<tblUnitOfMeasure>> getUOMs()
        {
            return await _context.tblUnitOfMeasures.Include(x => x.UnitOfMeasureTranslation).ToListAsync();
        }
        public IQueryable<tblProductCategory> getCategories()
        {
            return _context.ProductCategory
                .Include(x => x.ProductCategoryTranslation);
        }
        public IQueryable<tblProductCategoryTranslation> getCategoriesTranslation()
        {
            return _context.ProductCategoryTranslation.Include(x => x.ProductCategory);
        }
        public async Task<List<tblProductCategoryTranslation>> GetCategoryTranslationsByCatID(int CatID)
        {
            return await _context.ProductCategoryTranslation.Include(x => x.ProductCategory).Include(x => x.Language).Where(x => x.ProductCategoryID == CatID).ToListAsync();
        }
        public void addCategory(tblProductCategory req)
        {
            _context.ProductCategory.Add(req);
        }
        public async Task<tblProductCategory?> GetCategoryByID(int id)
        {
            return await _context.ProductCategory.Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public void updateCategory(tblProductCategory req)
        {
            _context.ProductCategory.Update(req);
        }
        public async Task<tblProductCategoryTranslation?> getProductCategoryTranslationByCatID(int catID, string languageCode)
        {
            return await _context.ProductCategoryTranslation.Where(x => x.ProductCategoryID == catID && x.LanguageCode == languageCode).FirstOrDefaultAsync();
        }
        public async Task<List<tblProductTranslation>> GetProductTranslationsByProductID(int productID)
        {
            return await _context.ProductTranslations
                .Include(x => x.Language)
                .Where(x=>x.ProductID == productID)
                .ToListAsync();
        }
        public void UpdateProductTranslation(tblProductTranslation translation)
        {
            _context.ProductTranslations.Update(translation);
        }
        public void AddProductTranslation(tblProductTranslation translation)
        {
            _context.ProductTranslations.Add(translation);
        }
        public async Task<tblProductTranslation?> GetProductTranslationById(int transID)
        {
            return await _context.ProductTranslations
                .Where(x => x.ID == transID)
                .FirstOrDefaultAsync();
        }
        public async Task<tblProductTranslation?> GetProductTranslationById(int productID, string languageCode)
        {
            return await _context.ProductTranslations
                .Where(x => x.ProductID == productID && x.LanguageCode == languageCode)
                .FirstOrDefaultAsync();
        }
        public async Task<TblProduct?> GetProductByID(int productID)
        {
            return await _context.Products.Where(x=>x.ID == productID).FirstOrDefaultAsync();
        }
        public async Task<TblProduct?> GetProductByCode(string productCode)
        {
            return await _context.Products.Where(x => x.ProductCode == productCode).FirstOrDefaultAsync();
        }
        public async Task<TblProduct?> GetProductByNameOrCode(string productName, string productCode)
        {
            return await _context.Products.Where(x => x.Name == productName || x.ProductCode == productCode).FirstOrDefaultAsync();
        }
        public void AddProduct(TblProduct product)
        {
            _context.Products.Add(product);
        }
        public void UpdateProduct(TblProduct product)
        {
            _context.Products.Update(product);
        }
        public void AddProductCategoryTranslation(tblProductCategoryTranslation translation)
        {
            _context.ProductCategoryTranslation.Add(translation);
        }
        public void UpdateProductCategoryTranslation(tblProductCategoryTranslation translation)
        {
            _context.ProductCategoryTranslation.Update(translation);
        }
        public IQueryable<tblProductConsumptionMetrics> GetProductConsumptionMetrics()
        {
            return _context.ProductConsumptionMetric
                .Include(x => x.Product)
                .Include(x => x.TerritoryID)
                .Include(x => x.Crop);
        }
        public async Task<tblProductConsumptionMetrics?> GetProductConsumptionMetrics(int product, int? territory, int crop)
        {
           return await _context.ProductConsumptionMetric.Where(pcm => 
                pcm.ProductID == product &&
                pcm.CropID == crop &&
                pcm.TerritoryID == territory
            ).FirstOrDefaultAsync();
        }
        public void AddProductConsumptionMetrics(tblProductConsumptionMetrics req)
        {
            _context.ProductConsumptionMetric.Add(req);
        }
        public void UpdateProductConsumptionMetrics(tblProductConsumptionMetrics req)
        {
            _context.ProductConsumptionMetric.Update(req);
        }
    }
}
