using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimulatePrice;
using System; // Added to use Exception class
using System.Collections.Generic; // Added to use List<>
using System.IdentityModel.Claims;
using System.Linq; // Added to use Sum()
using System.Threading.Tasks; // Added to use Task

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly NotificationService _notificationService;

        private WsdlConfig _wsdlConfig;
        // Constructor to inject IRepositoryWrapper dependency
        public ProductController(IRepositoryWrapper repoWrapper, NotificationService notificationService, IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
        }

        // Action method to get products
        [HttpGet("getProducts")]
        public async Task<APIResponse> getProducts()
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Get the language code from the user claims
                GetProductDTO_Internal_req req = new GetProductDTO_Internal_req()
                {
                    languageCode = User.FindFirst("languageCode")?.Value,
                    basePath = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")
                };

                // Call repository method to get products by language ID
                resp.response = await _repoWrapper.ProductRepo.getProductsByLangugageID(req, Convert.ToInt32(ConfigExntension.GetConfigurationValue("productSettings:PostDeliveryIn")));
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        // Action method to get product prices
        [HttpPost("getProductPrices")]
        public async Task<APIResponse> getProductPrices(ProductPrices_Request req)
        {
            APIResponse resp = new APIResponse();
            // Initialize MathExtensions instance
            MathExtensions mathExtensionsInstance = new MathExtensions();

            // Initialize response object
            GetProductPricings_Resp APIresp = new GetProductPricings_Resp();

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                                                                           // Get the language code from the user claims
            string languageCode = User.FindFirst("languageCode")?.Value;
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            if (User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);

            // Initialize list to store product prices
            List<ProductPrices_Resp> productsResp = new List<ProductPrices_Resp>();


            #region filterring products from request
            List<int> productIDs = new();
            productIDs.AddRange(req.products.Select(x => x.productID).ToList());
            List<TblProduct> products = await _repoWrapper.ProductRepo.getProductsByProductIDs(productIDs, languageCode);
            #endregion
            #region filterring services from request
            List<int> serviceIDs = req.serviceIDs;
            //foreach (int crop in req.serviceIDs)
            //{
            //    serviceIDs.AddRange(crop.serviceIDs);
            //}
            List<tblService> services = await _repoWrapper.ServiceRepo.getServicesByIDs(serviceIDs, languageCode);
            #endregion
            tblwarehouse warehouse = await _repoWrapper.WarehouseRepo.getWarehouseByID(req.warehouseID);



            //foreach (var crop in req.crops)
            //{
            // Loop through each product in the request
            foreach (var item in req.products)
            {
                List<PlanCropProductPrice_ServicesInt> newServices = req.serviceIDs
                .Select(ps => new PlanCropProductPrice_ServicesInt
                {
                    ServiceID = ps
                }).ToList();

                ProductPrices_Resp? planProductPrice = productsResp
                        .Where(pp => pp.productID == item.productID
                    && pp.serviceIDs.Count == newServices.Count &&
                    pp.serviceIDs.TrueForAll(s => newServices.Exists(ns => ns.ServiceID == s)))
                    .FirstOrDefault();

                if (planProductPrice == null)
                {
                    TblProduct product = products.Where(x => x.ID == item.productID).First();

                    PrivateFunc_GetProductPrice sapReq = new PrivateFunc_GetProductPrice()
                    {
                        serviceCode = services.Where(x => req.serviceIDs.Contains(x.ID)).Select(x => x.Code).ToList(),
                        sapFarmerCode = loggedInUser.FarmerProfile.FirstOrDefault().SAPFarmerCode,
                        productDivision = product.Division,
                        productCode = product.ProductCode,
                        warehouseSalePoint = warehouse.SalePoint,
                        productSalesOrg = product.SalesOrg,
                        productUOM = product.UOM.UOM,
                        productQTY = item.qty
                    };
                    ProductPrice sapResp = await GetProductPrices(sapReq);
                    productsResp.Add(new ProductPrices_Resp
                    {
                        productCode = product.ProductCode,
                        productID = item.productID,
                        filePath = product.ProductTranslations.FirstOrDefault().Image,
                        productName = product.ProductTranslations.FirstOrDefault().Text,
                        productPrice = sapResp.TotalAmount,
                        productUnitPrice = sapResp.UnitTotalAmount,
                        serviceIDs = req.serviceIDs,
                        //productQTY = sapReq.productQTY
                    });
                }
                else
                {
                    TblProduct product = products.Where(x => x.ID == item.productID).First();

                    decimal newProductPrice = item.qty * planProductPrice.productUnitPrice;
                    productsResp.Add(new ProductPrices_Resp
                    {
                        productCode = planProductPrice.productCode,
                        productID = planProductPrice.productID,
                        filePath = product.ProductTranslations.FirstOrDefault().Image,
                        productName = product.ProductTranslations.FirstOrDefault().Text,
                        productPrice = planProductPrice.productUnitPrice * item.qty,
                        productUnitPrice = planProductPrice.productUnitPrice,
                        serviceIDs = req.serviceIDs,
                        //productQTY = planProductPrice.productQTY + item.qty
                    });
                }
            }
            //}
            string advancePercentValue = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentPercent);

            // Calculate total price of all products
            APIresp.totalPrice = productsResp.Sum(x => x.productPrice);
            APIresp.advance = Convert.ToInt32(advancePercentValue);
            APIresp.advancePrice = mathExtensionsInstance.AdvanceValue(APIresp.advance, APIresp.totalPrice);

            //Merge Same Products
            var mergedProducts = productsResp
            .GroupBy(p => p.productID)
            .Select(g => new ProductPrices_Resp
            {
                productID = g.Key,
                productCode = g.First().productCode,
                filePath = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL") + g.First().filePath.Replace("/", "%2F").Replace(" ", "%20"),
                productName = g.First().productName,
                productPrice = g.Sum(x => x.productPrice),
                productUnitPrice = g.First().productUnitPrice,
                serviceIDs = g.First().serviceIDs,
                //productQTY = g.Sum(x=>x.productQTY)
            }).ToList();

            APIresp.productPricings = mergedProducts;

            // Set response object
            resp.response = APIresp;
            return resp;
        }

        private async Task<ProductPrice> GetProductPrices(PrivateFunc_GetProductPrice req)
        {
            decimal newProductPrice = 0;

            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
            RequestType request = new()
            {
                condGp1 = req.serviceCode.Count > 0 ? req.serviceCode[0] : "",
                condGp2 = req.serviceCode.Count > 1 ? req.serviceCode[1] : "",
                condGp3 = req.serviceCode.Count > 2 ? req.serviceCode[2] : "",
                condGp4 = req.serviceCode.Count > 3 ? req.serviceCode[3] : "",
                custNum = req.sapFarmerCode,
                custRef = "Created Plan for Farm",
                division = req.productDivision,
                matNum = req.productCode,
                saleDistict = req.warehouseSalePoint,
                salesOrg = req.productSalesOrg,
                saleUnit = req.productUOM
            };
            ResponseType wsdlResponse = await wSDLFunctions.PriceSimluate(request);
            if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0
                        && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S" && !string.IsNullOrEmpty(wsdlResponse.itemNum.TrimStart('0')))
            {
                newProductPrice = (Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal)) * req.productQTY;
                ProductPrice ProductPrice = new()
                {
                    Quantity = req.productQTY,
                    UnitPrice = Convert.ToDecimal(wsdlResponse.netVal),
                    UnitTax = Convert.ToDecimal(wsdlResponse.taxVal),
                    UnitTotalAmount = Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal),
                    TotalAmount = newProductPrice
                };
                return ProductPrice;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.pricingNotMaintained);
            }
        }

        // Helper method to compare two lists of integers for equality
        private bool AreServiceIDsEqual(List<int> list1, List<int> list2)
        {
            if (list1 == null && list2 == null)
                return true;
            if (list1 == null || list2 == null)
                return false;
            return list1.Count == list2.Count && !list1.Except(list2).Any();
        }

        // Check if any element in a1 matches the given a2Item
        private bool ContainsMatchingProductService(List<usedProductServices> a1, usedProductServices a2Item)
        {
            return a1.Any(a1Item =>
                a1Item.productID == a2Item.productID &&
                AreServiceIDsEqual(a1Item.serviceIDs, a2Item.serviceIDs)
            );
        }




    }
}
