using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
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

        public ProductRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<categoryDTO_Resp>> getProductsByLangugageID(LanguageReq req, int postDeliveryIn)
        {
            return await _context.ProductCategoryTranslation
                .Include(x => x.ProductCategory)
                .ThenInclude(x => x.Products)
                .ThenInclude(x => x.ProductTranslations)
                .Where(x => x.LanguageCode == req.languageCode)
                .Select(x => new categoryDTO_Resp
                {
                    categoryID = x.ProductCategoryID,
                    categoryName = x.Text,
                    filePath = x.Image,
                    products = x.ProductCategory.Products.Select(x => new ProductDTO_Resp
                    {
                        postDeliveryIn = postDeliveryIn,
                        productID = x.ID,
                        filePath = x.ProductTranslations.FirstOrDefault(pt => pt.LanguageCode == req.languageCode).Image, //Get productImageIcon from ProductTranslations
                        productName = x.ProductTranslations.FirstOrDefault(pt => pt.LanguageCode == req.languageCode).Text, // Get productName from ProductTranslations
                    }).ToList()

                }).ToListAsync();
        }


        public async Task<ProductPrices_Resp> getProductByProductID(int productID, string languageCode)
        {
            return await _context.ProductTranslations
                .Include(x => x.Product)
                .Where(x=>x.ProductID == productID && x.LanguageCode == languageCode)
                .Select(x => new ProductPrices_Resp
                {
                    productID= x.ProductID,
                    productCode = x.Product.ProductCode,
                    filePath = x.Image,
                    productName = x.Text,
                    productPrice = Convert.ToDecimal(10)
                }).FirstOrDefaultAsync();
        }
    }
}
