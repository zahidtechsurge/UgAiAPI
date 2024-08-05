using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmerAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public ProductController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        //[Authorize]
        [HttpGet("getProducts")]

        public async Task<APIResponse> getProducts()
        {
            APIResponse resp = new APIResponse();
            try
            {
                LanguageReq req = new LanguageReq() { languageCode= User.FindFirst("languageCode")?.Value };
                resp.response = await _repoWrapper.ProductRepo.getProductsByLangugageID(req, Convert.ToInt32(ConfigExntension.GetConfigurationValue("productSettings:PostDeliveryIn")));
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        //[Authorize]
        [HttpPost("getProductPrices")]
        public async Task<APIResponse> getProductPrices(List<ProductPrices_Req> req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                MathExtensions mathExtensionsInstance = new MathExtensions();
                GetProductPricings_Resp APIresp = new GetProductPricings_Resp();
                string languageCode = User.FindFirst("languageCode")?.Value;
                List<ProductPrices_Resp> products = new List<ProductPrices_Resp>();
                foreach (var item in req)
                {
                    ProductPrices_Resp product = await _repoWrapper.ProductRepo.getProductByProductID(item.productID, languageCode);
                    product.productPrice = (product.productPrice * item.qty);
                    products.Add(product);
                }

                APIresp.totalPrice = products.Sum(x => x.productPrice);
                APIresp.advance = 5;
                APIresp.advancePrice = mathExtensionsInstance.AdvanceValue(APIresp.advance, APIresp.totalPrice);
                APIresp.productPricings = products;

                resp.response = APIresp;
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

    }
}
