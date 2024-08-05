using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimulatePrice;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [EnableCors("corsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private WsdlConfig _wsdlConfig;
        public ProductController(IRepositoryWrapper repoWrapper, IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _wsdlConfig = wsdlConfig.Value;
        }

        [HttpGet("GetAllProductsListing")]
        public async Task<APIResponse> GetProductList()
        {
            APIResponse response = new APIResponse();

            response.message = "Records Fetched";
            response.isError = false;
            response.response = await _repoWrapper.ProductRepo.getAllProducts();

            return response;
        }
        [HttpPost("ReconcileOrders")]
        //[AllowAnonymous]
        public async Task<APIResponse> ReconcileOrdersUpdate(ReconcilteOrderRequest request)
        {
            APIResponse response = new APIResponse();

            List<TblOrderProducts> orderProducts = await _repoWrapper.ProductRepo.getAllOrdersForProducts(request.ProductIDs);

            foreach (TblOrderProducts orderProduct in orderProducts)
            {
                tblPlan plan = orderProduct.Order.Plan;
                TblOrders order = orderProduct.Order;
                string SAPFarmerCode = orderProduct.Order.User.FarmerProfile.FirstOrDefault().SAPFarmerCode;

                decimal newOrderPrice = await GetOrderPriceWSDL(order, plan, SAPFarmerCode);

                decimal previousUnitPrice;

                //Check if the order was already reconcilled or not
                if (order.ReconciliationAmount != 0)
                    previousUnitPrice = order.ReconciliationAmount / orderProduct.QTY;
                else
                    previousUnitPrice = order.OrderAmount / orderProduct.QTY;


                int remainingQuantity = orderProduct.QTY - orderProduct.ClosingQTY;
                decimal newOrderUnitPrice = newOrderPrice / orderProduct.QTY;
                if (previousUnitPrice < newOrderUnitPrice)
                {
                    order.ReconciliationAmount = orderProduct.QTY * newOrderUnitPrice;
                    decimal reconciliationTotalAmount = remainingQuantity * newOrderUnitPrice;
                    TblOrders previousReconciliationOrder = order.ChildOrders.FirstOrDefault();
                    if (previousReconciliationOrder == null)
                    {
                        await CreateANewOrderReconciliation(order, reconciliationTotalAmount);
                    }
                    else
                    {
                        previousReconciliationOrder.OrderAmount += reconciliationTotalAmount;
                        await _repoWrapper.OrderRepo.UpdateOrder(previousReconciliationOrder);
                    }
                }

            }
            await _repoWrapper.SaveAsync();
            return response;
        }

        [HttpPost("addProductCategory")]
        public async Task<JSONResponse> AddProductCategory(AddProductCategoryByAdminRequest req)
        {
            JSONResponse response = new JSONResponse();
            tblProductCategory prodCat = new tblProductCategory() { Name = req.name, Status = req.status ? EActivityStatus.Active : EActivityStatus.DeActive };
            _repoWrapper.ProductRepo.addCategory(prodCat);
            await _repoWrapper.SaveAsync();
            response.message = "Product category added";
            return response;

        }
        [HttpPost("getProductCategories")]
        public async Task<APIResponse> GetProductCategories(pagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblProductCategory> lst = _repoWrapper.ProductRepo.getCategories();
            if (!string.IsNullOrEmpty(req.search))
            {
                lst = lst.Where(x =>
                    x.Name.ToLower().Contains(req.search)
                );
            }
            InResp.totalRecord = lst.Count();
            lst = lst.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = lst.Count();
            InResp.list = await lst.Select(x => new GetProductCategoryByAdminResponse
            {
                categoryID = x.ID,
                name = x.Name,
                status = (int)x.Status,
                translations = x.ProductCategoryTranslation.Select(t => new ProductCategoryTranslationDTO
                {
                    translationID = t.ID,
                    categoryID = t.ProductCategoryID,
                    languageCode = t.LanguageCode,
                    text = t.Text
                }).ToList()
            }).ToListAsync();
            response.response = InResp;
            return response;
        }


        [HttpPut("updateProductCategory")]
        public async Task<JSONResponse> UpdateProductCategory(UpdateProductCategoryByAdminRequest req)
        {
            JSONResponse response = new JSONResponse();
            tblProductCategory prodCat = await _repoWrapper.ProductRepo.GetCategoryByID(req.categoryID);
            if (prodCat == null)
            {
                throw new AmazonFarmerException(_exceptions.productCategoryNotFound);
            }
            prodCat.Name = req.name;
            prodCat.Status = req.status ? EActivityStatus.Active : EActivityStatus.DeActive;
            _repoWrapper.ProductRepo.updateCategory(prodCat);
            await _repoWrapper.SaveAsync();
            response.message = "Product category added";
            return response;
        }
        [HttpPatch("syncProductCategoryTranslation")]
        public async Task<JSONResponse> AddProductCategoryTranslation(SyncProductCategoryTranslationDTO req)
        {
            JSONResponse response = new JSONResponse();
            tblProductCategoryTranslation productCategoryTranslation = await _repoWrapper.ProductRepo.getProductCategoryTranslationByCatID(req.categoryID, req.languageCode);

            return response;
        }
        [HttpPost("getUnitOfMeasures")]
        public async Task<APIResponse> GeUnitOfMeasures()
        {
            APIResponse response = new APIResponse();

            List<tblUnitOfMeasure> lst = await _repoWrapper.ProductRepo.getUOMs();
            response.response = lst.Select(x => new GetProductUnitOfMeasuresByAdminResponse
            {
                uomID = x.ID,
                name = x.UOM,
                status = (int)x.Status,
                translations = x.UnitOfMeasureTranslation.Select(t => new UnitOfMeasureTranslationDTO
                {
                    translationID = t.ID,
                    uomID = t.UOMID,
                    languageCode = t.LanguageCode,
                    text = t.Text
                }).ToList()
            }).ToList();

            return response;
        }

        [HttpPost("getProducts")]
        public async Task<APIResponse> GetProducts(pagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            IQueryable<TblProduct> lst = _repoWrapper.ProductRepo.getProducts();

            if (!string.IsNullOrEmpty(req.search))
            {
                lst = lst.Where(x =>
                    (x.Name != null && x.Name.ToLower().Contains(req.search.ToLower())) ||
                    (x.ProductCode != null && x.ProductCode.ToLower().Contains(req.search.ToLower())) ||
                    (x.SalesOrg != null && x.SalesOrg.ToLower().Contains(req.search.ToLower())) ||
                    (x.Category != null && x.Category.Name.ToLower().Contains(req.search.ToLower())) ||
                    (x.UOM != null && x.UOM.UOM.ToLower().Contains(req.search.ToLower())) ||
                    (x.Division != null && x.Division.ToLower().Contains(req.search.ToLower()))
                );
            }
            InResp.totalRecord = lst.Count();
            lst = lst.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = lst.Count();
            InResp.list = await lst.Select(x => new GetProductsByAdminResponse
            {
                productID = x.ID,
                name = x.Name ?? string.Empty,
                code = x.ProductCode ?? string.Empty,
                category = x.Category != null ? x.Category.Name : string.Empty,
                salesOrg = x.SalesOrg ?? string.Empty,
                division = x.Division ?? string.Empty,
                uom = x.UOM != null ? x.UOM.UOM : string.Empty,
                status = (int)x.Active,
                translations = x.ProductTranslations.Select(t => new ProductTranslationDTO
                {
                    translationID = t.ID,
                    productID = t.ProductID,
                    languageCode = t.LanguageCode,
                    text = t.Text,
                    filePath = t.Image
                }).ToList()
            }).ToListAsync();
            response.response = InResp;
            return response;
        }

        private async Task<decimal> GetOrderPriceWSDL(TblOrders planOrder, tblPlan plan, string SAPFarmerCode)
        {
            decimal oldProductPrice = 0;
            foreach (TblOrderProducts orderProduct in planOrder.Products)
            {
                RequestType request = new()
                {
                    condGp1 = plan.OrderServices.Count > 0 ? plan.OrderServices[0].Service.Code : "",
                    condGp2 = plan.OrderServices.Count > 1 ? plan.OrderServices[1].Service.Code : "",
                    condGp3 = plan.OrderServices.Count > 2 ? plan.OrderServices[2].Service.Code : "",
                    condGp4 = plan.OrderServices.Count > 3 ? plan.OrderServices[3].Service.Code : "",
                    custNum = SAPFarmerCode,
                    custRef = "Created Plan for Farm",
                    division = orderProduct.Product.Division,
                    matNum = orderProduct.Product.ProductCode,
                    saleDistict = planOrder.Warehouse.SalePoint,
                    salesOrg = orderProduct.Product.SalesOrg,
                    saleUnit = orderProduct.Product.UOM.UOM
                };
                WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

                var wsdlResponse = await wSDLFunctions.PriceSimluate(request);
                if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0
                   && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S"
                   && !string.IsNullOrEmpty(wsdlResponse.itemNum.TrimStart('0')))
                {
                    oldProductPrice = (Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal)) * orderProduct.QTY;
                }

                if (oldProductPrice == 0)
                    throw new AmazonFarmerException(_exceptions.pricingNotMaintained);

            }
            return oldProductPrice;

        }

        private async Task<TblOrders> CreateANewOrderReconciliation(TblOrders order, decimal orderAmount)
        {
            DateTime DuePaymentDate = DateTime.UtcNow;
            DateTime deliveryDate = DateTime.UtcNow;
            List<DateTime> deliveryDates = new List<DateTime>();



            if (order.ExpectedDeliveryDate.Value.Date <= DateTime.UtcNow.AddHours(5).Date)
            {
                DuePaymentDate = DateTime.UtcNow.AddDays(+1);
            }
            else
            {
                DuePaymentDate = order.ExpectedDeliveryDate.Value;
            }

            TblOrders newOrder = new TblOrders()
            {
                AdvancePercent = order.AdvancePercent,
                ApprovalDate = order.ApprovalDate,
                ApprovalDatePrice = orderAmount,
                CreatedByID = order.CreatedByID,
                CreatedOn = DateTime.UtcNow,
                DuePaymentDate = DuePaymentDate,
                InvoicedDate = null,
                InvoicedDatePrice = null,
                OrderAmount = orderAmount,
                OrderName = "Order Reconcile for Order ID: " + order.OrderID.ToString(),
                OrderStatus = EOrderStatus.Active,
                OrderType = EOrderType.OrderReconcile,
                ParentOrderID = order.OrderID,
                PaymentDate = null,
                PaymentDatePrice = orderAmount,
                SAPTransactionID = null,
                OneLinkTransactionID = null,
                PaymentStatus = EOrderPaymentStatus.NonPaid,
                CropID = order.CropID,
                PlanID = order.PlanID,
                SAPOrderID = null,
                WarehouseID = order.WarehouseID,
                OrderRandomTransactionID = GetRandomNumber(),
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                Products = null,
                ReconciliationAmount = 0,
                DeliveryStatus = EDeliveryStatus.None
            };
            newOrder = _repoWrapper.OrderRepo.AddOrder(newOrder);

            //);
            return newOrder;
        }


        private int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 10000);
        }
    }
}
