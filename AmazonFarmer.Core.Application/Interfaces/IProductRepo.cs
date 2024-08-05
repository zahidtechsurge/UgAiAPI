using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IProductRepo
    {
        Task<List<categoryDTO_Resp>> getProductsByLangugageID(LanguageReq req, int postDeliveryIn);
        Task<ProductPrices_Resp> getProductByProductID(int productID, string languageCode);
    }
}
