using AmazonFarmer.Core.Application; // Importing necessary namespaces
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmerAPI.Extensions;
using DinkToPdf.Contracts;
using FirebaseAdmin.Messaging;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AmazonFarmerException = AmazonFarmer.Core.Application.Exceptions.AmazonFarmerException;

namespace AmazonFarmerAPI.Controllers // Defining namespace for the controller
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class PlanController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly IConverter _converter; // Converter wrapper to interact with data
        private readonly NotificationService _notificationService; // Notification service extension for notification
        private readonly GoogleLocationExtension _googleLocationExtension; // Google location extension for distance calculations

        public PlanController(IRepositoryWrapper repoWrapper, IConverter converter, NotificationService notificationService, GoogleLocationExtension googleLocationExtension) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
            _converter = converter; // Initializing the repository wrapper
            _notificationService = notificationService;
            _googleLocationExtension = googleLocationExtension;
        }

        // Endpoint for retrieving farms.
        [HttpPost("getFarms")]
        public async Task<APIResponse> getFarms(farms_Request request)
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            if (!string.IsNullOrEmpty(userID)) // Checking if user ID is not null or empty
            {
                List<tblfarm> Farms = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);

                List<farms_Resp> farmsResponse = new List<farms_Resp>();
                foreach (var farm in Farms)
                {
                    decimal allRabiPlanAcreage = farm.plans
                            .Where(p => p.Status != EPlanStatus.Rejected
                                    && p.Status != EPlanStatus.Removed
                                    && p.Status != EPlanStatus.Completed
                                    && p.SeasonID == (int)ESeasons.Rabi)
                            .SelectMany(p => p.PlanCrops.Where(pc => pc.Status == EActivityStatus.Active))
                            .Sum(pc => (decimal?)pc.Acre) ?? 0m;
                    decimal allKharifPlanAcreage = farm.plans
                            .Where(p => p.Status != EPlanStatus.Rejected
                                    && p.Status != EPlanStatus.Removed
                                    && p.Status != EPlanStatus.Completed
                                    && p.SeasonID == (int)ESeasons.Kharif)
                            .SelectMany(p => p.PlanCrops.Where(pc => pc.Status == EActivityStatus.Active))
                            .Sum(pc => (decimal?)pc.Acre) ?? 0m;

                    decimal excludingCurrentPlanAcreage = farm.plans
                            .Where(p => p.Status != EPlanStatus.Rejected
                                && p.Status != EPlanStatus.Removed
                                && p.Status != EPlanStatus.Completed
                                && p.ID != request.planID)
                            .SelectMany(p => p.PlanCrops.Where(pc => pc.Status == EActivityStatus.Active))
                            .Sum(pc => (decimal?)pc.Acre) ?? 0m;

                    farms_Resp farmResponse = new farms_Resp()
                    {
                        address = farm.Address1 ?? string.Empty,
                        farmID = farm.FarmID,
                        farmName = farm.FarmName,
                        isApproved = farm.isApproved,
                        isPrimary = farm.isPrimary,
                        acreage = farm.Acreage,
                        rabiAcreage = 0,
                        kharifAcreage = 0
                    };

                    if (request.planID == 0)
                    {
                        farmResponse.rabiAcreage = (int)(farmResponse.acreage - allRabiPlanAcreage) < 0 ? 0 : (int)(farmResponse.acreage - allRabiPlanAcreage);
                        farmResponse.kharifAcreage = (int)(farmResponse.acreage - allKharifPlanAcreage) < 0 ? 0 : (int)(farmResponse.acreage - allKharifPlanAcreage);
                        farmResponse.acreage = 0;
                    }
                    else
                    {
                        farmResponse.acreage = (int)(farmResponse.acreage - excludingCurrentPlanAcreage);
                    }

                    farmsResponse.Add(farmResponse);
                }
                resp.response = farmsResponse; // Retrieving farms by user ID
            }
            return resp; // Returning the API response
        }

        // Endpoint to save a plan
        [HttpPost("savePlan")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> savePlan(PlanDTO req) // Method to handle POST requests for saving a plan
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
                                                  // Checking if crops are provided
            await validateSetupPlan(req);
            // Add logic here to save the plan to the database or perform other operations
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims

            if (!string.IsNullOrEmpty(userID)) // Checking if user ID is not null or empty
            {
                getPlanOrder_Req _resp = new getPlanOrder_Req(); // Creating response DTO object
                tblfarm Farm = await _repoWrapper.FarmRepo.getFarmByFarmID(req.farmID, userID, languageCode);
                // Throw exception if the farm is not found or not authorized
                if (Farm == null)
                    throw new AmazonFarmerException(_exceptions.farmNotAuthorized);


                tblPlan plan = await CreatePlan(req, userID);
                tblPlan planEntity = await _repoWrapper.PlanRepo.addPlan(plan); // Adding plan and getting plan ID
                await _repoWrapper.SaveAsync();
                if (plan.Status == EPlanStatus.TSOProcessing)
                {
                    List<NotificationRequest> notifications = new List<NotificationRequest>();
                    List<TblUser> employees = await _repoWrapper.UserRepo.getTSOsByFarmID(req.farmID);
                    foreach (var item in employees)
                    {
                        List<NotificationRequest> empNotification = await SendNotificationForPlanApproval(item.FirstName,
                            item.Email, item.PhoneNumber, item?.DeviceToken, item?.Id,
                            planEntity.ID.ToString().PadLeft(10, '0'));
                        notifications.AddRange(empNotification);
                    }
                    if (notifications != null && notifications.Count() > 0)
                    {

                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_planPendingForApproval;
                        replacementDTO.PlanID = planEntity.ID.ToString().PadLeft(10, '0');
                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    }

                }
                _resp.planID = planEntity.ID.ToString().PadLeft(10, '0');
                if (planEntity.Status == EPlanStatus.TSOProcessing)
                    resp.message = "Your plan has been sent for approval";
                else if (planEntity.Status == EPlanStatus.Draft)
                    resp.message = "Your plan has been saved";
                resp.response = _resp; // Setting response
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound); // Throws exception if user ID is not found
            }
            return resp; // Returning the API response
        }
        private async Task<List<NotificationRequest>> SendNotificationForPlanApproval(string employeeName,
                string? email, string? cellNumber, string? deviceToken,
                 string? employeeUserID, string planID)
        {
            List<NotificationRequest> notifications = new List<NotificationRequest>();
            // Create notifications for all TSOs for all farms added
            NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.Employee_planPendingForApproval, "EN");

            if (notificationDTO != null)
            {
                var farmerEmail = new NotificationRequest
                {
                    Type = ENotificationType.Email,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = email, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.body
                };
                notifications.Add(farmerEmail);
                var farmerFCM = new NotificationRequest
                {
                    Type = ENotificationType.FCM,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = deviceToken, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.fcmBody
                };
                notifications.Add(farmerFCM);
                var farmerSMS = new NotificationRequest
                {
                    Type = ENotificationType.SMS,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = cellNumber, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.smsBody
                };
                notifications.Add(farmerSMS);
                var farmerDevice = new NotificationRequest
                {
                    Type = ENotificationType.Device,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = employeeUserID, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.deviceBody
                };
                notifications.Add(farmerDevice);
            }
            return notifications;

        }
        private async Task validateSetupPlan(PlanDTO req)
        {
            if (req.crops == null || req.crops.Count() <= 0)
            {
                throw new AmazonFarmerException(_exceptions.cropsRequired); // Throws exception if crops are not provided
            }
            // Checking if each crop has products
            else if (req.crops.Where(x => x.products.Count() <= 0 || x.products == null).Count() > 0)
            {
                throw new AmazonFarmerException(_exceptions.productsRequired); // Throws exception if any crop does not have products
            }
            // Checking if season ID is provided
            else if (req.seasonID == 0)
            {
                throw new AmazonFarmerException(_exceptions.seasonRequired); // Throws exception if season ID is not provided
            }
            // Checking if farm ID is provided
            else if (req.farmID == 0)
            {
                throw new AmazonFarmerException(_exceptions.atleastOneFarm); // Throws exception if farm ID is not provided
            }
            // Checking if warehouse ID is provided
            else if (req.warehouseID == 0)
            {
                throw new AmazonFarmerException(_exceptions.warehouseNotFound); // Throws exception if farm ID is not provided
            }
            else if (string.IsNullOrEmpty(req.farmerComment) && req.requestType == 1)
                throw new AmazonFarmerException(_exceptions.farmerCommentRequired);
        }
        private async Task<tblPlan> CreatePlan(PlanDTO req, string userID)
        {

            List<tblPlanCrops> PlanCrops = new();
            // Iterate over each crop in the PlanDTO and add them to the database
            foreach (var item in req.crops)
            {


                List<tblPlanProduct> CropPlanProducts = new();

                // Iterate over each product in the crop and add them to the database
                foreach (var product in item.products)
                {
                    tblPlanProduct productReq = new tblPlanProduct()
                    {
                        ProductID = product.productID,
                        Date = DateTime.ParseExact(product.deliveryDate, "yyyy-MM-dd", null),
                        Qty = Convert.ToInt32(product.qty)
                    };

                    CropPlanProducts.Add(productReq);
                }

                List<tblPlanService> CropPlanServices = new();
                // Iterate over each service in the crop and add them to the database
                foreach (var service in item.services)
                {
                    tblPlanService serviceReq = new tblPlanService()
                    {
                        ServiceID = service.serviceID,
                        LastHarvestDate = DateTime.ParseExact(service.lastHarvestDate, "yyyy-MM-dd", null),
                        LandPreparationDate = DateTime.ParseExact(service.landPreparationDate, "yyyy-MM-dd", null),
                        SewingDate = DateTime.ParseExact(service.sewingDate, "yyyy-MM-dd", null),
                    };
                    CropPlanServices.Add(serviceReq);
                }

                int cropGroupID = await _repoWrapper.PlanRepo.getCropGroupIDByCropIDs(item.cropIDs);

                tblPlanCrops cropReq = new tblPlanCrops()
                {
                    CropGroupID = cropGroupID,
                    //CropID = item.cropID,
                    Acre = item.crop_acreage,
                    PlanProducts = CropPlanProducts,
                    PlanServices = CropPlanServices
                };

                PlanCrops.Add(cropReq);
            }
            // Create a new plan entity based on the provided PlanDTO
            EPlanStatus planStatus = req.requestType == 0 ? EPlanStatus.Draft : EPlanStatus.TSOProcessing;
            tblPlan planReq = new tblPlan()
            {
                FarmID = req.farmID,
                SeasonID = req.seasonID,
                WarehouseID = req.warehouseID,
                UserID = userID,
                Status = planStatus,
                PlanCrops = PlanCrops,
                FarmerComment = req.farmerComment,
                PlanChangeStatus = EPlanChangeRequest.Default,
                ModeOfPayment = (EModeOfPayment?)(req.modeOfPayment ?? (int)EModeOfPayment.Partial_Payment)
            };

            return planReq;



        }
        // Endpoint to edit a plan
        [HttpPut("editPlan")] // Defines the HTTP PUT method and endpoint route
        public async Task<APIResponse> editPlan(EditPlanDTO req) // Method to handle PUT requests for editing a plan
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            getPlanOrder_Req _resp = new getPlanOrder_Req(); // Creating response DTO object
                                                             // Checking if planID is provided
            if (req.planID == 0)
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            // Checking if crops are provided
            //else if (req.crops == null || req.crops.Count() <= 0)
            //{
            //    throw new AmazonFarmerException(_exceptions.cropsRequired); // Throws exception if crops are not provided
            //}
            //// Checking if each crop has products
            //else if (req.crops.Where(x => x.products.Count() <= 0 || x.products == null).Count() > 0)
            //{
            //    throw new AmazonFarmerException(_exceptions.productsRequired); // Throws exception if any crop does not have products
            //}
            // Checking if season ID is provided
            else if (req.seasonID == 0)
            {
                throw new AmazonFarmerException(_exceptions.seasonRequired); // Throws exception if season ID is not provided
            }
            // Checking if farm ID is provided
            else if (req.farmID == 0)
            {
                throw new AmazonFarmerException(_exceptions.atleastOneFarm); // Throws exception if farm ID is not provided
            }
            else if (string.IsNullOrEmpty(req.farmerComment) && req.requestType == 1)
                throw new AmazonFarmerException(_exceptions.farmerCommentRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(languageCode)) // Checking if user ID is not null or empty
                {
                    tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(req.planID, userID, languageCode);

                    bool isPlainCompleted = plan.Status == EPlanStatus.Completed ? true : false;
                    bool isNullableCropsEnabled = (plan!.Orders!.Count() == 0 && (req.crops == null || req.crops.Count() == 0)) ? true : false;

                    List<EOrderPaymentStatus> orderPaymentStatus = new List<EOrderPaymentStatus>() { EOrderPaymentStatus.Paid, EOrderPaymentStatus.PaymentProcessing, EOrderPaymentStatus.Acknowledged, EOrderPaymentStatus.LedgerUpdate, EOrderPaymentStatus.Refund };
                    bool isplanPaid = plan!.Orders!.Where(x => orderPaymentStatus.Contains(x.PaymentStatus)).Count() == plan!.Orders!.Count() && plan!.Orders!.Count() > 0 ? true : false;

                    if (isPlainCompleted || isNullableCropsEnabled)
                    {
                        throw new AmazonFarmerException(_exceptions.planNotInValidState);
                    }

                    plan = await EditPlan(plan, req); // Editing the plan
                    await _repoWrapper.PlanRepo.updatePlan(plan); // updating plan
                    await _repoWrapper.SaveAsync(); // Saving Changes
                    if (plan.Status == EPlanStatus.TSOProcessing)
                    {
                        List<NotificationRequest> notifications = new List<NotificationRequest>();
                        List<TblUser> employees = await _repoWrapper.UserRepo.getTSOsByFarmID(req.farmID);
                        foreach (var item in employees)
                        {
                            List<NotificationRequest> empNotification = await SendNotificationForPlanApproval(item.FirstName,
                                item.Email, item.PhoneNumber, item?.DeviceToken, item?.Id,
                                plan.ID.ToString().PadLeft(10, '0'));
                            notifications.AddRange(empNotification);
                        }
                        if (notifications != null && notifications.Count() > 0)
                        {

                            NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                            replacementDTO.PlanID = plan.ID.ToString().PadLeft(10, '0');
                            replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_planPendingForApproval;
                            await _notificationService.SendNotifications(notifications, replacementDTO);
                        }

                    }
                    _resp.planID = plan.ID.ToString();
                    if (plan.Status == EPlanStatus.TSOProcessing)
                        resp.message = "Your plan has been sent for approval";
                    else if (plan.Status == EPlanStatus.Draft)
                        resp.message = "Your plan has been saved";
                    resp.response = _resp;
                }
                else
                    throw new AmazonFarmerException(_exceptions.userIDNotFound); // Throws exception if user ID is not found

            }
            return resp; // Returning the API response
        }
        private async Task<tblPlan> EditPlan(tblPlan plan, EditPlanDTO req)
        {
            plan.FarmID = req.farmID;
            plan.SeasonID = req.seasonID;

            #region new method 
            //new plan crops 
            List<tblPlanCrops> PlanCrops = new List<tblPlanCrops>();
            //finding plan crops which were added before and removed now
            List<tblPlanCrops> unUsedPlanCrop = plan.PlanCrops.ToList();
            if (req.crops.Count() > 0)
            {
                unUsedPlanCrop = plan.PlanCrops
                .Where(x => req.crops.Where(y => y.planCropID != x.ID).Count() > 0 && x.Status == EActivityStatus.Active).ToList();
            }


            //check if there's any unsed plan crops
            if (unUsedPlanCrop != null && unUsedPlanCrop.Count() > 0)
            {
                //removing unused plan crops
                foreach (tblPlanCrops item in unUsedPlanCrop)
                {
                    item.Status = EActivityStatus.DeActive;
                    await _repoWrapper.PlanRepo.updatePlanCrop(item);
                }
            }

            // adding and updating plan crops
            foreach (PlanCrop_Req item in req.crops)
            {
                tblPlanCrops planCrop = plan.PlanCrops.Where(x => x.ID == item.planCropID).FirstOrDefault();
                if (planCrop != null)
                {
                    //planCrop.CropID = item.cropID;
                    planCrop.Acre = item.crop_acreage;
                    planCrop.PlanCropEndorse = EPlanCropEndorse.Ok;
                    planCrop.Status = EActivityStatus.Active;

                    //finding plan products which were added before and removed now
                    List<tblPlanProduct> unUsedPlanProducts = planCrop.PlanProducts
                        .Where(x => item.products.Where(y => y.planProductID != x.ID).Count() > 0 && x.Status == EActivityStatus.Active).ToList();
                    //check if there's any unsed plan product
                    if (unUsedPlanProducts != null && unUsedPlanProducts.Count() > 0)
                    {
                        //removing unsed plan products
                        foreach (tblPlanProduct planProduct in unUsedPlanProducts)
                        {
                            planProduct.Status = EActivityStatus.DeActive;
                            await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);
                        }
                    }
                    //finding plan services which were added before and removed now
                    List<tblPlanService> unUsedPlanServices = planCrop.PlanServices
                        .Where(x => item.services.Where(y => y.planServiceID != x.ID).Count() > 0 && x.Status == EActivityStatus.Active).ToList();
                    //check if there's any unsed plan product
                    if (unUsedPlanServices != null && unUsedPlanServices.Count() > 0)
                    {
                        //removing unsed plan products
                        foreach (tblPlanService unUsedPlanService in unUsedPlanServices)
                        {
                            unUsedPlanService.Status = EActivityStatus.DeActive;
                            await _repoWrapper.PlanRepo.updatePlanService(unUsedPlanService);
                        }
                    }

                    //adding and updating plan product
                    foreach (addCropPlan_Req requestedProduct in item.products)
                    {
                        tblPlanProduct planProduct = planCrop.PlanProducts.Where(x => x.ID == requestedProduct.planProductID).FirstOrDefault();
                        if (planProduct != null && planProduct.ID != 0)
                        {
                            //updating values
                            planProduct.ProductID = requestedProduct.productID;
                            planProduct.Qty = planProduct.DeliveredQty + requestedProduct.qty;
                            planProduct.Date = DateTime.ParseExact(requestedProduct.deliveryDate, "yyyy-MM-dd", null);
                            planProduct.Status = EActivityStatus.Active;
                            //updating plan product
                            await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);
                        }
                        else
                        {
                            //setting values
                            planProduct = new tblPlanProduct()
                            {
                                PlanCropID = planCrop.ID,
                                ProductID = requestedProduct.productID,
                                Qty = 0 + requestedProduct.qty,
                                Date = DateTime.ParseExact(requestedProduct.deliveryDate, "yyyy-MM-dd", null),
                                Status = EActivityStatus.Active
                            };
                            //adding new plan product
                            await _repoWrapper.PlanRepo.addPlanProduct(planProduct);
                        }
                    }
                    //updating plan services
                    foreach (Server_Req requestedService in item.services)
                    {
                        tblPlanService planService = new tblPlanService();
                        planService = planCrop.PlanServices.Where(x => x.ID == requestedService.planServiceID).FirstOrDefault();
                        if (planService != null && planService.ID > 0)
                        {
                            //updating values
                            planService.ServiceID = requestedService.serviceID;
                            planService.LastHarvestDate = DateTime.ParseExact(requestedService.lastHarvestDate, "yyyy-MM-dd", null);
                            planService.LandPreparationDate = DateTime.ParseExact(requestedService.landPreparationDate, "yyyy-MM-dd", null);
                            planService.SewingDate = DateTime.ParseExact(requestedService.sewingDate, "yyyy-MM-dd", null);
                            planService.Status = EActivityStatus.Active;
                            //updating plan service
                            await _repoWrapper.PlanRepo.updatePlanService(planService);
                        }
                        else
                        {
                            //setting values
                            planService = new tblPlanService()
                            {
                                PlanCropID = item.planCropID,
                                ServiceID = requestedService.serviceID,
                                LastHarvestDate = DateTime.ParseExact(requestedService.lastHarvestDate, "yyyy-MM-dd", null),
                                LandPreparationDate = DateTime.ParseExact(requestedService.landPreparationDate, "yyyy-MM-dd", null),
                                SewingDate = DateTime.ParseExact(requestedService.sewingDate, "yyyy-MM-dd", null),
                                Status = EActivityStatus.Active
                            };
                            //adding new plan service
                            await _repoWrapper.PlanRepo.addPlanService(planService);
                        }
                    }


                    await _repoWrapper.PlanRepo.updatePlanCrop(planCrop);
                }
                else
                {
                    List<tblPlanService> PlanServices = new List<tblPlanService>();
                    List<tblPlanProduct> PlanProducts = new List<tblPlanProduct>();
                    //adding new plan services on crop
                    foreach (Server_Req requestedService in item.services)
                    {
                        tblPlanService PlanService = new tblPlanService()
                        {
                            ServiceID = requestedService.serviceID,
                            LastHarvestDate = DateTime.ParseExact(requestedService.lastHarvestDate, "yyyy-MM-dd", null),
                            LandPreparationDate = DateTime.ParseExact(requestedService.landPreparationDate, "yyyy-MM-dd", null),
                            SewingDate = DateTime.ParseExact(requestedService.sewingDate, "yyyy-MM-dd", null),
                            Status = EActivityStatus.Active
                        };
                        //adding new plan service
                        PlanServices.Add(PlanService);
                    }
                    //adding new plan product on crop
                    foreach (addCropPlan_Req requestedProduct in item.products)
                    {
                        tblPlanProduct PlanProduct = new tblPlanProduct
                        {
                            ProductID = requestedProduct.productID,
                            Qty = requestedProduct.qty,
                            Date = DateTime.ParseExact(requestedProduct.deliveryDate, "yyyy-MM-dd", null),
                            Status = EActivityStatus.Active
                        };
                        //adding new plan product
                        PlanProducts.Add(PlanProduct);
                    }
                    int cropGroupID = await _repoWrapper.PlanRepo.getCropGroupIDByCropIDs(item.cropIDs);

                    //setting values
                    planCrop = new tblPlanCrops()
                    {
                        PlanID = plan.ID,
                        CropGroupID = cropGroupID,
                        //CropID = item.cropID,
                        Acre = item.crop_acreage,
                        PlanCropEndorse = EPlanCropEndorse.Ok,
                        Status = EActivityStatus.Active,
                        PlanProducts = PlanProducts,
                        PlanServices = PlanServices
                    };
                    PlanCrops.Add(planCrop);
                }
            }
            #endregion
            EPlanStatus planStatus = req.requestType == 0 ? EPlanStatus.Draft : EPlanStatus.TSOProcessing;
            plan.WarehouseID = req.warehouseID;
            plan.PlanCrops.AddRange(PlanCrops);
            plan.PlanChangeStatus = EPlanChangeRequest.Default;
            plan.Status = planStatus;
            plan.FarmerComment = req.farmerComment;
            if (plan.Status == EPlanStatus.TSOProcessing)
            {
                plan.Reason = string.Empty;
            }
            return plan;

            #region old method (will remove after discussion )
            //List<tblPlanCrops> PlanCrops = new();
            //// Iterate over each crop in the PlanDTO and add them to the database
            //foreach (var item in req.crops)
            //{
            //    List<tblPlanProduct> CropPlanProducts = new();

            //    // Iterate over each product in the crop and add them to the database
            //    foreach (var product in item.products)
            //    {
            //        tblPlanProduct productReq = new tblPlanProduct()
            //        {
            //            ProductID = product.productID,
            //            Date = DateTime.ParseExact(product.deliveryDate, "yyyy-MM-dd", null),
            //            Qty = Convert.ToInt32(product.qty)
            //        };

            //        CropPlanProducts.Add(productReq);
            //    }

            //    List<tblPlanService> CropPlanServices = new();
            //    // Iterate over each service in the crop and add them to the database
            //    foreach (var service in item.services)
            //    {
            //        tblPlanService serviceReq = new tblPlanService()
            //        {
            //            ServiceID = service.serviceID,
            //            LastHarvestDate = DateTime.ParseExact(service.lastHarvestDate, "yyyy-MM-dd", null),
            //            LandPreparationDate = DateTime.ParseExact(service.landPreparationDate, "yyyy-MM-dd", null),
            //            SewingDate = DateTime.ParseExact(service.sewingDate, "yyyy-MM-dd", null),
            //        };
            //        CropPlanServices.Add(serviceReq);
            //    }

            //    tblPlanCrops cropReq = new tblPlanCrops()
            //    {
            //        CropID = item.cropID,
            //        Acre = item.crop_acreage,
            //        PlanProducts = CropPlanProducts,
            //        PlanServices = CropPlanServices
            //    };

            //    PlanCrops.Add(cropReq);
            //}
            #endregion


        }
        // Endpoint to get plans
        [HttpPost("getPlans")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> getPlans(getPlan_Req req) // Method to handle POST requests for getting plans
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            pagination_Resp inResp = new();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(languageCode)) // Checking if both user ID and language code are provided
            {
                IQueryable<tblPlan> plans = await _repoWrapper.PlanRepo.getPlansByUserIDandLanguageCode(userID, languageCode); // Retrieving plans by user ID and language code

                if (req.requestTypeID != 0)
                {
                    if (req.requestTypeID == 3)
                    {
                        plans = plans.Where(x => x.Status == EPlanStatus.TSOProcessing || x.Status == EPlanStatus.RSMProcessing || x.Status == EPlanStatus.NSMProcessing);
                    }
                    else if (req.requestTypeID == 2)
                    {
                        plans = plans.Where(x => x.Orders.Where(x => x.PaymentStatus == EOrderPaymentStatus.Paid && x.DeliveryStatus != EDeliveryStatus.ShipmentComplete).Count() > 0);
                    }
                    else if (req.requestTypeID == 1)
                    {
                        plans = plans.Where(x => x.Orders.Where(x => x.PaymentStatus == EOrderPaymentStatus.NonPaid).Count() > 0);
                    }
                    else if (req.requestTypeID == 4)
                    {
                        plans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending);
                    }
                    else if (req.requestTypeID == 5)
                    {
                        plans = plans.Where(x => x.Status == EPlanStatus.Draft);
                    }
                }

                inResp.totalRecord = plans.Count();

                List<EPlanStatus> approverStates = new List<EPlanStatus>() { EPlanStatus.TSOProcessing, EPlanStatus.RSMProcessing, EPlanStatus.NSMProcessing };
                List<EOrderPaymentStatus> orderPaymentStatus = new List<EOrderPaymentStatus>() { EOrderPaymentStatus.Paid, EOrderPaymentStatus.PaymentProcessing, EOrderPaymentStatus.Acknowledged, EOrderPaymentStatus.LedgerUpdate, EOrderPaymentStatus.Refund };

                plans = plans.Skip(req.skip).Take(req.take);

                // Project the results into getPlans_Resp DTO
                List<getPlans_Resp> lst = await
                plans.Skip(req.skip).Take(req.take).OrderByDescending(x => x.ID).Select(x => new getPlans_Resp
                {
                    isSummaryAvailable = x!.PlanCrops!.Where(y => y.Status == EActivityStatus.DeActive).Count() == x!.PlanCrops!.Count() && x!.PlanCrops!.Count() > 0 ? false : true,
                    // Format planID as a 10-digit string with leading zeros
                    planID = x.ID.ToString().PadLeft(10, '0'),
                    // Assign farm name from Farm entity
                    farm = x.Farm.FarmName,
                    // Get season translation in the specified language
                    season = x.Season.SeasonTranslations.First().Translation,
                    // Format farm acreage and assign to farmAcreage
                    farmAcreage = x.Farm.Acreage.ToString(),
                    // Assign statusID as integer value of plan status
                    statusID = (int)x.Status,
                    // Assign planChangeStatusID  as integer value of plan status
                    planChangeStatusID = (int)x.PlanChangeStatus,
                    // Assign reason from plan entity
                    reason = x.Reason ?? string.Empty,
                    canDelete = x.Orders.Any(x => orderPaymentStatus.Contains(x.PaymentStatus)) ? false : x.Status != EPlanStatus.Draft ? false : true,
                    //canRequestForChanges = x.Orders.Where(x => orderPaymentStatus.Contains(x.PaymentStatus)).Count() == x.Orders.Count() && x.Orders.Count() > 0 ? false : (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined) && x.Status != EPlanStatus.Draft ? true : false,
                    canRequestForChanges = (/*x.Status == EPlanStatus.Draft && x.Status != EPlanStatus.Removed && */(approverStates.Contains(x.Status) || x.Status == EPlanStatus.Approved) && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined)) ? true : false,
                    canSubmitForApproval = (x.Status == EPlanStatus.Draft || x.Status == EPlanStatus.Revert) ? true : false,
                    canViewOrder = x.Orders.Any() ? true : false,
                    //canViewOrder = (x.Status == EPlanStatus.Completed)  
                    //    && (x.PlanChangeStatus == EPlanChangeRequest.Default 
                    //        || x.PlanChangeStatus == EPlanChangeRequest.Declined) ? true : false,
                    canOrderPayment = false
                    //canOrderPayment = x.Orders.Where(x => x.PaymentStatus == EOrderPaymentStatus.NonPaid && x.DuePaymentDate < DateTime.UtcNow).Count() > 0 && (x.Status == EPlanStatus.Approved) && x.PlanChangeStatus == EPlanChangeRequest.Default ? true : false
                }).ToListAsync();

                inResp.list = lst.ToList();
                inResp.filteredRecord = lst.Count();

                resp.response = inResp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound); // Throws exception if either user ID or language code is not found
            }
            return resp; // Returning the API response
        }

        // Endpoint to get plan orders
        [HttpPost("getPlanOrders")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> getPlanOrders(getPlanOrder_Req req) // Method to handle POST requests for getting plan orders
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            if (string.IsNullOrEmpty(req.planID)) // Checking if plan ID is provided
                throw new AmazonFarmerException(_exceptions.planIDRequired); // Throws exception if plan ID is not provided
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(languageCode)) // Checking if both user ID and language code are provided
                {
                    tblPlan plan = await _repoWrapper.PlanRepo.getPlanOrderByUserIDandLanguageCode(userID, languageCode, Convert.ToInt32(req.planID)); // Retrieving plan order by user ID, language code, and plan ID
                    if (plan == null)
                    {
                        throw new AmazonFarmerException(_exceptions.planNotFound);
                    }

                    tblfarm farm = plan.Farm;
                    tblSeason season = plan.Season;
                    List<TblOrders> orders = plan.Orders;

                    TblOrders advanceOrder = orders.Where(x => x.OrderStatus != EOrderStatus.Blocked
                        && (x.OrderType == EOrderType.Advance)).FirstOrDefault();
                    TblOrders? advanceOrderReconcile = orders.Where(x => x.OrderStatus != EOrderStatus.Blocked
                        && (x.OrderType == EOrderType.AdvancePaymentReconcile)).FirstOrDefault();

                    if (advanceOrder == null)
                        throw new AmazonFarmerException(_exceptions.advanceOrderNotFound);
                    else if (orders == null || orders.Count() <= 0)
                        throw new AmazonFarmerException(_exceptions.orderNotFound);
                    else if (season == null)
                        throw new AmazonFarmerException(_exceptions.seasonNotFound);
                    else if (farm == null)
                        throw new AmazonFarmerException(_exceptions.farmNotFound);

                    resp.response = new getPlanOrder_Resp
                    {
                        canPay = advanceOrder == null || advanceOrder.isLocked ? false :
                                    advanceOrder.DuePaymentDate < DateTime.UtcNow ? false :
                                        advanceOrder.PaymentStatus == EOrderPaymentStatus.NonPaid ? true :
                                        false,
                        // Format planID as a 10-digit string with leading zeros
                        planID = plan.ID.ToString().PadLeft(10, '0'),
                        // Check if Farm is not null and assign farmName, otherwise assign null
                        farmName = farm != null ? farm.FarmName : null,
                        advancestatusDescription = advanceOrder.PaymentStatus != EOrderPaymentStatus.Paid && advanceOrder.DuePaymentDate < DateTime.UtcNow ? ConfigExntension.GetEnumDescription(EOrderStatus.Expired) : ConfigExntension.GetEnumDescription(advanceOrder.PaymentStatus),
                        // Set advancePercent and advanceAmount values (hardcoded for demonstration)
                        advancePercent = advanceOrder != null ? (advanceOrder.AdvancePercent.ToString() + " %") : "0",
                        advanceAmount = advanceOrder != null ? advanceOrder.OrderAmount.ToString("N2") : "0",
                        advancePaymentStatus = advanceOrder != null ? (int)advanceOrder.PaymentStatus : 0,
                        advanceOrderStatus = advanceOrder != null ? !advanceOrder.isLocked ?
                            ((advanceOrder.PaymentStatus != EOrderPaymentStatus.Paid && advanceOrder.DuePaymentDate < DateTime.UtcNow)
                                ? (int)EOrderStatus.Expired : (int)advanceOrder.OrderStatus) : (int)EOrderStatus.Blocked : 0,
                        advancePaymentOrderID = advanceOrder != null ? Convert.ToInt64(advanceOrder.OrderID) : 0,
                        // Check if Season and SeasonTranslations are not null, assign seasonName, otherwise assign null
                        seasonName = season != null ?
                                season.SeasonTranslations
                                .Select(y => y.Translation)
                                .FirstOrDefault() :
                            null,
                        // Project orders into getPlanOrder DTO
                        advanceReconciledOrders = orders != null ? orders.Where(x => x.OrderType == EOrderType.AdvancePaymentReconcile && x.OrderStatus != EOrderStatus.Deleted).Select(o => new getPlanOrder
                        {
                            closingQty = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().ClosingQTY.ToString("N0") : "0",
                            deliveryStatusDescription = ConfigExntension.GetEnumDescription(o.DeliveryStatus),
                            orderStatusDescription = o.isLocked ? ConfigExntension.GetEnumDescription(EOrderStatus.Locked) : ConfigExntension.GetEnumDescription(o.OrderStatus),
                            paymentStatusDescription = ConfigExntension.GetEnumDescription(o.PaymentStatus),
                            duePaymentDate = o.DuePaymentDate,
                            canPay = !o.isLocked ?
                                        o.OrderStatus != EOrderStatus.Active || o.DuePaymentDate < DateTime.UtcNow ?
                                        false : o.PaymentStatus == EOrderPaymentStatus.NonPaid ?
                                            true :
                                        false :
                                    false,
                            canViewAuthorityLetter = false, //No need for authority letter on Advance Payment Reconcilation
                            sapOrderID = o.SAPOrderID ?? string.Empty,
                            orderID = o.OrderID,
                            // Assuming date is a DateTime property, convert it to string representation
                            date = o.ExpectedDeliveryDate,
                            // Set statusID (hardcoded for demonstration)
                            statusID = ((o.PaymentStatus != EOrderPaymentStatus.Paid && o.DuePaymentDate < DateTime.UtcNow) ? (int)EOrderStatus.Expired : (int)o.OrderStatus),
                            // Set paymentID (hardcoded for demonstration)
                            paymentStatusID = (int)o.PaymentStatus,
                            // Calculate totalBags by summing up Qty from PlanProducts and formatting it
                            totalBags = o.Products != null && o.Products.Count() != 0 ? o.Products.Select(x => x.QTY).Sum().ToString("N0") : "0",
                            isProductVisible = false,
                            // Group PlanProducts by date and project them into getOrderProduct DTO
                            products =
                                new List<getOrderProduct>
                                    {
                                    new getOrderProduct()
                                        { productName = ConfigExntension.GetEnumDescription(o.OrderType) + " doesn't contain any product", bag = "0" }
                                },
                            orderPrice = o.OrderAmount,
                            approvalDatePrice = o.ApprovalDatePrice == 0 ? 0.0m : o.ApprovalDatePrice,
                            paymentDatePrice = o.PaymentDatePrice ?? 0.0m,
                            reconciliationPrice = o.ReconciliationAmount == 0 ? 0.0m : o.ReconciliationAmount,
                            balanceQTY = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().QTY.ToString("N0") : "0"
                        })
                        .OrderBy(po => po.date).ToList() : new List<getPlanOrder>(),
                        productReconciledOrders = orders != null ? orders.Where(x => x.OrderType == EOrderType.OrderReconcile && x.OrderStatus != EOrderStatus.Deleted).Select(o => new getPlanOrder
                        {
                            closingQty = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().ClosingQTY.ToString("N0") : "0",
                            deliveryStatusDescription = ConfigExntension.GetEnumDescription(o.DeliveryStatus),
                            orderStatusDescription = o.isLocked ? ConfigExntension.GetEnumDescription(EOrderStatus.Locked) : ConfigExntension.GetEnumDescription(o.OrderStatus),
                            paymentStatusDescription = ConfigExntension.GetEnumDescription(o.PaymentStatus),
                            duePaymentDate = o.DuePaymentDate,
                            canPay = !o.isLocked ?
                                        o.OrderStatus != EOrderStatus.Active || o.DuePaymentDate < DateTime.UtcNow ?
                                        false : o.PaymentStatus == EOrderPaymentStatus.NonPaid ?
                                            advanceOrder == null || (advanceOrder?.PaymentStatus != EOrderPaymentStatus.Paid) ? false :
                                                advanceOrderReconcile == null || (advanceOrderReconcile?.PaymentStatus == EOrderPaymentStatus.Paid || o.OrderType == EOrderType.AdvancePaymentReconcile) ? true :
                                                false :
                                        false :
                                    false,
                            canViewAuthorityLetter = false, //assuming no need for the authority letter on product reconcilation kamran
                            //canViewAuthorityLetter = !o.isLocked ? o.PaymentStatus == EOrderPaymentStatus.Paid ? o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile ? true : false : false : false,
                            sapOrderID = o.SAPOrderID ?? string.Empty,
                            orderID = o.OrderID,
                            // Assuming date is a DateTime property, convert it to string representation
                            date = o.ExpectedDeliveryDate,
                            // Set statusID (hardcoded for demonstration)
                            statusID = ((o.PaymentStatus != EOrderPaymentStatus.Paid && o.DuePaymentDate < DateTime.UtcNow) ? (int)EOrderStatus.Expired : (int)o.OrderStatus),
                            // Set paymentID (hardcoded for demonstration)
                            paymentStatusID = (int)o.PaymentStatus,
                            // Calculate totalBags by summing up Qty from PlanProducts and formatting it
                            totalBags = o.Products != null && o.Products.Count() != 0 ? o.Products.Select(x => x.QTY).Sum().ToString("N0") : "0",
                            isProductVisible = o.OrderType == EOrderType.AdvancePaymentReconcile || o.OrderType == EOrderType.OrderReconcile ? false : true,
                            // Group PlanProducts by date and project them into getOrderProduct DTO
                            products = o.Products != null && o.Products.Count() != 0 ?
                                o.Products
                                    .Select(group => new getOrderProduct
                                    {
                                        // Check if ProductTranslations is not null, assign productName, otherwise assign null
                                        productName = group.Product.ProductTranslations.FirstOrDefault().Text,
                                        // Calculate total bag count for each product group and format it
                                        bag = group.QTY.ToString("N0")
                                    }).ToList() :
                                new List<getOrderProduct>
                                    {
                                    new getOrderProduct()
                                        { productName = ConfigExntension.GetEnumDescription(o.OrderType) + " doesn't contain any product", bag = "0" }
                                },
                            orderPrice = o.OrderAmount,
                            approvalDatePrice = o.ApprovalDatePrice == 0 ? 0.0m : o.ApprovalDatePrice,
                            paymentDatePrice = o.PaymentDatePrice ?? 0.0m,
                            reconciliationPrice = o.ReconciliationAmount == 0 ? 0.0m : o.ReconciliationAmount,
                            balanceQTY = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().QTY.ToString("N0") : "0"
                        })
                        .OrderBy(po => po.date).ToList() : new List<getPlanOrder>(),
                        orders = orders != null ? orders.Where(x => x.OrderType == EOrderType.Product && x.OrderStatus != EOrderStatus.Deleted).Select(o => new getPlanOrder
                        {
                            closingQty = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().ClosingQTY.ToString("N0") : "0",
                            deliveryStatusDescription = ConfigExntension.GetEnumDescription(o.DeliveryStatus),
                            orderStatusDescription = o.isLocked ? ConfigExntension.GetEnumDescription(EOrderStatus.Locked) : ConfigExntension.GetEnumDescription(o.OrderStatus),
                            paymentStatusDescription = ConfigExntension.GetEnumDescription(o.PaymentStatus),
                            duePaymentDate = o.DuePaymentDate,
                            canPay = !o.isLocked ?
                                        o.OrderStatus != EOrderStatus.Active || o.DuePaymentDate < DateTime.UtcNow ?
                                        false : o.PaymentStatus == EOrderPaymentStatus.NonPaid ?
                                            advanceOrder == null || (advanceOrder?.PaymentStatus != EOrderPaymentStatus.Paid) ? false :
                                                advanceOrderReconcile == null || (advanceOrderReconcile?.PaymentStatus == EOrderPaymentStatus.Paid) ? true :
                                                false :
                                        false :
                                    false,
                            canViewAuthorityLetter = !o.isLocked ? o.PaymentStatus == EOrderPaymentStatus.Paid ? o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile ? true : false : false : false,
                            sapOrderID = o.SAPOrderID ?? string.Empty,
                            orderID = o.OrderID,
                            // Assuming date is a DateTime property, convert it to string representation
                            date = o.ExpectedDeliveryDate,
                            // Set statusID (hardcoded for demonstration)
                            statusID = ((o.PaymentStatus != EOrderPaymentStatus.Paid && o.DuePaymentDate < DateTime.UtcNow) ? (int)EOrderStatus.Expired : (int)o.OrderStatus),
                            // Set paymentID (hardcoded for demonstration)
                            paymentStatusID = (int)o.PaymentStatus,
                            // Calculate totalBags by summing up Qty from PlanProducts and formatting it
                            totalBags = o.Products != null && o.Products.Count() != 0 ? o.Products.Select(x => x.QTY).Sum().ToString("N0") : "0",
                            isProductVisible = o.OrderType == EOrderType.AdvancePaymentReconcile || o.OrderType == EOrderType.OrderReconcile ? false : true,
                            // Group PlanProducts by date and project them into getOrderProduct DTO
                            products = o.Products != null && o.Products.Count() != 0 ?
                                o.Products
                                    .Select(group => new getOrderProduct
                                    {
                                        // Check if ProductTranslations is not null, assign productName, otherwise assign null
                                        productName = group.Product.ProductTranslations.FirstOrDefault().Text,
                                        // Calculate total bag count for each product group and format it
                                        bag = group.QTY.ToString("N0")
                                    }).ToList() :
                                new List<getOrderProduct>
                                    {
                                    new getOrderProduct()
                                        { productName = ConfigExntension.GetEnumDescription(o.OrderType) + " doesn't contain any product", bag = "0" }
                                },
                            orderPrice = o.OrderAmount,
                            approvalDatePrice = o.ApprovalDatePrice == 0 ? 0.0m : o.ApprovalDatePrice,
                            paymentDatePrice = o.PaymentDatePrice ?? 0.0m,
                            reconciliationPrice = o.ReconciliationAmount == 0 ? 0.0m : o.ReconciliationAmount,
                            balanceQTY = o.Products != null && o.Products.Count() != 0 ? o.Products.FirstOrDefault().QTY.ToString("N0") : "0"
                        })
                        .OrderBy(po => po.date).ToList() : new List<getPlanOrder>()
                    };

                }
                else
                    throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound); // Throws exception if either user ID or language code is not found
            }
            return resp; // Returning the API response
        }

        // Endpoint to update plan status
        [HttpPost("updatePlanStatus")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> updatePlanStatus(updatePlanStatus_Req req) // Method to handle POST requests for updating plan status
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            if (string.IsNullOrEmpty(req.planID)) // Checking if plan ID is provided
                throw new AmazonFarmerException(_exceptions.planIDRequired); // Throws exception if plan ID is not provided
            else if (req.statusID == 0) // Checking if status ID is provided
                throw new AmazonFarmerException(_exceptions.statusIDRequired); // Throws exception if status ID is not provided
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                if (string.IsNullOrEmpty(userID)) // Checking if user ID is null or empty
                    throw new AmazonFarmerException(_exceptions.userIDNotFound); // Throws exception if user ID is not found
                else
                {
                    var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                    tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), userID, languageCode);
                    TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
                    // Check if the plan exists in the database
                    if (plan == null)
                        throw new AmazonFarmerException(_exceptions.planNotFound);
                    // Check if the user exists in the database
                    if (user == null)
                        throw new AmazonFarmerException(_exceptions.userNotFound);
                    // Check if the user designation is Employee and the plan status is not Active                // Check if the user designation is Farmer and the plan status is not Completed
                    else if (
                            User.IsInRole("Employee") ||
                            (User.IsInRole("Farmer") && (plan.Status != EPlanStatus.Draft))
                        )
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    else if (req.statusID != 1 && req.statusID != 8)
                    {
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    }
                    plan.Status = (EPlanStatus)req.statusID;

                    if (req.statusID == (int)EPlanStatus.Revert)
                        plan.Reason = req.message; // set reverted reason if the request is for revert.

                    await _repoWrapper.PlanRepo.updatePlan(plan); // Updating plan status
                    _repoWrapper.Save();
                    resp.response = "udpated"; // Setting response
                }
            }
            return resp; // Returning the API response
        }

        [Obsolete]
        [HttpPost("getPlanDetails")]
        public async Task<APIResponse> getPlanDetails(getPlanOrder_Req req)
        {
            APIResponse resp = new APIResponse();
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            string designationID = User.FindFirst("designationID")?.Value; // Retrieving designation ID from user claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);

            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID), userID, languageCode);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (User.IsInRole("Farmer") && plan.UserID != userID)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (User.IsInRole("Employee"))
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                else
                {

                    var imageBase = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL");

                    resp.response = new getPlanDetail_Resp()
                    {
                        planID = plan.ID,
                        farmID = plan.FarmID,
                        farm = plan.Farm.FarmName,
                        warehouseID = plan.WarehouseID,
                        warehouse = plan.Warehouse.WarehouseTranslation.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text,
                        seasonID = plan.SeasonID,
                        season = plan.Season.SeasonTranslations.Where(x => x.LanguageCode == languageCode).First().Translation,
                        status = plan.Status,
                        reason = plan.Reason,
                        planner = plan.User.FirstName,
                        farmerComment = plan.FarmerComment ?? string.Empty,
                        crops = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).Select(x => new planCrops_getPlanDetail
                        {
                            planCropID = x.ID,
                            //cropID = x.CropID,
                            cropGroupID = x.CropGroupID,
                            cropsGroup = _repoWrapper.PlanRepo.getCropInformationByCropGroupID(x.CropGroupID, languageCode, imageBase).Result,
                            //cropIDs = x.CropGroup.CropGroupCrops.Select(gc => gc.CropID).ToList(),
                            //imagePath = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Image,
                            //crop = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text,
                            acreage = Convert.ToInt32(x.Acre),
                            hasException = x.PlanCropEndorse == EPlanCropEndorse.Exception ? true : false,
                            //suggestion = x.Crop.ProductConsumptionMetrics
                            //.Select(x => new ConsumptionMatrixDTO
                            //{
                            //    name = x.Product.ProductTranslations
                            //        .Where(x => x.LanguageCode == languageCode)
                            //        .FirstOrDefault().Text,
                            //    qty = x.Usage.ToString(),
                            //    uom = x.UOM
                            //}).ToList(),
                            products = x.PlanProducts.Where(x => x.Status == EActivityStatus.Active).Select(p => new cropProduct_planCrops_getPlanDetail
                            {
                                planProductID = p.ID,
                                productID = p.ProductID,
                                product = p.Product.ProductTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault() != null ? p.Product.ProductTranslations.Where(x => x.LanguageCode == languageCode).First().Text : string.Empty,
                                qty = p.Qty,
                                uom = p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == languageCode).FirstOrDefault() != null ? p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == languageCode).First().Text : string.Empty,
                                date = p.Date//.ToString("yyyy-MM-dd")
                            }).ToList(),
                            services = x.PlanServices.Where(x => x.Status == EActivityStatus.Active).Select(s => new cropService_planCrops_getPlanDetail
                            {
                                planServiceID = s.ID,
                                serviceID = s.ServiceID,
                                service = s.Service.ServiceTranslations.Where(x => x.LanguageCode == languageCode).First().Text,
                                lastHarvestDate = s.LastHarvestDate,//.ToString("yyyy-MM-dd"),
                                landPreparationDate = s.LandPreparationDate,//.ToString("yyyy-MM-dd"),
                                sewingDate = s.SewingDate//.ToString("yyyy-MM-dd")
                            }).ToList()
                        }).ToList(),
                        changeRequestStatus = plan.PlanChangeStatus,
                        isEmptyCropsAllowed = plan!.Orders!.Any() ? true : false
                    };
                }
            }

            return resp;
        }
        [HttpPost("getEditPlan")]
        public async Task<APIResponse> getEditPlan(getPlanOrder_Req req)
        {
            APIResponse resp = new APIResponse();
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            string designationID = User.FindFirst("designationID")?.Value; // Retrieving designation ID from user claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);

            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID), userID, languageCode);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (User.IsInRole("Farmer") && plan.UserID != userID)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (User.IsInRole("Employee"))
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                else
                {

                    plan.PlanCrops = plan.PlanCrops.Where(
                        c => c.PlanProducts.Where(
                            prod => (prod.Qty - prod.DeliveredQty) > 0
                        ).Count() > 0
                    ).ToList();

                    foreach (tblPlanCrops item in plan.PlanCrops)
                    {
                        item.PlanProducts = item.PlanProducts.Where(x => (x.Qty - x.DeliveredQty) > 0).ToList();
                    }
                    getDistance getDistance = new getDistance // Creating getDistance object for distance calculation
                    {
                        farmLatitude = plan.Farm.latitude.Value, // Setting farm latitude
                        farmLongitude = plan.Farm.longitude.Value, // Setting farm longitude
                        WarehouseLocations = new List<LocationDTO>() { new LocationDTO { latitude = plan.Warehouse.latitude, longitude = plan.Warehouse.longitude } } // Initializing warehouse locations list
                    };
                    getDistance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance); // Getting distance between locations using Google location extension

                    var imageBase = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL");

                    resp.response = new getPlanDetail_Resp()
                    {
                        planID = plan.ID,
                        farmID = plan.FarmID,
                        farm = plan.Farm.FarmName,
                        warehouseID = plan.WarehouseID,
                        warehouse = plan.Warehouse.WarehouseTranslation.FirstOrDefault().Text + " - " + getDistance.WarehouseLocations.FirstOrDefault().distanceText,
                        warehouseDistance = getDistance.WarehouseLocations.FirstOrDefault().distanceText,
                        seasonID = plan.SeasonID,
                        season = plan.Season.SeasonTranslations.Where(x => x.LanguageCode == languageCode).First().Translation,
                        status = plan.Status,
                        reason = plan.Reason,
                        planner = plan.User.FirstName,
                        farmerComment = plan.FarmerComment ?? string.Empty,
                        crops = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).Select(x => new planCrops_getPlanDetail
                        {
                            planCropID = x.ID,
                            //cropID = x.CropID,
                            cropGroupID = x.CropGroupID,
                            cropsGroup = _repoWrapper.PlanRepo.getCropInformationByCropGroupID(x.CropGroupID, languageCode, imageBase).Result,
                            //cropIDs = x.CropGroup.CropGroupCrops.Select(gc => gc.CropID).ToList(),
                            //imagePath = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Image,
                            //crop = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text,
                            acreage = Convert.ToInt32(x.Acre),
                            hasException = x.PlanCropEndorse == EPlanCropEndorse.Exception ? true : false,
                            //suggestion = x.CropGroup.CropGroupCrops.Select(cgc => new ConsumptionMatrixDTO
                            //{
                            //    cgc.Crop.ProductConsumptionMetrics
                            //.Select(x => new ConsumptionMatrixDTO
                            //{
                            //    name = x.Product.ProductTranslations
                            //        .Where(x => x.LanguageCode == languageCode)
                            //        .FirstOrDefault().Text,
                            //    qty = x.Usage.ToString(),
                            //    uom = x.UOM
                            //}).FirstOrDefault() }).ToList(),
                            //}).ToList(),
                            products = x.PlanProducts.Where(x => x.Status == EActivityStatus.Active).Select(p => new cropProduct_planCrops_getPlanDetail
                            {
                                planProductID = p.ID,
                                productID = p.ProductID,
                                product = p.Product.ProductTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault() != null ? p.Product.ProductTranslations.Where(x => x.LanguageCode == languageCode).First().Text : string.Empty,
                                qty = p.Qty - p.DeliveredQty,
                                uom = p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == languageCode).FirstOrDefault() != null ? p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == languageCode).First().Text : string.Empty,
                                date = p.Date//.ToString("yyyy-MM-dd")
                            }).ToList(),
                            services = x.PlanServices.Where(x => x.Status == EActivityStatus.Active).Select(s => new cropService_planCrops_getPlanDetail
                            {
                                planServiceID = s.ID,
                                serviceID = s.ServiceID,
                                service = s.Service.ServiceTranslations.Where(x => x.LanguageCode == languageCode).First().Text,
                                lastHarvestDate = s.LastHarvestDate,//.ToString("yyyy-MM-dd"),
                                landPreparationDate = s.LandPreparationDate,//.ToString("yyyy-MM-dd"),
                                sewingDate = s.SewingDate//.ToString("yyyy-MM-dd")
                            }).ToList()
                        }).ToList(),
                        changeRequestStatus = plan.PlanChangeStatus,
                        isEmptyCropsAllowed = plan!.Orders!.Any() ? true : false
                    };
                }
            }

            return resp;
        }
        [HttpPost("getPlanSummary")]
        public async Task<APIResponse> getPlanSummary(getPlanOrder_Req req)
        {
            APIResponse resp = new();

            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), userID, languageCode);

            if (!string.IsNullOrEmpty(languageCode))
            {
                getDistance getDistance = new getDistance // Creating getDistance object for distance calculation
                {
                    farmLatitude = plan.Farm.latitude.Value, // Setting farm latitude
                    farmLongitude = plan.Farm.longitude.Value, // Setting farm longitude
                    WarehouseLocations = new List<LocationDTO>() { new LocationDTO { latitude = plan.Warehouse.latitude, longitude = plan.Warehouse.longitude } } // Initializing warehouse locations list
                };
                getDistance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance); // Getting distance between locations using Google location extension

                if (plan != null)
                {

                    resp.response = new planSummary()
                    {
                        isCropsAvailable = plan!.PlanCrops!.Where(x => x.Status == EActivityStatus.Active).Count() == plan!.PlanCrops!.Count() && plan!.PlanCrops!.Count() > 0 ? true : false,
                        seasonID = plan.SeasonID,
                        season = plan.Season.SeasonTranslations.Where(x => x.LanguageCode == languageCode).First().Translation,
                        months = await _repoWrapper.MonthRepo.getMonthsByLanguageCodeAndSeasonID(languageCode, plan.SeasonID),
                        crops = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).Select(x => new Crop
                        {
                            cropGroupID = x.CropGroupID,
                            //crop = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).First().Text,
                            crops = x.CropGroup.CropGroupCrops.Select(cgc => new planCropGroup_get
                            {
                                cropID = cgc.CropID,
                                cropName = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text,
                                filePath = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Image
                            }).ToList(),
                            acreage = Convert.ToInt32(x.Acre),
                            products = x.PlanProducts.Where(x => x.Status == EActivityStatus.Active).GroupBy(x => x.Date.Month).Select(x => new getMonthWiseProductCount
                            {
                                monthID = x.Key,
                                totalProducts = x.Sum(p => p.Qty)
                            }).ToList()
                        }).ToList(),
                        products = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).SelectMany(x => x.PlanProducts)
                                  .GroupBy(p => new { p.Date.Month, p.ProductID, p.Product.Name })
                                  .Select(g => new Product_DTO
                                  {
                                      productID = g.Key.ProductID,
                                      product = g.Key.Name, // You might need to set the product name here
                                      months = g.Select(p => new Month
                                      {
                                          monthID = p.Date.Month,
                                          productID = p.Product.ID,
                                          product = p.Product.ProductTranslations.Where(x => x.LanguageCode == languageCode).First().Text, // You might need to set the product name here
                                          totalProducts = g.Sum(x => x.Qty),
                                          uom = p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == languageCode).First().Text // You might need to set the unit of measurement here
                                      }).ToList()
                                  }).ToList(),
                        warehouseLocation = plan.Warehouse.WarehouseTranslation.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text + " - " + getDistance.WarehouseLocations.FirstOrDefault().distanceText,
                        warehouseDistance = getDistance.WarehouseLocations.FirstOrDefault().distanceText
                    };
                }
            }
            return resp;
        }
        [AllowAnonymous]
        [HttpGet("downloadPlanSummary/{summaryType}/{planID}")]
        public async Task<dynamic> downloadPlanSummary(string planID, EPlanSummaryType summaryType)
        {
            if (string.IsNullOrEmpty(planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(planID.TrimStart('0')), string.Empty);

            //var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            //string designationID = User.FindFirst("designationID")?.Value; // Retrieving designation ID from user claims
            //if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
            //throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);

            if (string.IsNullOrEmpty(planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                //tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(planID));
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                //else if (User.IsInRole("Farmer") && plan.UserID != userID)
                //throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (User.IsInRole("Employee") && 1 == 2) // add condition to check employee is authorized to get the farm details or not
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else
                {
                    getDistance getDistance = new getDistance // Creating getDistance object for distance calculation
                    {
                        farmLatitude = plan.Farm.latitude.Value, // Setting farm latitude
                        farmLongitude = plan.Farm.longitude.Value, // Setting farm longitude
                        WarehouseLocations = new List<LocationDTO>() { new LocationDTO { latitude = plan.Warehouse.latitude, longitude = plan.Warehouse.longitude } } // Initializing warehouse locations list
                    };
                    getDistance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance); // Getting distance between locations using Google location extension

                    planSummary _planSummary = new planSummary()
                    {
                        isCropsAvailable = plan!.PlanCrops!.Where(x => x.Status == EActivityStatus.Active).Count() == plan!.PlanCrops!.Count() && plan!.PlanCrops!.Count() > 0 ? true : false,
                        seasonID = plan.SeasonID,
                        season = plan.Season.SeasonTranslations.Where(x => x.LanguageCode == "EN").First().Translation,
                        months = await _repoWrapper.MonthRepo.getMonthsByLanguageCodeAndSeasonID("EN", plan.SeasonID),
                        crops = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).Select(x => new Crop
                        {
                            cropGroupID = x.CropGroupID,
                            //cropID = x.CropID,
                            //crop = x.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).First().Text,
                            crops = x.CropGroup.CropGroupCrops.Select(cgc => new planCropGroup_get
                            {
                                cropID = cgc.CropID,
                                cropName = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == "EN").FirstOrDefault().Text,
                                filePath = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == "EN").FirstOrDefault().Image
                            }).ToList(),
                            acreage = Convert.ToInt32(x.Acre),
                            products = x.PlanProducts.Where(x => x.Status == EActivityStatus.Active).GroupBy(x => x.Date.Month).Select(x => new getMonthWiseProductCount
                            {
                                monthID = x.Key,
                                totalProducts = x.Sum(p => p.Qty)
                            }).ToList()
                        }).ToList(),
                        products = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).SelectMany(x => x.PlanProducts)
                                  .GroupBy(p => new { p.Date.Month, p.ProductID, p.Product.Name })
                                  .Select(g => new Product_DTO
                                  {
                                      productID = g.Key.ProductID,
                                      product = g.Key.Name, // You might need to set the product name here
                                      months = g.Select(p => new Month
                                      {
                                          monthID = p.Date.Month,
                                          productID = p.Product.ID,
                                          product = p.Product.ProductTranslations.Where(x => x.LanguageCode == "EN").First().Text, // You might need to set the product name here
                                          totalProducts = g.Sum(x => x.Qty),
                                          uom = p.Product.UOM.UnitOfMeasureTranslation.Where(x => x.LanguageCode == "EN").First().Text // You might need to set the unit of measurement here
                                      }).ToList()
                                  }).ToList(),
                        warehouseLocation = plan.Warehouse.WarehouseTranslation.Where(x => x.LanguageCode == "EN").FirstOrDefault().Text + " - " + getDistance.WarehouseLocations.FirstOrDefault().distanceText,
                        warehouseDistance = getDistance.WarehouseLocations.FirstOrDefault().distanceText
                    };
                    PDFExtension pdfExtension = new PDFExtension(_converter);
                    try
                    {

                        var summaryPath = await pdfExtension.generatePlanSummary(_planSummary, summaryType);

                        var imageOutput = System.IO.File.OpenRead(summaryPath);
                        var file = base.File(imageOutput, "application/pdf", "planSummary");
                        if (file != null && file.FileDownloadName == "planSummary")
                        {
                            file.FileDownloadName = "planSummary.pdf";
                        }
                        return file;
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            throw new Exception("");
        }
        [HttpPost("ChangeRequest")]
        public async Task<JSONResponse> changeRequest(getPlanOrder_Req req)
        {
            JSONResponse resp = new JSONResponse();
            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), userID, languageCode);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else if (plan.Status == EPlanStatus.Completed)
                    throw new AmazonFarmerException(_exceptions.planNotInValidState);
                else
                {
                    plan.PlanChangeStatus = EPlanChangeRequest.Pending;
                    if (plan.Status == EPlanStatus.Approved)
                    {
                        plan.Status = EPlanStatus.TSOProcessing;
                    }
                    await lockOrders(plan.ID);
                    await _repoWrapper.PlanRepo.updatePlan(plan);
                    await _repoWrapper.SaveAsync();
                    resp.message = "Plan change request has been submitted";

                    #region sending change request notification to user!
                    NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.Employee_PlanChangeRequest, "EN");
                    List<TblUser> users = new List<TblUser>();
                    List<NotificationRequest> notifications = new List<NotificationRequest>();
                    if (plan.Status == EPlanStatus.TSOProcessing)
                        users = await _repoWrapper.UserRepo.getTSOsByFarmID(plan.FarmID);
                    else if (plan.Status == EPlanStatus.RSMProcessing)
                        users = await _repoWrapper.UserRepo.getRSMsByFarmID(plan.FarmID);
                    else if (plan.Status == EPlanStatus.NSMProcessing)
                        users = await _repoWrapper.UserRepo.getNSMsByFarmID(plan.FarmID);
                    foreach (var user in users)
                    {
                        if (notificationDTO != null)
                        {
                            if (!string.IsNullOrEmpty(user.Email))
                                notifications.Add(
                                            new NotificationRequest
                                            {
                                                Type = ENotificationType.Email,
                                                Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.Email, Name = user.FirstName } },
                                                Subject = notificationDTO.title,
                                                Message = notificationDTO.body
                                            });
                            if (!string.IsNullOrEmpty(user.PhoneNumber))
                                notifications.Add(
                                        new NotificationRequest
                                        {
                                            Type = ENotificationType.SMS,
                                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.PhoneNumber, Name = user.FirstName } },
                                            Subject = notificationDTO.title,
                                            Message = notificationDTO.smsBody
                                        });
                            if (!string.IsNullOrEmpty(user.DeviceToken))
                                notifications.Add(
                                        new NotificationRequest
                                        {
                                            Type = ENotificationType.FCM,
                                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.DeviceToken, Name = user.FirstName } },
                                            Subject = notificationDTO.title,
                                            Message = notificationDTO.fcmBody
                                        });
                            if (!string.IsNullOrEmpty(user.Id))
                                notifications.Add(
                                        new NotificationRequest
                                        {
                                            Type = ENotificationType.Device,
                                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.Id, Name = user.FirstName } },
                                            Subject = notificationDTO.title,
                                            Message = notificationDTO.deviceBody
                                        });
                            NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                            replacementDTO.UserName = user.FirstName;
                            replacementDTO.PlanID = plan.ID.ToString().PadLeft(10, '0');
                            replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_PlanChangeRequest;
                            await _notificationService.SendNotifications(notifications, replacementDTO);
                        }
                    }
                }
                #endregion
            }
            return resp;
        }

        [HttpDelete("removePlan")]
        public async Task<JSONResponse> removePlan(getPlanOrder_Req req)
        {
            JSONResponse resp = new JSONResponse();
            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), userID, languageCode);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else
                {
                    if (plan.Status != EPlanStatus.Draft)
                        throw new AmazonFarmerException(_exceptions.planNotInValidState);
                    else
                    {
                        plan.Status = EPlanStatus.Removed;
                        await _repoWrapper.PlanRepo.updatePlan(plan);
                        await _repoWrapper.SaveAsync();
                        resp.message = "Plan has been removed";
                    }

                }
            }
            return resp;
        }

        [HttpGet("getPlanModeOfPaymentOptions")]
        public async Task<APIResponse> GetPlanModeOfPaymentOptions()
        {
            var resp = new APIResponse();
            List<EConfigType> types = new List<EConfigType>() { EConfigType.FullPayment, EConfigType.PartialPayment };
            List<tblConfig> Configurations = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(types);
            resp.response = Configurations
                .Select(c => new
                {
                    modeOfPayment = c.Id,
                    modeOfPaymentID = c.Value,
                    modeOfPaymentName = c.Name,
                    modeOfPaymentDesc = c.Description,
                })
                .ToList();
            return resp;
        }

        private async Task lockOrders(int planID)
        {
            List<TblOrders> orders = await _repoWrapper.OrderRepo.getOrdersByPlanID(planID);
            if (orders != null && orders.Count() > 0)
            {
                foreach (TblOrders order in orders)
                {
                    order.isLocked = true;
                    await _repoWrapper.OrderRepo.UpdateOrder(order);
                }
                //await _repoWrapper.SaveAsync();
            }
        }

        //private List<TblOrders> GenerateOrderFromPlan(tblPlan plan)
        //{

        //}
    }
}
