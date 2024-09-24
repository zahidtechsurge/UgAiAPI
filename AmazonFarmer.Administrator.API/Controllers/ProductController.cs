using AmazonFarmer.Administrator.API.Extensions;
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
using Org.BouncyCastle.Ocsp;
using SimulatePrice;
using System.IdentityModel.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [EnableCors("corsPolicy")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private WsdlConfig _wsdlConfig;
        private readonly IAzureFileShareService _azureFileShareService;

        public ProductController(IRepositoryWrapper repoWrapper, IOptions<WsdlConfig> wsdlConfig, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _wsdlConfig = wsdlConfig.Value;
            _azureFileShareService = azureFileShareService;
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

        #region Product Category
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

        [HttpGet("getProductCategories")]
        public async Task<APIResponse> GetProductCategories()
        {
            APIResponse resp = new APIResponse();
            IQueryable<tblProductCategory> lst = _repoWrapper.ProductRepo.getCategories();
            lst = lst.Where(x => x.Status == EActivityStatus.Active);
            resp.response = await lst.Select(x => new GetProductCategoryByAdminResponse
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

            return resp;
        }
        [HttpPost("getProductCategories")]
        public async Task<APIResponse> GetProductCategories(ReportPagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblProductCategory> lst = _repoWrapper.ProductRepo.getCategories();
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("categoryID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.ID);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("name"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Name);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Name);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Status);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Status);
                    }
                }

            }
            else
            {
                lst = lst.OrderByDescending(x => x.ID);
            }
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
            tblProductCategory? prodCat = await _repoWrapper.ProductRepo.GetCategoryByID(req.categoryID);
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
        [HttpGet("getProductCategoryTranslations/{categoryID}")]
        public async Task<APIResponse> GetProductCategoryTranslations(int categoryID)
        {
            APIResponse response = new APIResponse();
            List<tblProductCategoryTranslation> lang = await _repoWrapper.ProductRepo.GetCategoryTranslationsByCatID(categoryID);
            response.response = lang.Select(x => new ProductCategoryTranslationResponse
            {
                categoryID = x.ProductCategoryID,
                translationID = x.ID,
                language = x.Language.LanguageName,
                languageCode = x.LanguageCode,
                text = x.Text
            }).ToList();
            return response;
        }

        [AllowAnonymous]
        [HttpPatch("syncProductCategoryTranslation")]
        public async Task<JSONResponse> AddProductCategoryTranslation(SyncProductCategoryTranslationDTO req)
        {
            JSONResponse response = new JSONResponse();
            tblProductCategoryTranslation? productCategoryTranslation = await _repoWrapper.ProductRepo.getProductCategoryTranslationByCatID(req.categoryID, req.languageCode);
            if (productCategoryTranslation == null)
            {
                productCategoryTranslation = new tblProductCategoryTranslation()
                {
                    ProductCategoryID = req.categoryID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = string.Empty
                };
                _repoWrapper.ProductRepo.AddProductCategoryTranslation(productCategoryTranslation);
                response.message = "product category added";
            }
            else
            {
                productCategoryTranslation.ProductCategoryID = req.categoryID;
                productCategoryTranslation.LanguageCode = req.languageCode;
                productCategoryTranslation.Text = req.text;
                productCategoryTranslation.Image = string.Empty;
                _repoWrapper.ProductRepo.UpdateProductCategoryTranslation(productCategoryTranslation);
                response.message = "product category updated";
            }
            await _repoWrapper.SaveAsync();
            return response;
        }
        #endregion

        [HttpGet("getUnitOfMeasures")]
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

        #region Product Translation
        [HttpGet("getTranslation/{productID}")]
        public async Task<APIResponse> GetTranslation(int productID)
        {
            APIResponse response = new APIResponse();
            List<tblProductTranslation> productTranslations = await _repoWrapper.ProductRepo.GetProductTranslationsByProductID(productID);
            response.response = productTranslations
                .Select(pt => new GetProductTranslationResponse
                {
                    translationID = pt.ID,
                    productID = pt.ProductID,
                    languageCode = pt.LanguageCode,
                    text = pt.Text,
                    filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), "%2F", pt.Image.TrimStart('/').Replace("\\", "%2F").Replace("/", "%2F").Replace(" ", "%20")),
                    language = pt.Language.LanguageName
                })
                .ToList();
            return response;
        }

        [HttpPost("addProductTranslation")]
        public async Task<JSONResponse> AddProductTranslation(AddProductTranslationRequest req)
        {
            tblProductTranslation? productTranslation = await _repoWrapper.ProductRepo.GetProductTranslationById(req.productID, req.languageCode);
            if (productTranslation == null)
            {
                JSONResponse resp = new JSONResponse();
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "untitledProduct.svg"), content: (req.content ?? string.Empty), requestTypeID: EAttachmentType.Product);
                productTranslation = new tblProductTranslation()
                {
                    ProductID = req.productID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = req.filePath ?? attachment.filePath
                };
                _repoWrapper.ProductRepo.AddProductTranslation(productTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Product translation added";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.productAlreadyExist);
        }
        [AllowAnonymous]
        [HttpPut("updateProductTranslation")]
        public async Task<JSONResponse> UpdateProductTranslation(UpdateProductTranslationRequest req)
        {
            tblProductTranslation? productTranslation = await _repoWrapper.ProductRepo.GetProductTranslationById(req.translationID);
            if (productTranslation != null)
            {
                JSONResponse resp = new JSONResponse();
                if (string.IsNullOrEmpty(req.filePath))
                {
                    AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                    AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "untitledProduct.svg"), content: (req.content ?? string.Empty), requestTypeID: EAttachmentType.Product);
                    productTranslation.Image = attachment.filePath;
                }
                else
                    productTranslation.Image = req.filePath;

                productTranslation.ProductID = req.productID;
                productTranslation.LanguageCode = req.languageCode;
                productTranslation.Text = req.text;
                _repoWrapper.ProductRepo.UpdateProductTranslation(productTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Product translation updated";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.productNotFound);
        }
        [AllowAnonymous]
        [HttpPatch("syncProductTranslation")]
        public async Task<JSONResponse> SyncProductTranslation(UpdateProductTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblProductTranslation? productTranslation = await _repoWrapper.ProductRepo.GetProductTranslationById(req.productID, req.languageCode);
            if (productTranslation != null)
            {
                if (string.IsNullOrEmpty(req.filePath))
                {
                    AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                    req.content = req.content.Replace("data:image/png;base64,", "");
                    req.content = req.content.Replace("data:image/svg+xml;base64,", "");
                    AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "untitledProduct.svg"), content: (req.content ?? string.Empty), requestTypeID: EAttachmentType.Product);
                    productTranslation.Image = string.Concat("/", attachment.filePath.Replace("\\", "/"));
                }
                else
                    productTranslation.Image = req.filePath.Replace(string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")), "").Replace("%20", " ").Replace("%2F", "/");

                productTranslation.ProductID = req.productID;
                productTranslation.LanguageCode = req.languageCode;
                productTranslation.Text = req.text;
                _repoWrapper.ProductRepo.UpdateProductTranslation(productTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Product translation updated";
            }
            else
            {
                AttachmentsDTO attachment = new AttachmentsDTO();
                if (string.IsNullOrEmpty(req.filePath))
                {
                    AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                    req.content = req.content.Replace("data:image/png;base64,", "");
                    req.content = req.content.Replace("data:image/svg+xml;base64,", "");
                    attachment = await attachmentExt.UploadAttachment(name: (req.fileName ?? "untitledProduct.svg"), content: (req.content ?? string.Empty), requestTypeID: EAttachmentType.Product);
                }
                productTranslation = new tblProductTranslation()
                {
                    ProductID = req.productID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = string.IsNullOrEmpty(req.filePath) ? string.Concat("/", attachment.filePath.TrimStart('/').Replace("\\", "/")) : req.filePath.Replace(string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")), "").Replace("%20", " ").Replace("%2F", "/")
                };
                _repoWrapper.ProductRepo.AddProductTranslation(productTranslation);
                await _repoWrapper.SaveAsync();
                resp.message = "Product translation added";
            }
            return resp;

        }
        #endregion

        #region Product Module

        [HttpPost("getProducts")]
        public async Task<APIResponse> GetProducts(ReportPagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            IQueryable<TblProduct> lst = _repoWrapper.ProductRepo.getProducts();
            //lst = lst.Where(x => x.CategoryID == req.rootID);
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("productID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.ID);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("productName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Name);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Name);
                    }
                }
                else if (req.sortColumn.Contains("productCode"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.ProductCode);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ProductCode);
                    }
                }
                else if (req.sortColumn.Contains("category"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Category.Name);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Category.Name);
                    }
                }
                else if (req.sortColumn.Contains("saleOrg"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.SalesOrg);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.SalesOrg);
                    }
                }
                else if (req.sortColumn.Contains("division"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Division);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Division);
                    }
                }
                else if (req.sortColumn.Contains("uom"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.UOM.UOM);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.UOM.UOM);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        lst = lst.OrderBy(x => x.Active);
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Active);
                    }
                }
            }
            else
            {
                lst = lst.OrderByDescending(x => x.ID);
            }

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
            InResp.list = await lst.Select(x => new GetProductRequest
            {
                productID = x.ID,
                productName = x.Name ?? string.Empty,
                productCode = x.ProductCode ?? string.Empty,
                categoryID = x.CategoryID,
                category = x.Category != null ? x.Category.Name : string.Empty,
                saleOrg = x.SalesOrg ?? string.Empty,
                division = x.Division ?? string.Empty,
                uomID = x.UOMID,
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
        [HttpPost("addProduct")]
        public async Task<JSONResponse> AddProduct(AddProductRequest req)
        {
            TblProduct? product = await _repoWrapper.ProductRepo.GetProductByNameOrCode(req.productName, req.productCode);
            if (product == null)
            {
                // Get the user ID from the token
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                JSONResponse resp = new JSONResponse();
                product = new TblProduct()
                {
                    CategoryID = req.categoryID,
                    Name = req.productName,
                    ProductCode = req.productCode,
                    UOMID = req.uomID,
                    SalesOrg = req.saleOrg,
                    CreatedByID = userID,
                    CreatedDate = DateTime.UtcNow,
                    Active = EActivityStatus.Active,
                    Division = req.division
                };
                _repoWrapper.ProductRepo.AddProduct(product);
                await _repoWrapper.SaveAsync();
                resp.message = "Product Added";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.productAlreadyExist);
        }
        [HttpPut("updateProduct")]
        public async Task<JSONResponse> UpdateProduct(UpdateProductRequest req)
        {
            TblProduct? product = await _repoWrapper.ProductRepo.GetProductByID(req.productID);
            if (product != null)
            {
                JSONResponse resp = new JSONResponse();
                product.CategoryID = req.categoryID;
                product.Name = req.productName;
                product.ProductCode = req.productCode;
                product.SalesOrg = req.saleOrg;
                product.UOMID = req.uomID;
                product.Active = (EActivityStatus)req.status;
                product.Division = req.division;
                _repoWrapper.ProductRepo.UpdateProduct(product);
                await _repoWrapper.SaveAsync();
                resp.message = "Product Updated";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.productNotFound);
        }
        #endregion

        #region Product Consumption Matrix
        [HttpPost("getConsumptionMatrix")]
        public async Task<APIResponse> GetConsumptionMatrix(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblProductConsumptionMetrics> pcm = _repoWrapper.ProductRepo.GetProductConsumptionMetrics();
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("pcmID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.ID);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("product"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.Product.Name);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.Product.Name);
                    }
                }
                else if (req.sortColumn.Contains("crop"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.Crop.Name);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.Crop.Name);
                    }
                }
                else if (req.sortColumn.Contains("territoryID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.TerritoryID);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.TerritoryID);
                    }
                }
                else if (req.sortColumn.Contains("qty"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.Usage);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.Usage);
                    }
                }
                else if (req.sortColumn.Contains("uom"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        pcm = pcm.OrderBy(x => x.UOM);
                    }
                    else
                    {
                        pcm = pcm.OrderByDescending(x => x.UOM);
                    }
                }

            }
            else
            {
                pcm = pcm.OrderByDescending(x => x.ID);
            }
            if (!string.IsNullOrEmpty(req.search))
            {
                pcm = pcm.Where(x =>
                    x.Product.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.Crop.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.ID.ToString().ToLower().Contains(req.search.ToLower()) ||
                    x.TerritoryID.ToString().ToLower().Contains(req.search.ToLower())
                );
            }
            InResp.totalRecord = pcm.Count();
            pcm = pcm.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = pcm.Count();
            InResp.list = await pcm
                .Select(x => new GetConsumptionMatrixResponse
                {
                    pcmID = x.ID,
                    productID = x.ProductID,
                    product = x.Product.Name,
                    cropID = x.CropID,
                    crop = x.Crop.Name,
                    territoryID = x.TerritoryID,
                    qty = x.Usage,
                    uom = x.UOM
                })
                .ToListAsync();

            resp.response = InResp;
            return resp;
        }
        [HttpPatch("syncConsumptionMatrix")]
        public async Task<JSONResponse> AddConsumptionMatrix(AddConsumptionMatrix req)
        {
            JSONResponse resp = new JSONResponse();
            tblProductConsumptionMetrics? pcm = await _repoWrapper.ProductRepo.GetProductConsumptionMetrics(req.productID, req.territoryID, req.cropID);
            if (pcm != null)
            {
                pcm.Usage = req.qty;
                pcm.UOM = req.uom;
                _repoWrapper.ProductRepo.UpdateProductConsumptionMetrics(pcm);
                resp.message = "Product Consumption Metrics Updated";
            }
            else
            {
                pcm = new tblProductConsumptionMetrics()
                {
                    ProductID = req.productID,
                    TerritoryID = req.territoryID,
                    CropID = req.cropID,
                    Usage = req.qty,
                    UOM = req.uom
                };
                _repoWrapper.ProductRepo.AddProductConsumptionMetrics(pcm);
                resp.message = "Product Consumption Metrics Added";
            }
            await _repoWrapper.SaveAsync();
            return resp;
        }
        #endregion

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
