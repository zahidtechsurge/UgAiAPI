using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using DinkToPdf.Contracts;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Cms;
using System.IdentityModel.Claims;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using AmazonFarmerException = AmazonFarmer.Core.Application.Exceptions.AmazonFarmerException;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing authority letter operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class AuthorityLetterController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private IWebHostEnvironment _hostEnvironment;
        private readonly NotificationService _notificationService;
        private readonly IAzureFileShareService _azureFileShareService;
        private readonly IConverter _converter; // Converter wrapper to interact with data
        // Constructor injection of IRepositoryWrapper.
        public AuthorityLetterController(IRepositoryWrapper repoWrapper, NotificationService notificationService, IWebHostEnvironment hostEnvironment, IAzureFileShareService azureFileShareService, IConverter converter)
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _hostEnvironment = hostEnvironment;
            _azureFileShareService = azureFileShareService;
            _converter = converter;
        }

        [HttpPost("addAuthorityLetter")]
        public async Task<JSONResponse> addAuthorityLetter(add_AuthorityLetter_Res req)
        {
            JSONResponse resp = new JSONResponse();
            if (req.orderID == 0)
                throw new AmazonFarmerException(_exceptions.orderIDRequired);
            else if (req.productID == 0)
                throw new AmazonFarmerException(_exceptions.productIDRequired);
            else if (req.qty == 0)
                throw new AmazonFarmerException(_exceptions.qtyRequired);
            else if (string.IsNullOrEmpty(req.truckerNo))
                throw new AmazonFarmerException(_exceptions.truckerRequired);
            else if (string.IsNullOrEmpty(req.biltyNo))
                throw new AmazonFarmerException(_exceptions.biltyNumberRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                if (string.IsNullOrEmpty(userID))
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                else
                {
                    TblUser farmer = await _repoWrapper.UserRepo.getUserByUserID(userID);
                    TblOrders? order = await _repoWrapper.OrderRepo.getOrderByOrderID(req.orderID);
                    if (order == null)
                        throw new AmazonFarmerException(_exceptions.orderNotFound);
                    else if (order.CreatedByID != userID)
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    else if (order.Products.Where(x => x.ProductID == req.productID).Count() <= 0)
                        throw new Exception(_exceptions.invalidOrderProduct);
                    else if ((order.Products.FirstOrDefault().ClosingQTY + req.qty) > order.Products.FirstOrDefault().QTY)
                        throw new AmazonFarmerException(_exceptions.authorityLetterQtyReached);
                    if (farmer.FarmerProfile == null)
                        throw new AmazonFarmerException(_exceptions.userNotFound);
                    if (string.IsNullOrEmpty(farmer.FarmerProfile.FirstOrDefault().SAPFarmerCode))
                        throw new AmazonFarmerException(_exceptions.sapFarmerCodeNotFound);
                    if ((order.Products.FirstOrDefault().ClosingQTY + req.qty) > order.Products.FirstOrDefault().QTY)
                        throw new AmazonFarmerException(_exceptions.authorityLetterQtyReached);
                    else
                    {
                        TblAuthorityLetterDetails letterDetails = new TblAuthorityLetterDetails()
                        {
                            BagQuantity = req.qty,
                            TruckerNo = req.truckerNo,
                            BiltyNo = req.biltyNo,
                            ProductID = req.productID,
                            AuthorityLetters = new TblAuthorityLetters()
                            {
                                AuthorityLetterNo = await generateLetterNo(),
                                SAPFarmerCode = farmer.FarmerProfile.FirstOrDefault()?.SAPFarmerCode,
                                OrderID = order.OrderID,
                                BearerName = req.brearName,
                                BearerNIC = req.bearerNIC,
                                FieldWHIncharge = req.fieldWHIncharge,
                                CreatedByID = userID,
                                CreatedOn = DateTime.UtcNow,
                                Status = false,// true when WHI approves the authority Letter
                                Active = EAuthorityLetterStatus.Active,
                                Dated = order.ApprovalDate,
                                WareHouseID = order.Plan.WarehouseID,
                            }
                        };

                        PDFExtension pdfExtension = new PDFExtension(_converter);
                        AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                        #region Generte Authority Letter PDF
                        byte[] pdfBytes = pdfExtension.generateAuthorityLetterBytes(letterDetails, order);
                        _uploadAttachmentReq attachmentReq = new _uploadAttachmentReq()
                        {
                            name = "AuthorityLetter-" + order.OrderID + "-" + order.Products.FirstOrDefault()?.Product?.ProductCode + "-" + DateTime.UtcNow.ToString("ddMMyyyy_hhmmff") + ".pdf",
                            contentBytes = pdfBytes,
                            requestTypeID = (int)EAttachmentType.PDF_AuthorityLetter
                        };
                        uploadAttachmentResp attachment = await attachmentExt.uploadAttachment(attachmentReq);
                        letterDetails.AuthorityLetters.PDFGUID = attachment.guid;
                        #endregion

                        await _repoWrapper.AuthorityLetterRepo.addAuthorityLetterByDetail(letterDetails);

                        //order.Products.FirstOrDefault().ClosingQTY += req.qty;
                        //await _repoWrapper.OrderRepo.UpdateOrder(order);

                        await _repoWrapper.SaveAsync();
                        NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.AuthorityLetter_Warehouse, "EN");
                        if (notificationDTO != null)
                        {
                            TblUser warehouseIncharge = await _repoWrapper.WarehouseRepo.getWarehouseInchargeByWarehouseID(order.Plan.WarehouseID);
                            if (warehouseIncharge != null)
                            {
                                List<NotificationRequest> notifications = new List<NotificationRequest> {
                                    new NotificationRequest
                                    {
                                        Type= ENotificationType.Email,
                                        Recipients =new List<NotificationRequestRecipient> {
                                            new NotificationRequestRecipient(){
                                                Email = warehouseIncharge.Email,
                                                Name = warehouseIncharge.FirstName
                                            }
                                            },
                                        Subject =  notificationDTO.title,
                                        Message = notificationDTO.body
                                    },
                                    new NotificationRequest
                                    {
                                        Type= ENotificationType.SMS,
                                        Recipients =new List<NotificationRequestRecipient> {
                                            new NotificationRequestRecipient(){
                                                Email = warehouseIncharge.PhoneNumber,
                                                Name = warehouseIncharge.FirstName
                                            }
                                            },
                                        Subject =  notificationDTO.title,
                                        Message = notificationDTO.smsBody
                                    },
                                    new NotificationRequest
                                    {
                                        Type= ENotificationType.FCM,
                                        Recipients =new List<NotificationRequestRecipient> {
                                            new NotificationRequestRecipient(){
                                                Email = warehouseIncharge.DeviceToken,
                                                Name = warehouseIncharge.FirstName
                                            }
                                            },
                                        Subject =  notificationDTO.title,
                                        Message = notificationDTO.fcmBody
                                    },
                                    new NotificationRequest
                                    {
                                        Type= ENotificationType.Device,
                                        Recipients =new List<NotificationRequestRecipient> {
                                            new NotificationRequestRecipient(){
                                                Email = warehouseIncharge.Id,
                                                Name = warehouseIncharge.FirstName
                                            }
                                            },
                                        Subject =  notificationDTO.title,
                                        Message = notificationDTO.deviceBody
                                    }
                                };

                                NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                                replacementDTO.NotificationBodyTypeID = ENotificationBody.AuthorityLetter_Warehouse;
                                replacementDTO.OrderID = order.OrderID.ToString().PadLeft(10, '0');
                                replacementDTO.AuthorityLetterId = letterDetails.AuthorityLetters.AuthorityLetterID.ToString();
                                replacementDTO.AuthorityLetterNo = letterDetails.AuthorityLetters.AuthorityLetterNo;

                                await _notificationService.SendNotifications(notifications, replacementDTO);
                            }
                        }
                        resp.message = "Authority Letter created.";
                    }
                }

            }
            return resp;
        }

        [HttpPost("getAuthorityLetters")]
        public async Task<APIResponse> getAuthorityLetters(getAuthorityLetters_Req req)
        {
            APIResponse resp = new APIResponse();
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
            {
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
            }

            IQueryable<TblAuthorityLetters> letters = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetters(userID, languageCode);
            TblOrders order = await _repoWrapper.OrderRepo.getOrderByID(req.orderID, userID, languageCode);

            if (order == null)
                throw new AmazonFarmerException(_exceptions.orderNotFound);

            letters = letters.Where(x => x.OrderID == req.orderID).OrderByDescending(x => x.AuthorityLetterID);
            if (!string.IsNullOrEmpty(req.search))
                letters = letters.Where(x => x.AuthorityLetterNo.Contains(req.search) || x.BearerNIC.Contains(req.search));
            letters = letters.Skip(req.skip).Take(req.take);
            var letterList = letters.ToList();

            getAuthorityLetters_Resp inResp = new getAuthorityLetters_Resp
            {
                TotalQTY = order.Products.FirstOrDefault().QTY,
                PendingQTY = (order.Products.FirstOrDefault().QTY - order.AuthorityLetters.Where(x => x.Active != EAuthorityLetterStatus.DeActive).Sum(x => x.AuthorityLetterDetails.FirstOrDefault().BagQuantity)),
                pagination = new pagination_Resp()
                {
                    filteredRecord = 0,
                    totalRecord = 0,
                    list = new List<getAuthorityLetters_RespList>()
                }
            };

            if (letterList == null || letterList.Count() <= 0)
            {
                resp.message = _exceptions.authorityLetterNotFound;
                resp.response = new List<getAuthorityLetters_RespList>();
            }
            else
            {
                var lst = letterList.Select(x => new getAuthorityLetters_RespList
                {
                    letterID = x.AuthorityLetterID,
                    letterNo = x.AuthorityLetterNo,
                    bearerName = x.BearerName,
                    bearerNIC = x.BearerNIC,
                    warehouseInchage = x.Order.Warehouse.WarehouseIncharge.FirstName,
                    invoiceNumber = x.INVNumber ?? string.Empty,
                    farmerName = x.CreatedBy.FirstName,
                    product = x.AuthorityLetterDetails.First().Products.ProductTranslations.First().Text,
                    qty = x.AuthorityLetterDetails.Sum(x => x.BagQuantity).ToString(),
                    isEditable = x.Status,
                    status = x.Active.ToString(),
                    creationDate = x.Dated,
                    autoGenerated = x.IsOCRAutomated,
                    canDelete = x.Active == EAuthorityLetterStatus.Approved || x.Active == EAuthorityLetterStatus.DeActive ? false : true,
                    isDeleted = x.Active == EAuthorityLetterStatus.DeActive ? true : false
                }).ToList();


                var paginatedList = new pagination_Resp
                {
                    list = lst,
                    filteredRecord = lst.Count(),
                    totalRecord = lst.Count()
                };
                inResp.pagination = paginatedList;
                //inResp = new getAuthorityLetters_Resp()
                //{
                //    TotalQTY = (letterList.First().Order.Products.First().QTY),
                //    PendingQTY = (letterList.First().Order.Products.First().QTY - letterList.First().Order.Products.First().ClosingQTY),
                //    pagination = paginatedList
                //}; 

            }
            resp.response = inResp;

            return resp;
        }

        [HttpPost("getAuthorityLetterDetail")]
        public async Task<APIResponse> getAuthorityLetterDetail(authorityLetter_GetDetails req)
        {
            APIResponse resp = new APIResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (userID == null || string.IsNullOrEmpty(userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (req.letterID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            if (User.IsInRole("Employee"))
            {
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
            }
            TblAuthorityLetters authLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.letterID, userID, languageCode);
            if (authLetter == null)
                throw new AmazonFarmerException(_exceptions.authorityLetterNotFound);
            else
            {
                string warehouseName = authLetter.Warehouse.Name;
                authorityLetter_GetDetails_Resp inResp = new authorityLetter_GetDetails_Resp()
                {
                    letterID = authLetter.AuthorityLetterID,
                    letterNo = authLetter.AuthorityLetterNo,
                    letterCreationDate = authLetter.CreatedOn,
                    orderDate = authLetter.Dated,
                    bearerName = authLetter.BearerName,
                    bearerNIC = authLetter.BearerNIC,
                    orderNo = authLetter.Order.OrderID.ToString().PadLeft(10, '0'),
                    sapOrderID = authLetter.Order.SAPOrderID,
                    invoiceNo = authLetter.INVNumber ?? string.Empty,
                    godownIncharge = authLetter.FieldWHIncharge,
                    pdfGUID = authLetter.PDFGUID ?? string.Empty,
                    attachment = authLetter.Attachment != null ? new uploadAttachmentResp()
                    {
                        id = authLetter.Attachment.ID,
                        guid = authLetter.Attachment.Guid.ToString(),
                        name = authLetter.Attachment.Name,
                        type = authLetter.Attachment.FileType
                    } : new uploadAttachmentResp(),
                    products = authLetter.AuthorityLetterDetails.Select(x => new authorityLetter_Product_Resp
                    {
                        productID = x.ProductID,
                        productCode = x.Products.ProductCode,
                        productName = x.Products.ProductTranslations.FirstOrDefault().Text,
                        productQTY = x.BagQuantity,
                        warehouseName = warehouseName,
                        biltyNo = x.BiltyNo,
                        truckerNo = x.TruckerNo
                    }).ToList()
                };
                resp.response = inResp;
            }



            return resp;
        }
        [HttpGet("getOrderDetail/{orderID}")]
        public async Task<APIResponse> getOrderDetail(Int64 orderID)
        {
            APIResponse resp = new();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            TblOrders order = await _repoWrapper.OrderRepo.getOrderByID(orderID, userID, languageCode);

            if (order == null)
                throw new AmazonFarmerException(_exceptions.orderNotFound);
            else if (order.PaymentStatus == EOrderPaymentStatus.NonPaid)
                throw new AmazonFarmerException(_exceptions.orderNotPaid);
            else if (!order.ExpectedDeliveryDate.HasValue)
                throw new AmazonFarmerException(_exceptions.orderExpectedDeliveryDateNotFound);

            resp.response = new authorityLetter_GetOrderDetail_Resp()
            {
                orderID = order.OrderID,
                orderDate = order.ExpectedDeliveryDate.Value,//.ToString("yyyy-MM-dd"),
                products = order.Products.Select(x => new authorityLetter_GetOrderDetail_Product
                {
                    productID = x.ProductID,
                    productImage = ConfigExntension.GetConfigurationValue("Locations:AdminBaseURL") + x.Product.ProductTranslations.First().Image,
                    productCode = x.Product.ProductCode,
                    productName = x.Product.ProductTranslations.First().Text,
                    qty = x.QTY
                }).ToList()
            };

            return resp;
        }
        [HttpDelete("removeAuthorityLetter")]
        public async Task<JSONResponse> removeAuthorityLetter(removeAuthorityLetterReq req)
        {
            JSONResponse resp = new JSONResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
            TblAuthorityLetters authorityLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.authorityLetterID, userID, languageCode);

            if (authorityLetter == null)
                throw new AmazonFarmerException(_exceptions.authorityLetterNotFound);
            else if (authorityLetter.Active != EAuthorityLetterStatus.Active)
                throw new AmazonFarmerException(_exceptions.authorityLetterNotInValidState);


            //authorityLetter.Order.Products.FirstOrDefault().ClosingQTY = (authorityLetter.Order.Products.FirstOrDefault().ClosingQTY - authorityLetter.AuthorityLetterDetails.FirstOrDefault().BagQuantity);
            authorityLetter.Active = EAuthorityLetterStatus.DeActive;
            await _repoWrapper.AuthorityLetterRepo.updateAuthorityLetter(authorityLetter);
            await _repoWrapper.SaveAsync();


            return resp;
        }
        private async Task<string> generateLetterNo()
        {
            string resp = string.Empty;
            // Array of alphabets
            char[] aplha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int hex = 1000;
            int hexInt = Convert.ToInt32(hex.ToString(), 16);
            hexInt = (hexInt + await _repoWrapper.AuthorityLetterRepo.GetAuthorityLetterHexCount());
            resp = hexInt.ToString("X");
            // Random number generator
            Random random = new Random();
            // Get a random alphabet from the array
            char randomAlphabet = aplha[random.Next(0, aplha.Length)];
            // Add the random alphabet at the beginning of the input string
            resp = randomAlphabet + resp;

            //saving in database
            tblAuthorityLetter_Hexs addHex = new tblAuthorityLetter_Hexs()
            {
                HexaNo = resp,
                Number = hexInt
            };
            await _repoWrapper.AuthorityLetterRepo.addAuthorityLetterHex(addHex);
            await _repoWrapper.SaveAsync();

            return resp;
        }

    }
}
