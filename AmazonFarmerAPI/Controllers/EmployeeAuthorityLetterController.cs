﻿using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using ChangeCustomerPayment;
using DetailsInvoice;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Claims;
using System.Text.RegularExpressions;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing authority letter operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Employee/AuthorityLetter")]
    public class EmployeeAuthorityLetterController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private IWebHostEnvironment _hostEnvironment;
        private readonly NotificationService _notificationService;
        //private readonly BlobStorageService _blobStorageService;
        private readonly IAzureFileShareService _azureFileShareService;
        private WsdlConfig _wsdlConfig;
        // Constructor injection of IRepositoryWrapper.
        public EmployeeAuthorityLetterController(IRepositoryWrapper repoWrapper, NotificationService notificationService,
            IWebHostEnvironment hostEnvironment,
            //BlobStorageService blobStorageService,
            IAzureFileShareService azureFileShareService,
            IOptions<WsdlConfig> wsdlConfig
            )
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _hostEnvironment = hostEnvironment;
            //_blobStorageService = blobStorageService;
            _azureFileShareService = azureFileShareService;
            _wsdlConfig = wsdlConfig.Value;
        }

        [HttpPost("getAuthorityLetters")]
        public async Task<APIResponse> getAuthorityLetters(getAuthorityLettersEmployee_Req req)
        {
            APIResponse resp = new APIResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || string.IsNullOrEmpty(userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);

            IQueryable<TblAuthorityLetters> letters = await _repoWrapper.AuthorityLetterRepo.getPendingAuthorityLettersByWarehouseInchangeID(userID);


            if (!string.IsNullOrEmpty(req.search))
                letters = letters.Where(x => x.AuthorityLetterNo.Contains(req.search));
            letters = letters.OrderByDescending(x => x.AuthorityLetterID).Skip(req.skip).Take(req.take);

            var letterList = letters.ToList();

            if (letterList == null || letterList.Count() <= 0)
            {
                resp.message = _exceptions.authorityLetterNotFound;
                resp.response = new List<getAuthorityLetters_RespList>();
            }

            var lst = letterList.Select(x => new getAuthorityLetters_RespList
            {
                letterID = x.AuthorityLetterID,
                letterNo = x.AuthorityLetterNo,
                bearerName = x.BearerName,
                bearerNIC = x.BearerNIC,
                warehouseInchage = x.Order.Warehouse.WarehouseIncharge.FirstName,
                invoiceNumber = x.INVNumber,
                farmerName = x.CreatedBy.FirstName,
                product = x.Order.Products.First().Product.Name,
                qty = x.AuthorityLetterDetails.Sum(x => x.BagQuantity).ToString(),
                isEditable = x.Status,
                status = x.Active.ToString(),
                creationDate = x.Dated,
                autoGenerated = x.IsOCRAutomated
            }).ToList();
            resp.response = new pagination_Resp
            {
                list = lst,
                filteredRecord = lst.Count(),
                totalRecord = lst.Count()
            };


            return resp;
        }
        [HttpPost("getAuthorityLetterDetail")]
        public async Task<APIResponse> getAuthorityLetterDetail(authorityLetter_GetDetails req)
        {
            APIResponse resp = new APIResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || string.IsNullOrEmpty(userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (req.letterID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            if (!User.IsInRole("Employee"))
            {
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
            }
            TblAuthorityLetters authLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.letterID);
            if (authLetter == null)
                throw new AmazonFarmerException(_exceptions.authorityLetterNotFound);
            else if (authLetter.Warehouse.InchargeID != userID)
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
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
                        productName = x.Products.Name,
                        productQTY = x.BagQuantity,
                        warehouseName = warehouseName,
                        biltyNo = x.BiltyNo,
                        truckerNo = x.TruckerNo
                    }).ToList(),
                    canEdit = authLetter.Active == EAuthorityLetterStatus.Active ? true : false
                };
                resp.response = inResp;
            }



            return resp;
        }
        [HttpPost("getInvoices")]
        public async Task<APIResponse> getInvoices(authorityLetter_GetINvoices_Req req)
        {
            APIResponse resp = new APIResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || string.IsNullOrEmpty(userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);
            else if (string.IsNullOrEmpty(req.sapOrderID))
                throw new AmazonFarmerException(_exceptions.sapOrderIDNotFound);
            else if (req.letterID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            else
            {
                TblAuthorityLetters authLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.letterID);
                if (authLetter == null)
                    throw new AmazonFarmerException(_exceptions.authorityLetterNotFound);
                string letterQTY = authLetter.AuthorityLetterDetails.First().BagQuantity.ToString();

                List<authorityLetter_Invoice> sapInvoices = await getInvoicesBySAPOrderID(req.sapOrderID);
                sapInvoices = sapInvoices.Where(x => x.qty == letterQTY).ToList();

                resp.response = sapInvoices;
            }

            return resp;
        }
        [HttpPost("validateCNIC")]
        public async Task<APIResponse> validateCNIC(validateCNIC_Req req)
        {
            APIResponse resp = new APIResponse();
            if (req.letterID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            else if (string.IsNullOrEmpty(req.content))
                throw new AmazonFarmerException(_exceptions.attachmentRequired);
            else
            {
                // Get the user ID from claims
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userID))
                    throw new AmazonFarmerException(_exceptions.userIDNotFound);
                TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
                if (loggedInUser == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                else if (!User.IsInRole("Employee"))
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);

                TblAuthorityLetters authLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.letterID);
                if (authLetter.Order.Plan.Farm.DistrictID == 0) // later the condition will be implemented here to check if the employee has access to perform this action or not
                {
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                }
                else
                {
                    if (authLetter.Status)
                        throw new AmazonFarmerException(_exceptions.authorityLetterUpdatedAlready);

                    string scannedCNIC = await performOCR(Convert.FromBase64String(req.content));

                    if (authLetter.BearerNIC != scannedCNIC)
                    {
                        resp.isError = true;
                        resp.message = (_exceptions.cnicNumberDoesNotMatach);
                    }

                    AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                    _uploadAttachmentReq attReq = new _uploadAttachmentReq()
                    {
                        requestTypeID = (int)EAttachmentType.Verify_AuthorityLetter_NIC,
                        content = req.content,
                        name = string.Concat("authorityLetter-", req.name)
                    };
                    uploadAttachmentResp attachment = await attachmentExt.uploadAttachment(attReq);
                    validateCNIC_Resp inResp = new validateCNIC_Resp()
                    {
                        cnicNumber = scannedCNIC,
                        attachmentID = attachment.id
                    };
                    resp.response = inResp;
                }
            }
            return resp;
        }
        [HttpPost("updateAuthorityLetter")]
        public async Task<JSONResponse> updateAuthorityLetter(update_AuthorityLetter_Req req)
        {
            JSONResponse resp = new JSONResponse();

            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            else if (req.authorityLetterID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            else if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
            else if (req.attachmentID == 0)
                throw new AmazonFarmerException(_exceptions.authorityLetterAttachmentRequired);
            else
            {

                TblAuthorityLetters authorityLetter = await _repoWrapper.AuthorityLetterRepo.getAuthorityLetterByID(req.authorityLetterID);
                List<TblOrders> allPaidNoConsumedAdvanceOrders = await _repoWrapper.PlanRepo.getPlanOrdersByPlanIDPaidUnConsumed(authorityLetter.Order.PlanID);

                if (authorityLetter == null)
                    throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
                else if (
                                req.attachmentID == 0 ||
                                authorityLetter.Warehouse.InchargeID != userID ||
                                authorityLetter.Status == true ||
                                string.IsNullOrEmpty(req.invoiceNumber)
                    )
                {
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                }
                else if (authorityLetter.BearerNIC != req.cnicNumber)
                {
                    throw new AmazonFarmerException(_exceptions.cnicNotValid);
                }
                else if (authorityLetter.Order.Products.FirstOrDefault().ClosingQTY > (authorityLetter.Order.Products.FirstOrDefault().ClosingQTY + authorityLetter.AuthorityLetterDetails.FirstOrDefault().BagQuantity))
                {
                    throw new AmazonFarmerException(_exceptions.authorityletterBagQuantityReached);
                }
                else if (authorityLetter.Status || authorityLetter.Active == EAuthorityLetterStatus.Approved)
                    throw new AmazonFarmerException(_exceptions.authorityLetterUpdatedAlready);
                else
                {
                    List<authorityLetter_Invoice> sapInvoices = await getInvoicesBySAPOrderID(authorityLetter.Order.SAPOrderID);
                    if (sapInvoices.Where(x => x.invoiceNumber == req.invoiceNumber).Count() <= 0)
                        throw new AmazonFarmerException(_exceptions.invoiceNotFoundInSAP);
                    tblAttachment attachmentInfo = await _repoWrapper.AttachmentRepo.getAttachmentByID(req.attachmentID);
                    if (attachmentInfo != null)
                    {
                        if (attachmentInfo.AttachmentTypes.AttachmentType != EAttachmentType.Verify_AuthorityLetter_NIC)
                            throw new AmazonFarmerException(_exceptions.invalidAttachmentType);
                        else
                            authorityLetter.AttachmentID = req.attachmentID;

                    }
                    else
                    {
                        throw new AmazonFarmerException(_exceptions.attachmentNotFound);
                    }
                    authorityLetter.Order.Products.FirstOrDefault().ClosingQTY = (authorityLetter.Order.Products.FirstOrDefault().ClosingQTY + authorityLetter.AuthorityLetterDetails.FirstOrDefault().BagQuantity);
                    if (authorityLetter.Order.Products.FirstOrDefault().ClosingQTY == authorityLetter.Order.Products.FirstOrDefault().QTY)
                    {
                        authorityLetter.Order.DeliveryStatus = EDeliveryStatus.ShipmentComplete;
                    }
                    else
                    {
                        authorityLetter.Order.DeliveryStatus = EDeliveryStatus.PartiallyDelivered;
                    }

                    authorityLetter.Status = true;
                    authorityLetter.IsOCRAutomated = req.isOCRAutomated;
                    authorityLetter.Active = EAuthorityLetterStatus.Approved;
                    authorityLetter.INVNumber = req.invoiceNumber;

                    await _repoWrapper.AuthorityLetterRepo.updateAuthorityLetter(authorityLetter);
                    await _repoWrapper.SaveAsync();
                    bool hasOpenOrders = await _repoWrapper.PlanRepo.HasPlanOrdersForCompletion(authorityLetter.Order.PlanID);
                    if (!hasOpenOrders)
                    {
                        authorityLetter.Order.Plan.Status = EPlanStatus.Completed;

                        foreach (TblOrders advanceOrder in allPaidNoConsumedAdvanceOrders)
                        {
                            await ChangeCustomerPaymentWSDL(advanceOrder);

                            advanceOrder.IsConsumed = true;
                            await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                        }
                    }

                    await _repoWrapper.AuthorityLetterRepo.updateAuthorityLetter(authorityLetter);
                    await _repoWrapper.SaveAsync();

                    #region Send Emails
                    List<NotificationRequest> notifications = new List<NotificationRequest>();
                    #region sending email to warehouse Incharge
                    //TblUser warehouseIncharge = await _repoWrapper.UserRepo.getUserByUserID(userID);
                    //NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.AuthorityLetter_Approved_WarehouseIncharge);
                    //if (warehouseIncharge != null && notificationDTO != null)
                    //{
                    //    if (!string.IsNullOrEmpty(warehouseIncharge.Email))
                    //        notifications.Add(
                    //                    new NotificationRequest
                    //                    {
                    //                        Type = ENotificationType.Email,
                    //                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = warehouseIncharge.Email, Name = warehouseIncharge.FirstName } },
                    //                        Subject = notificationDTO.title
                    //                            .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                            .Replace("<br>", Environment.NewLine)
                    //                            .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo),
                    //                        Message = notificationDTO.body
                    //                            .Replace("[firstname]", warehouseIncharge.FirstName)
                    //                            .Replace("<br>", Environment.NewLine)
                    //                            .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                            .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo)
                    //                    });
                    //    if (!string.IsNullOrEmpty(warehouseIncharge.PhoneNumber))
                    //        notifications.Add(
                    //                new NotificationRequest
                    //                {
                    //                    Type = ENotificationType.SMS,
                    //                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = warehouseIncharge.PhoneNumber, Name = warehouseIncharge.FirstName } },
                    //                    Subject = notificationDTO.title
                    //                        .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                        .Replace("<br>", Environment.NewLine)
                    //                        .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo),
                    //                    Message = notificationDTO.body
                    //                        .Replace("[firstname]", warehouseIncharge.FirstName)
                    //                        .Replace("<br>", Environment.NewLine)
                    //                        .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                        .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo)
                    //                });
                    //    if (!string.IsNullOrEmpty(warehouseIncharge.DeviceToken))
                    //        notifications.Add(
                    //                new NotificationRequest
                    //                {
                    //                    Type = ENotificationType.FCM,
                    //                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = warehouseIncharge.DeviceToken, Name = warehouseIncharge.FirstName } },
                    //                    Subject = notificationDTO.title
                    //                        .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                        .Replace("<br>", Environment.NewLine)
                    //                        .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo),
                    //                    Message = notificationDTO.body
                    //                        .Replace("[firstname]", warehouseIncharge.FirstName)
                    //                        .Replace("<br>", Environment.NewLine)
                    //                        .Replace("[OrderID]", authorityLetter.OrderID.ToString().PadLeft(10, '0'))
                    //                        .Replace("[AuthorityLetterNo]", authorityLetter.AuthorityLetterNo)
                    //                });
                    //}
                    #endregion


                    TblUser farmer = await _repoWrapper.UserRepo.getUserByUserID(authorityLetter.CreatedByID);
                    NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.AuthorityLetter_Approved_Farmer, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                    if (farmer != null && notificationDTO != null)
                    {
                        if (!string.IsNullOrEmpty(farmer.Email))
                            notifications.Add(
                                        new NotificationRequest
                                        {
                                            Type = ENotificationType.Email,
                                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer.Email, Name = farmer.FirstName } },
                                            Subject = notificationDTO.title,
                                            Message = notificationDTO.body 
                                        });
                        if (!string.IsNullOrEmpty(farmer.PhoneNumber))
                            notifications.Add(
                                    new NotificationRequest
                                    {
                                        Type = ENotificationType.SMS,
                                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer.PhoneNumber, Name = farmer.FirstName } },
                                        Subject = notificationDTO.title,
                                        Message = notificationDTO.smsBody
                                    });
                        if (!string.IsNullOrEmpty(farmer.DeviceToken))
                            notifications.Add(
                                    new NotificationRequest
                                    {
                                        Type = ENotificationType.FCM,
                                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer.DeviceToken, Name = farmer.FirstName } },
                                        Subject = notificationDTO.title,
                                        Message = notificationDTO.fcmBody
                                    });
                        if (!string.IsNullOrEmpty(farmer.Id))
                            notifications.Add(
                                    new NotificationRequest
                                    {
                                        Type = ENotificationType.Device,
                                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer.Id, Name = farmer.FirstName } },
                                        Subject = notificationDTO.title,
                                        Message = notificationDTO.deviceBody 
                                    });

                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.AuthorityLetter_Approved_Farmer;
                        replacementDTO.OrderID = authorityLetter.OrderID.ToString().PadLeft(10, '0');
                        replacementDTO.AuthorityLetterQuantity = authorityLetter.AuthorityLetterDetails.FirstOrDefault().BagQuantity.ToString();

                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    }
                    #endregion

                }
            }

            return resp;
        }

        private async Task<string> performOCR(byte[] imageBytes)
        {
            string credentialsFilePath = Path.Combine(_hostEnvironment.ContentRootPath, "visioncredentials.json");
            string VideorespText = "";
            try
            {
                // Authenticate with Google Cloud using service account credentials
                var builder = new ImageAnnotatorClientBuilder
                {
                    CredentialsPath = credentialsFilePath
                };
                var visionClient = builder.Build();
                // Create the image
                Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromBytes(imageBytes);
                // Create the OCR request
                var response = await visionClient.DetectDocumentTextAsync(image);
                // Extract and return the text detected by OCR
                if (response != null && response.Text != null)
                {
                    return await GetMaskedPatternMatch(response.Text, @"\s+(\d{5}-\d{7}-\d)");
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.textNotFoundInImage);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<string> GetMaskedPatternMatch(string inputString, string maskedPattern)
        {
            // Create a regex object
            Regex regex = new Regex(maskedPattern);
            if (inputString.Contains("Identity Number"))
            {
                Match match = regex.Match(inputString);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.imageDoesNotSeemsLikeCNIC);
                }
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.imageDoesNotSeemsLikeCNIC);
            }
        }
        private async Task<List<authorityLetter_Invoice>> getInvoicesBySAPOrderID(string sapOrderID)
        {

            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
            ZSD_AMAZ_ORDER_INV_DETAILSResponse? wsdlResponse = await wSDLFunctions.InvoiceDetailsRequest(new ZSD_AMAZ_ORDER_INV_DETAILS
            {
                I_AUBEL = sapOrderID,
                I_VBELN = ""
            });
            List<authorityLetter_Invoice> invoices = new List<authorityLetter_Invoice>();
            if (wsdlResponse != null && wsdlResponse.E_DETAILS != null && wsdlResponse.E_DETAILS.Count() > 0)
            {
                invoices = wsdlResponse.E_DETAILS.Select(inv => new authorityLetter_Invoice
                {
                    sapOrderID = sapOrderID,
                    sapFarmerCode = inv.KUNAG,
                    qty = Convert.ToInt32(inv.FKIMG.ToString()).ToString(),
                    invoiceNumber = inv.VBELN,
                    invoiceDate = inv.FKDAT,
                    invoiceAmount = "3000"
                }).ToList();
                invoices = await _repoWrapper.AuthorityLetterRepo.getAvailableInvoicesForOrder(sapOrderID, invoices);
            }

            return invoices;
        }


        private async Task<bool> ChangeCustomerPaymentWSDL(TblOrders order)
        {
            ZSD_AMAZON_CUSTOMER_PAY_CHG request = new()
            {
                COMPANY_CODE = order.CompanyCode,
                DOC_NUM = order.SAPTransactionID,
                FISCAL_YEAR = order.FiscalYear,
                REASON_CODE = "",
                TEXT = "Z041"
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            var wsdlResponse = await wSDLFunctions.ChangeCustomerPaymentRequest(request);

            if (wsdlResponse != null && wsdlResponse.ET_RETURN.Count() > 0
              && (wsdlResponse.ET_RETURN.FirstOrDefault().MSGTYP.ToUpper() == "S")
             )
            {
                return true;
            }
            else
            {
                throw new AmazonFarmerException(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
            }

        }
    }
}