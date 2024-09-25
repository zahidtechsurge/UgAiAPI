using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using ChangeCustomerPayment;
using ChangeSaleOrder;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using SimulatePrice;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/Employee/Plan")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class EmployeePlanController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly GoogleLocationExtension _googleLocationExtension; // Google location extension for distance calculations
        private readonly NotificationService _notificationService;
        private readonly IConfiguration _configruation;
        private WsdlConfig _wsdlConfig;
        public EmployeePlanController(IRepositoryWrapper repoWrapper, NotificationService notificationService,
            GoogleLocationExtension googleLocationExtension, IOptions<WsdlConfig> wsdlConfig, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _googleLocationExtension = googleLocationExtension;
            _wsdlConfig = wsdlConfig.Value;
            _configruation = configuration;
        }

        [HttpPost("getPlanRequests")]
        public async Task<APIResponse> getPlanRequests(Employee_getPlanReq req)
        {
            // Get the user ID from the token
            APIResponse resp = new APIResponse();
            pagination_Resp pagResp = new pagination_Resp();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            if (string.IsNullOrEmpty(userID))
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            }

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims

            IQueryable<tblPlan> plans = await _repoWrapper.PlanRepo.getPlanList();
            List<int> territoryIds = new List<int>();
            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                plans = plans.Where(x => territoryIds.Contains(x.Farm.DistrictID));
                if (req.requestTypeID == 1)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.TSOProcessing && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined));
                }
                else if (req.requestTypeID == 2)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.Approved);
                }
                else if (req.requestTypeID == 3)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.RSMProcessing || x.Status == EPlanStatus.NSMProcessing);
                }
                else if (req.requestTypeID == 4)
                {
                    plans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.TSOProcessing);
                }
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
                plans = plans.Where(x => territoryIds.Contains(x.Farm.DistrictID));
                if (req.requestTypeID == 1)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.RSMProcessing && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined));
                }
                else if (req.requestTypeID == 2)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.Approved);
                }
                else if (req.requestTypeID == 3)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.NSMProcessing);
                }
                else if (req.requestTypeID == 4)
                {
                    plans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.RSMProcessing);
                }
            }
            else if (designationID == (int)EDesignation.National_Sales_Manager)
            {
                if (req.requestTypeID == 1)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.NSMProcessing);
                }
                else if (req.requestTypeID == 2)
                {
                    plans = plans.Where(x => x.Status == EPlanStatus.Approved);
                }
                else if (req.requestTypeID == 4)
                {
                    plans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.NSMProcessing);
                }
            }


            if (!string.IsNullOrEmpty(req.orderBy))
            {
                plans = filterPlan(plans, req.orderBy);
            }

            List<Employee_getPlansDTO> lst = new List<Employee_getPlansDTO>();
            pagResp.totalRecord = plans.Count();
            //add search terms
            pagResp.filteredRecord = plans.Count();
            plans = plans.OrderByDescending(x => x.ID).Skip(req.skip).Take(req.take);

            pagResp.list = await plans.Select(x => new Employee_getPlansDTO
            {
                planID = x.ID,
                viewablePlanID = x.ID.ToString().PadLeft(10, '0'),
                farm = x.Farm.FarmName,
                farmer = string.Concat(x.User.FirstName + " " + x.User.LastName),
                season = x.Season.Name,
                status = x.Status.ToString(),
                statusID = (int)x.Status
            }).ToListAsync();
            resp.response = pagResp;

            return resp;
        }
        private IQueryable<tblPlan> filterPlan(IQueryable<tblPlan> plans, string key)
        {
            string col = key.Split()[0];
            string orderBy = key.Split()[1];
            if (col == "acreage")
            {
                if (orderBy == "asc")
                {
                    plans = plans.OrderBy(p => p.Farm.Acreage);
                }
                else
                {
                    plans = plans.OrderByDescending(p => p.Farm.Acreage);
                }
            }
            else if (col == "season")
            {
                if (orderBy == "asc")
                {
                    plans = plans.OrderBy(p => p.SeasonID);
                }
                else
                {
                    plans = plans.OrderByDescending(p => p.SeasonID);
                }
            }
            else if (col == "createdOn")
            {
                if (orderBy == "asc")
                {
                    plans = plans.OrderBy(p => p.CreatedOn);
                }
                else
                {
                    plans = plans.OrderByDescending(p => p.CreatedOn);
                }
            }
            else if (col == "district")
            {
                if (orderBy == "asc")
                {
                    plans = plans.OrderBy(p => p.Farm.DistrictID);
                }
                else
                {
                    plans = plans.OrderByDescending(p => p.Farm.DistrictID);
                }
            }
            return plans;
        }

        [HttpPost("getPlanDetail")]
        public async Task<APIResponse> getPlanDetail(Employee_getPlanDetailReq req)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
            APIResponse resp = new APIResponse();
            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(req.planID, string.Empty);
            if (plan == null)
            {
                throw new AmazonFarmerException(_exceptions.planNotFound);
            }

            bool canEndorse = true;
            bool canPerformAction = false;
            List<int> territoryIds = new List<int>();
            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                if (territoryIds.Contains(plan.Farm.DistrictID) && plan.Status == EPlanStatus.TSOProcessing)
                {
                    canPerformAction = true;
                }
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
                if (territoryIds.Contains(plan.Farm.DistrictID) && plan.Status == EPlanStatus.RSMProcessing)
                {
                    canPerformAction = true;
                }
            }
            else if (designationID == (int)EDesignation.National_Sales_Manager)
            {
                if (plan.Status == EPlanStatus.NSMProcessing)
                {
                    canPerformAction = true;
                }
                canEndorse = false;
            }




            tblfarm farm = plan.Farm;
            TblUser farmer = plan.User;
            tblFarmerProfile farmerProfile = plan.User.FarmerProfile.FirstOrDefault();

            var reasons = await _repoWrapper.ReasonRepo.getReasonsByLanguageCodeAndReasonOf("EN", EReasonOf.ChangedPlan_Employee);

            Employee_getPlanDetailResp planDetail = new Employee_getPlanDetailResp()
            {
                isSummaryAvailable = plan!.PlanCrops!.Where(y => y.Status == EActivityStatus.DeActive).Count() == plan!.PlanCrops!.Count() && plan!.PlanCrops!.Count() > 0 ? false : true,
                planID = plan.ID,
                season = plan.Season.Name,
                pickupLocation = plan.Warehouse.Name,
                crop = plan.PlanCrops.Select(x => new planCrops_getPlanDetail
                {
                    planCropID = x.ID,
                    cropGroupID = x.CropGroupID,
                    cropsGroup = _repoWrapper.PlanRepo.getCropInformationByCropGroupID(x.CropGroupID, "EN", ConfigExntension.GetConfigurationValue("Locations:AdminBaseURL")).Result,
                    acreage = Convert.ToInt32(x.Acre),
                    hasException = x.PlanCropEndorse == EPlanCropEndorse.Exception ? true : false,
                    products = x.PlanProducts.Select(p => new cropProduct_planCrops_getPlanDetail
                    {
                        planProductID = p.ID,
                        productID = p.ProductID,
                        product = p.Product.Name,
                        qty = p.Qty,
                        uom = p.Product.UOM.UOM,
                        date = p.Date//.ToString("yyyy-MM-dd")
                    }).ToList(),
                    services = x.PlanServices.Select(s => new cropService_planCrops_getPlanDetail
                    {
                        serviceID = s.ID,
                        service = s.Service.Name,
                        lastHarvestDate = s.LastHarvestDate,//.ToString("yyyy-MM-dd"),
                        landPreparationDate = s.LandPreparationDate,//.ToString("yyyy-MM-dd"),
                        sewingDate = s.SewingDate//.ToString("yyyy-MM-dd")
                    }).ToList(),
                    totalProducts = x.PlanProducts.Sum(x => x.Qty),
                }).ToList(),
                farm = new Employee_getPlanDetail_Farm
                {
                    name = farm == null ? string.Empty : farm.FarmName,
                    address1 = farm == null ? string.Empty : farm.Address1,
                    address2 = farm == null ? string.Empty : farm.Address2,
                    city = farm == null ? string.Empty : farm.City.Name,
                    district = farm == null ? string.Empty : farm.District.Name,
                    tehsil = farm == null ? string.Empty : farm.Tehsil.Name
                },
                farmer = new farmerProfileDTO
                {
                    firstName = farmer == null ? string.Empty : farmer.FirstName,
                    LastName = farmer == null ? string.Empty : farmer.LastName,
                    phone = farmer == null ? string.Empty : farmer.PhoneNumber,
                    email = farmer == null ? string.Empty : farmer.Email,
                    cnicNumber = farmerProfile == null ? string.Empty : farmerProfile.CNICNumber,
                    dateOfBirth = farmerProfile == null ? string.Empty : farmerProfile.DateOfBirth,
                    fatherName = farmerProfile == null ? string.Empty : farmerProfile.FatherName,
                    ntnNumber = farmerProfile == null ? string.Empty : farmerProfile.NTNNumber,
                    strnNumber = farmerProfile == null ? string.Empty : farmerProfile.STRNNumber
                },
                reason = string.IsNullOrEmpty(plan.Reason) ? string.Empty : plan.Reason,
                canEndorse = canEndorse,
                canPerformAction = canPerformAction,
                isPaid = plan.Orders.Any(x => x.PaymentStatus == EOrderPaymentStatus.Paid || x.PaymentStatus == EOrderPaymentStatus.Acknowledged || x.PaymentStatus == EOrderPaymentStatus.LedgerUpdate || x.PaymentStatus == EOrderPaymentStatus.PaymentProcessing),
                farmerComment = plan.FarmerComment ?? string.Empty,
                planApprovalRejectionType = Enum.GetValues(typeof(EPlanApprovalRejectionType))
                    .Cast<EPlanApprovalRejectionType>()
                    .Select(e => new DropDownValues
                    {
                        labelFor = e,
                        value = (int)e,
                        label = ConfigExntension.GetEnumDescription(e)
                    })
                .ToList()
            };

            resp.response = planDetail;

            return resp;
        }
        [HttpPost("updatePlanStatus")]
        public async Task<JSONResponse> updatePlanStatus(updatePlanStatusReq req)
        {
            JSONResponse resp = new JSONResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            if (string.IsNullOrEmpty(userID))
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            }

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            if (req.statusID == (int)EPlanUpdateStatus.Endorse && string.IsNullOrEmpty(req.reason)) // message is required in case of plan endorsemenet only
                throw new AmazonFarmerException(_exceptions.reasonNotFound);

            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims 
            List<NotificationRequest> notifications = new();
            List<int> territoryIds = new List<int>();
            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
            }

            bool hasSImplePlanApproval = false;
            if (req.statusID == (int)EPlanUpdateStatus.Approve)
            {
                if (req.planApprovalRejectionType == 0)
                {
                    hasSImplePlanApproval = true;
                    req.planApprovalRejectionType = EPlanApprovalRejectionType.NoImpactAdvance; //Kamran
                    //throw new AmazonFarmerException(_exceptions.planEPlanApprovalRejectionTypeNotFound);
                }

                req.statusID = (int)EPlanStatus.Approved;
            }
            else if (req.statusID == (int)EPlanUpdateStatus.SendBack)
            {
                req.statusID = (int)EPlanStatus.Revert;
            }
            else if (req.statusID == (int)EPlanUpdateStatus.Reject)
            {
                //if (req.planApprovalRejectionType == 0)
                //{
                //    throw new AmazonFarmerException(_exceptions.noImpactOnRejectionError);
                //}
                req.statusID = (int)EPlanStatus.Rejected;
            }

            if (designationID == (int)EDesignation.Territory_Sales_Officer && req.statusID == (int)EPlanUpdateStatus.Endorse)
            {
                req.statusID = (int)EPlanStatus.RSMProcessing;
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager && req.statusID == (int)EPlanUpdateStatus.Endorse)
            {
                req.statusID = (int)EPlanStatus.NSMProcessing;
            }
            else if (designationID == (int)EDesignation.National_Sales_Manager && req.statusID == (int)EPlanUpdateStatus.Endorse)
            {
                throw new AmazonFarmerException(_exceptions.invalidMethod);
            }


            #region Approving work when approved option is selected
            if (req.statusID == (int)EPlanStatus.Approved)
            {
                List<tblCrop> cropsConsumption = await _repoWrapper.CropRepo.getCropsProductConsumptionMetrics();

                tblPlan wholePlan = await _repoWrapper.PlanRepo.getPlanByPlanIDForApproval(req.planID, territoryIds);

                tblPlan plan = wholePlan;
                plan.PlanCrops = wholePlan.PlanCrops.Where(wp => wp.Status == EActivityStatus.Active).ToList();
                if (plan.PlanCrops.Count() == 0 && req.planApprovalRejectionType == EPlanApprovalRejectionType.NoImpactAdvance)
                {
                    throw new AmazonFarmerException(_exceptions.noImpactOnApprovalError);
                }

                List<TblOrders> planOrders = await _repoWrapper.PlanRepo.getOrdersForPlanForApproval(req.planID);
                bool endorseDueToConsumptionMetrics = false;
                decimal tsoMargin = Convert.ToDecimal(_configruation["loamSettings:TSOMargin"]);
                decimal rsmMargin = Convert.ToDecimal(_configruation["loamSettings:RSMMargin"]);
                //Checking for consumption metrics checks
                foreach (TblOrders planOrder in planOrders)
                {
                    TblOrderProducts planOrderProduct = planOrder.Products.FirstOrDefault();

                    foreach (var planCrop in planOrder.Plan.PlanCrops)
                    {
                        foreach (var item in planCrop.CropGroup.CropGroupCrops)
                        {
                            var cropConsumption = cropsConsumption.Where(cc => cc.ID == item.CropID).FirstOrDefault();
                            if (cropConsumption != null)
                            {
                                tblProductConsumptionMetrics metrics = cropConsumption.ProductConsumptionMetrics
                                      .Where(pcm => pcm.ProductID == planOrderProduct.ProductID).FirstOrDefault();

                                if (metrics != null)
                                {
                                    var calculatedValue = Convert.ToDecimal(planCrop.Acre) * metrics.Usage;
                                    if (designationID == (int)EDesignation.Territory_Sales_Officer)
                                    {
                                        req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                        // Calculate TSO Margin of the reference value
                                        decimal tenPercent = calculatedValue * tsoMargin;
                                        if ((planOrderProduct.QTY > calculatedValue + tenPercent) || (planOrderProduct.QTY < calculatedValue - tenPercent))
                                        {
                                            req.statusID = (int)EPlanStatus.RSMProcessing;
                                            req.reason = "Consumption metric issue";
                                            await EndorseWork(req, territoryIds);
                                            endorseDueToConsumptionMetrics = true;
                                        }
                                    }
                                    else if (designationID == (int)EDesignation.Regional_Sales_Manager)
                                    {

                                        req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                        // Calculate RSM Margin of the reference value
                                        decimal twentyPercent = calculatedValue * rsmMargin;
                                        if ((planOrderProduct.QTY > calculatedValue + twentyPercent) || (planOrderProduct.QTY < calculatedValue - twentyPercent))
                                        {
                                            req.statusID = (int)EPlanStatus.NSMProcessing;
                                            req.reason = "Consumption metric issue";
                                            await EndorseWork(req, territoryIds);
                                            endorseDueToConsumptionMetrics = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (designationID == (int)EDesignation.Territory_Sales_Officer)
                                    {

                                        req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                        req.statusID = (int)EPlanStatus.RSMProcessing;
                                        req.reason = "Consumption metric issue";
                                        await EndorseWork(req, territoryIds);
                                        endorseDueToConsumptionMetrics = true;
                                    }
                                    else if (designationID == (int)EDesignation.Regional_Sales_Manager)
                                    {

                                        req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                        req.statusID = (int)EPlanStatus.NSMProcessing;
                                        req.reason = "Consumption metric issue";
                                        await EndorseWork(req, territoryIds);
                                        endorseDueToConsumptionMetrics = true;
                                    }
                                }
                            }
                            else
                            {
                                if (designationID == (int)EDesignation.Territory_Sales_Officer)
                                {

                                    req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                    req.statusID = (int)EPlanStatus.RSMProcessing;
                                    req.reason = "Consumption metric issue";
                                    await EndorseWork(req, territoryIds);
                                    endorseDueToConsumptionMetrics = true;
                                }
                                else if (designationID == (int)EDesignation.Regional_Sales_Manager)
                                {

                                    req.planCrops.Where(pc => pc.planCropID == planCrop.ID).FirstOrDefault().hasException = true;
                                    req.statusID = (int)EPlanStatus.NSMProcessing;
                                    req.reason = "Consumption metric issue";
                                    await EndorseWork(req, territoryIds);
                                    endorseDueToConsumptionMetrics = true;
                                }
                            }
                        }
                    }
                }
                if (!endorseDueToConsumptionMetrics)
                {
                    if (plan == null)
                        throw new AmazonFarmerException(_exceptions.planNotFound);

                    bool isRejectionNotAllowed = planOrders.Where(ao => ao.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
                         || ao.PaymentStatus == EOrderPaymentStatus.LedgerUpdate
                         || ao.PaymentStatus == EOrderPaymentStatus.Acknowledged
                         ).Any();

                    if (isRejectionNotAllowed)
                    {
                        throw new AmazonFarmerException(_exceptions.rejectionNotAllowed);
                    }

                    if (plan.PlanChangeStatus == EPlanChangeRequest.Pending || plan.PlanChangeStatus == EPlanChangeRequest.Accept)
                        throw new AmazonFarmerException(_exceptions.isPlanEditable);

                    if (plan.Status == EPlanStatus.TSOProcessing || plan.Status == EPlanStatus.RSMProcessing || plan.Status == EPlanStatus.NSMProcessing)
                    {
                        if (req.planCrops.Where(pc => pc.hasException).Any())
                        {
                            throw new AmazonFarmerException(_exceptions.invalidMethod);
                        }

                        string advancePercentValue = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentPercent);
                        int percentageValue = Convert.ToInt32(advancePercentValue);

                        //Getting Total Price List for Plan
                        List<PlanCropProductPrice> planCropProductPrices = await GetPlanPrice(plan, percentageValue);

                        //Sum of all plan Prices
                        decimal newPlanTotalPrice = planCropProductPrices.Sum(pp => pp.TotalAmount);
                        decimal AdvancePaymentAmount = (newPlanTotalPrice * percentageValue) / 100;


                        if (planOrders == null || planOrders.Count() == 0)
                        {
                            await ApproveNewPlan(plan, planCropProductPrices, AdvancePaymentAmount, EOrderType.Advance);
                        }
                        else
                        {
                            await ApprovePlan(plan, planCropProductPrices, planOrders);


                            bool alreadyPaidPlan = planOrders.Where(po =>
                                  po.PaymentStatus != EOrderPaymentStatus.Paid
                                  || po.PaymentStatus != EOrderPaymentStatus.Acknowledged
                                  || po.PaymentStatus != EOrderPaymentStatus.LedgerUpdate
                                  || po.PaymentStatus != EOrderPaymentStatus.PaymentProcessing
                                  ).Any();


                            if (alreadyPaidPlan)
                            {
                                List<TblOrders> allAdvancePaymentOrders = planOrders
                                    .Where(o => o.OrderType == EOrderType.Advance || o.OrderType == EOrderType.AdvancePaymentReconcile)
                                    .ToList();

                                List<TblOrders> allOrderReconcileOrders = planOrders.Where(o => o.OrderType == EOrderType.OrderReconcile)
                                    .ToList();

                                bool anyUnPaidPaymentRemaining = planOrders.Where(o => o.PaymentStatus == EOrderPaymentStatus.NonPaid)
                                    .Any();

                                if (anyUnPaidPaymentRemaining && req.planApprovalRejectionType == EPlanApprovalRejectionType.RefundAdvance)
                                {
                                    throw new AmazonFarmerException(_exceptions.orderApprovalFailure);
                                }

                                if (req.planApprovalRejectionType == EPlanApprovalRejectionType.ForfeitAdvance)
                                {
                                    foreach (TblOrders advanceOrder in allAdvancePaymentOrders)
                                    {
                                        if (advanceOrder.PaymentStatus == EOrderPaymentStatus.Paid)
                                        {
                                            await ChangeCustomerPaymentWSDL(advanceOrder, refundText: "Z042", reasonCode: "FA");
                                            advanceOrder.PaymentStatus = EOrderPaymentStatus.Forfeit;
                                            advanceOrder.IsConsumed = true;
                                            await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                                            _repoWrapper.OrderRepo.AddOrderLog(advanceOrder, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                                        }
                                    }
                                }
                                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.RefundAdvance)
                                {
                                    foreach (TblOrders advanceOrder in allAdvancePaymentOrders)
                                    {
                                        if (advanceOrder.PaymentStatus == EOrderPaymentStatus.Paid)
                                        {

                                            await ChangeCustomerPaymentWSDL(advanceOrder, refundText: "Z045", reasonCode: "");
                                            advanceOrder.PaymentStatus = EOrderPaymentStatus.Refund;
                                            advanceOrder.IsConsumed = true;
                                            await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                                            _repoWrapper.OrderRepo.AddOrderLog(advanceOrder, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                                        }
                                    }
                                }
                            }
                        }

                        await UpdateOrderServices(plan);
                    }

                    plan.Status = (EPlanStatus)req.statusID;
                    plan.Reason = req.reason;
                    //update plan
                    await _repoWrapper.PlanRepo.updatePlan(plan);
                    await _repoWrapper.SaveAsync();

                    //Unlocking Orders when approving the plan
                    await unLockOrders(plan.ID);
                }


            }
            #endregion

            #region Rejection work
            else if (req.statusID == (int)EPlanStatus.Rejected)
            {
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanOrdersForRejectectionByPlanID(req.planID, territoryIds);

                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                if ((plan.Orders != null && plan.Orders.Count() > 0) && plan.Orders.Where(x => x.OrderType == EOrderType.Advance).FirstOrDefault().PaymentStatus != EOrderPaymentStatus.NonPaid)
                {
                    if (req.planApprovalRejectionType == 0)
                    {
                        throw new AmazonFarmerException(_exceptions.noImpactOnRejectionError);
                    }
                }

                if (plan.PlanChangeStatus == EPlanChangeRequest.Pending || plan.PlanChangeStatus == EPlanChangeRequest.Accept)
                    throw new AmazonFarmerException(_exceptions.isPlanEditable);

                plan.Reason = req.reason;

                await RejectPlan(plan, req.planApprovalRejectionType);

                //Unlocking Orders when rejected the plan
                await unLockOrders(plan.ID);
            }
            #endregion


            #region Endorse work
            else if (req.statusID == (int)EPlanStatus.RSMProcessing || req.statusID == (int)EPlanStatus.NSMProcessing)
            {
                await EndorseWork(req, territoryIds);
            }
            #endregion

            #region Send Back
            else if (req.statusID == (int)EPlanStatus.Revert)
            {
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanIDForApproval(req.planID, territoryIds);
                #region updating plan when exception is raised Endorse Crops
                plan.Status = EPlanStatus.Revert;
                plan.Reason = req.reason;
                await _repoWrapper.PlanRepo.updatePlan(plan);
                await _repoWrapper.SaveAsync();

                //Unlocking Orders when sent back
                //await unLockOrders(plan.ID);
                #endregion
            }
            #endregion

            // Create notifications for farmer
            NotificationDTO notificationDTO = new NotificationDTO();
            NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
            NotificationDTO employeeNotificationDTO = new NotificationDTO();
            // get farmer by planID
            TblUser farmer = await _repoWrapper.UserRepo.getUserByPlanID(req.planID);
            List<TblUser> nextApprover = new();

            //selecting the plan change notification message
            if (req.statusID == (int)EPlanStatus.Approved)
            {
                if (hasSImplePlanApproval)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planApproved;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planApproved, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.NoImpactAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestApprovedandNoImpact;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestApprovedandNoImpact, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.ForfeitAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestApprovedandForfieted;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestApprovedandForfieted, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.RefundAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestApprovedandRefundPayment;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestApprovedandRefundPayment, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
            }
            else if (farmer != null && (req.statusID == (int)EPlanStatus.RSMProcessing || req.statusID == (int)EPlanStatus.NSMProcessing))
            {
                if (req.statusID == (int)EPlanStatus.NSMProcessing)
                {
                    nextApprover = await _repoWrapper.UserRepo.getNSMsByFarmID(farmer.farms.First().FarmID);
                }
                else if (req.statusID == (int)EPlanStatus.RSMProcessing)
                {
                    nextApprover = await _repoWrapper.UserRepo.getRSMsByFarmID(farmer.farms.First().FarmID);
                }
                replacementDTO.NotificationBodyTypeID = ENotificationBody.PlanEndorsed_Farmer;
                notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.PlanEndorsed_Farmer, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                employeeNotificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.PlanEndorsed_Employee, "EN");
            }
            else if (req.statusID == (int)EPlanStatus.Rejected)
            {
                if (req.planApprovalRejectionType == EPlanApprovalRejectionType.NoImpactAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestRejected;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestRejected, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.ForfeitAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestRejectedandForfieted;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestRejectedandForfieted, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else if (req.planApprovalRejectionType == EPlanApprovalRejectionType.RefundAdvance)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestRejectedandRefunded;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestRejectedandRefunded, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
                else
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.planRejectionByEmployee;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planRejectionByEmployee, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                }
            }
            else if (req.statusID == (int)EPlanStatus.Revert)
            {
                replacementDTO.NotificationBodyTypeID = ENotificationBody.planSendBackByEmployee;
                notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planSendBackByEmployee, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
            }
            if (notificationDTO != null && !string.IsNullOrEmpty(notificationDTO.body))
            {
                if (farmer != null && nextApprover.Count() == 0)
                {

                    var farmerEmail = new NotificationRequest
                    {
                        Type = ENotificationType.Email,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Email, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.body
                    };
                    notifications.Add(farmerEmail);

                    tblDeviceNotifications? deviceNotification = null;// await _repoWrapper.NotificationRepo.getDeviceNotificationByType(EDeviceNotificationType.Farmer_PlanStatusUpdated);
                    if (deviceNotification != null)
                    {
                        tblNotification newNotification = new tblNotification()
                        {
                            ClickedOn = DateTime.UtcNow,
                            CreatedOn = DateTime.UtcNow,
                            DeviceNotificationID = deviceNotification.NotificationID,
                            FarmID = null,
                            PlanID = req.planID,
                            OrderID = null,
                            IsClicked = false,
                            UserID = farmer.Id,
                            NotificationRequestStatus = GetNotificationRequestStatus((EPlanStatus)req.statusID)
                        };
                        _repoWrapper.NotificationRepo.addDeviceNotification(newNotification);
                        await _repoWrapper.SaveAsync();
                    }
                    var farmerFCM = new NotificationRequest
                    {
                        Type = ENotificationType.FCM,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.DeviceToken, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.fcmBody
                        //.Replace("<br/>", Environment.NewLine)
                        //.Replace("[Farms]", farmer.farms.FirstOrDefault().FarmName)
                        //.Replace("[Reasons Dropdown Option]", ConfigExntension.GetEnumDescription((EPlanApprovalRejectionType)req.planApprovalRejectionType))
                        //.Replace("[Reason Comment Box]", req.reason)
                        //.Replace("[Send to name]", "")
                    };
                    notifications.Add(farmerFCM);
                    var farmerSMS = new NotificationRequest
                    {
                        Type = ENotificationType.SMS,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.PhoneNumber, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.smsBody
                        //.Replace("<br/>", Environment.NewLine)
                        //.Replace("[Farms]", farmer.farms.FirstOrDefault().FarmName)
                        //.Replace("[Reasons Dropdown Option]", ConfigExntension.GetEnumDescription((EPlanApprovalRejectionType)req.planApprovalRejectionType))
                        //.Replace("[Reason Comment Box]", req.reason)
                        //.Replace("[Send to name]", "")
                    };
                    notifications.Add(farmerSMS);
                    var farmerDevice = new NotificationRequest
                    {
                        Type = ENotificationType.Device,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Id, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.deviceBody
                        //.Replace("<br/>", Environment.NewLine)
                        //.Replace("[Farms]", farmer.farms.FirstOrDefault().FarmName)
                        //.Replace("[Reasons Dropdown Option]", ConfigExntension.GetEnumDescription((EPlanApprovalRejectionType)req.planApprovalRejectionType))
                        //.Replace("[Reason Comment Box]", req.reason)
                        //.Replace("[Send to name]", "")
                    };
                    notifications.Add(farmerDevice);
                }
                if (nextApprover.Count() > 0 && employeeNotificationDTO != null)
                {
                    foreach (var emp in nextApprover)
                    {
                        var empEmailNotification = new NotificationRequest
                        {
                            Type = ENotificationType.Email,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = emp?.Email, Name = emp?.FirstName } },
                            Subject = employeeNotificationDTO.title,
                            Message = employeeNotificationDTO.body.Replace("[firstName]", emp.FirstName)
                        };
                        notifications.Add(empEmailNotification);
                        var empFCMNotification = new NotificationRequest
                        {
                            Type = ENotificationType.FCM,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = emp?.DeviceToken, Name = emp?.FirstName } },
                            Subject = employeeNotificationDTO.title,
                            Message = employeeNotificationDTO.fcmBody.Replace("[firstName]", emp.FirstName)
                        };
                        notifications.Add(empFCMNotification);
                        var empSMSNotification = new NotificationRequest
                        {
                            Type = ENotificationType.SMS,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = emp?.PhoneNumber, Name = emp?.FirstName } },
                            Subject = employeeNotificationDTO.title,
                            Message = employeeNotificationDTO.smsBody.Replace("[firstName]", emp.FirstName)
                        };
                        notifications.Add(empSMSNotification);
                        var empDeviceNotification = new NotificationRequest
                        {
                            Type = ENotificationType.Device,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = emp?.Id, Name = emp?.FirstName } },
                            Subject = employeeNotificationDTO.title,
                            Message = employeeNotificationDTO.deviceBody.Replace("[firstName]", emp.FirstName)
                        };
                        notifications.Add(empDeviceNotification);
                    }
                }

                replacementDTO.FarmName = farmer.farms.FirstOrDefault().FarmName;
                replacementDTO.UserName = "TSO";
                replacementDTO.Status = ConfigExntension.GetEnumDescription((EPlanStatus)req.statusID);
                replacementDTO.ReasonDropDownOptionId = ((int)req.planApprovalRejectionType).ToString();
                replacementDTO.ReasonDropDownOption = req.planApprovalRejectionType != null && req.planApprovalRejectionType != 0 ? ConfigExntension.GetEnumDescription((EPlanApprovalRejectionType)req.planApprovalRejectionType) : null;
                replacementDTO.ReasonComment = req.reason;
                replacementDTO.PlanID = req.planID.ToString().PadLeft(10, '0');

                await _notificationService.SendNotifications(notifications, replacementDTO);

            }
            resp.message = "Status has been updated";
            return resp;
        }
        private async Task EndorseWork(updatePlanStatusReq req, List<int> territoryIds)
        {
            tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanIDForApproval(req.planID, territoryIds);
            #region updating plan when exception is raised Endorse Crops
            if (req.planCrops.Count() > 0)
            {
                plan.Status = (EPlanStatus)req.statusID;
                plan.Reason = req.reason;
                await _repoWrapper.PlanRepo.updatePlan(plan);

                foreach (PlanCropsReq item in req.planCrops)
                {
                    tblPlanCrops? planCrop = plan.PlanCrops?.Where(x => x.ID == item.planCropID).FirstOrDefault();
                    if (planCrop != null)
                    {
                        planCrop.PlanCropEndorse = item.hasException ? EPlanCropEndorse.Exception : EPlanCropEndorse.Ok;
                        await _repoWrapper.PlanRepo.updatePlanCrop(planCrop);
                    }
                }
                await _repoWrapper.SaveAsync();
            }
            #endregion
        }
        private EDeviceNotificationRequestStatus GetNotificationRequestStatus(EPlanStatus ePlanStatus)
        {
            switch (ePlanStatus)
            {
                case EPlanStatus.TSOProcessing:
                    return EDeviceNotificationRequestStatus.Plan_TSOProcessing;
                case EPlanStatus.RSMProcessing:
                    return EDeviceNotificationRequestStatus.Plan_RSMProcessing;
                case EPlanStatus.NSMProcessing:
                    return EDeviceNotificationRequestStatus.Plan_NSMProcessing;
                case EPlanStatus.Approved:
                    return EDeviceNotificationRequestStatus.Plan_Approved;
                case EPlanStatus.Declined:
                    return EDeviceNotificationRequestStatus.Plan_Declined;
                case EPlanStatus.Completed:
                    return EDeviceNotificationRequestStatus.Plan_Completed;
                case EPlanStatus.Revert:
                    return EDeviceNotificationRequestStatus.Plan_Revert;
                case EPlanStatus.Removed:
                    return EDeviceNotificationRequestStatus.Plan_Removed;
                case EPlanStatus.Draft:
                    return EDeviceNotificationRequestStatus.Plan_Draft;
                case EPlanStatus.Rejected:
                    return EDeviceNotificationRequestStatus.Plan_Rejected;
                default:
                    return EDeviceNotificationRequestStatus.Plan_Draft;
            }
        }
        private EDeviceNotificationRequestStatus GetNotificationRequestStatus(EPlanChangeRequest ePlanChangeStatus)
        {

            switch (ePlanChangeStatus)
            {
                case EPlanChangeRequest.Default:
                    return EDeviceNotificationRequestStatus.PlanChange_Default;
                case EPlanChangeRequest.Pending:
                    return EDeviceNotificationRequestStatus.PlanChange_Pending;
                case EPlanChangeRequest.Accept:
                    return EDeviceNotificationRequestStatus.PlanChange_Accept;
                case EPlanChangeRequest.Declined:
                    return EDeviceNotificationRequestStatus.PlanChange_Declined;
                default:
                    return EDeviceNotificationRequestStatus.PlanChange_Default;
            }
        }

        [HttpPost("ChangeRequest")]
        public async Task<JSONResponse> ChangeRequest(Employee_planChangeReq req)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            if (string.IsNullOrEmpty(userID))
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            }

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);
            JSONResponse resp = new JSONResponse();
            if (req.planID == 0)
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(req.planID, string.Empty);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                else
                {
                    if ((EPlanChangeRequest)req.statusID == EPlanChangeRequest.Accept)
                    {
                        plan.Status = EPlanStatus.Draft;
                    }
                    plan.PlanChangeStatus = (EPlanChangeRequest)req.statusID;
                    await _repoWrapper.PlanRepo.updatePlan(plan);
                    await _repoWrapper.SaveAsync();
                    resp.message = "Plan change request has been submitted";
                    NotificationDTO notificationDTO = new NotificationDTO();
                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();

                    if ((EPlanChangeRequest)req.statusID == EPlanChangeRequest.Declined)
                    {
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestRejected;
                        notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestRejected, plan.User.FarmerProfile.FirstOrDefault().SelectedLangCode);
                    }
                    else if ((EPlanChangeRequest)req.statusID == EPlanChangeRequest.Accept)
                    {
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.planChangeRequestApproved;
                        notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.planChangeRequestApproved, plan.User.FarmerProfile.FirstOrDefault().SelectedLangCode);
                    }

                    List<NotificationRequest> notifications = new();
                    if (notificationDTO != null)
                    {

                        var farmerEmail = new NotificationRequest
                        {
                            Type = ENotificationType.Email,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = plan.User?.Email, Name = plan.User?.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.body
                            //.Replace("<br/>", Environment.NewLine)
                            //.Replace("[Reasons Dropdown Option]", "")
                            //.Replace("[Reason Comment Box]", "")
                        };
                        notifications.Add(farmerEmail);
                        var farmerFCM = new NotificationRequest
                        {
                            Type = ENotificationType.FCM,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = plan.User?.DeviceToken, Name = plan.User?.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.fcmBody
                            //.Replace("<br/>", Environment.NewLine)
                            //.Replace("[Reasons Dropdown Option]", "")
                            //.Replace("[Reason Comment Box]", "")
                        };
                        notifications.Add(farmerFCM);
                        var farmerSMS = new NotificationRequest
                        {
                            Type = ENotificationType.SMS,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = plan.User?.PhoneNumber, Name = plan.User?.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.smsBody
                            //.Replace("<br/>", Environment.NewLine)
                            //.Replace("[Reasons Dropdown Option]", "")
                            //.Replace("[Reason Comment Box]", "")
                        };
                        notifications.Add(farmerSMS);
                        var farmerDevice = new NotificationRequest
                        {
                            Type = ENotificationType.Device,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = plan.User?.Id, Name = plan.User?.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.deviceBody
                            //.Replace("[Reasons Dropdown Option]", "")
                            //.Replace("[Reason Comment Box]", "")
                        };
                        notifications.Add(farmerDevice);
                    }
                    replacementDTO.UserName = plan.User?.FirstName;

                    replacementDTO.PlanID = req.planID.ToString().PadLeft(10, '0');
                    replacementDTO.ReasonDropDownOption = "";
                    replacementDTO.ReasonDropDownOptionId = "";
                    replacementDTO.ReasonComment = "";
                    await _notificationService.SendNotifications(notifications, replacementDTO);

                    tblDeviceNotifications? deviceNotification = null;// await _repoWrapper.NotificationRepo.getDeviceNotificationByType(EDeviceNotificationType.Farmer_PlanStatusUpdated);
                    if (deviceNotification != null)
                    {
                        tblNotification newNotification = new tblNotification()
                        {
                            ClickedOn = DateTime.UtcNow,
                            CreatedOn = DateTime.UtcNow,
                            DeviceNotificationID = deviceNotification.NotificationID,
                            FarmID = null,
                            PlanID = req.planID,
                            OrderID = null,
                            IsClicked = false,
                            UserID = plan.UserID,
                            NotificationRequestStatus = GetNotificationRequestStatus((EPlanChangeRequest)req.statusID)
                        };
                        _repoWrapper.NotificationRepo.addDeviceNotification(newNotification);
                        await _repoWrapper.SaveAsync();
                    }
                }
            }
            return resp;
        }

        [HttpPost("getPlanSummary")]
        public async Task<APIResponse> getPlanSummary(getPlanOrder_Req req)
        {
            APIResponse resp = new();

            #region Old Plan Summary Logic
            //if (string.IsNullOrEmpty(req.planID))
            //    throw new AmazonFarmerException(_exceptions.planIDRequired);

            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            //if (string.IsNullOrEmpty(userID))
            //{
            //    throw new AmazonFarmerException(_exceptions.userIDNotFound);
            //}

            //if (!User.IsInRole("Employee"))
            //    throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            //tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), "EN");

            //if (plan != null)
            //{
            //    getDistance getDistance = new getDistance // Creating getDistance object for distance calculation
            //    {
            //        farmLatitude = plan.Farm.latitude.Value, // Setting farm latitude
            //        farmLongitude = plan.Farm.longitude.Value, // Setting farm longitude
            //        WarehouseLocations = new List<LocationDTO>() { new LocationDTO { latitude = plan.Warehouse.latitude, longitude = plan.Warehouse.longitude } } // Initializing warehouse locations list
            //    };
            //    getDistance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance); // Getting distance between locations using Google location extension


            //    resp.response = new planSummary()
            //    {
            //        isCropsAvailable = plan!.PlanCrops!.Where(x => x.Status == EActivityStatus.Active).Count() == plan!.PlanCrops!.Count() && plan!.PlanCrops!.Count() > 0 ? true : false,
            //        seasonID = plan.SeasonID,
            //        season = plan.Season.SeasonTranslations.First().Translation,
            //        months = await _repoWrapper.MonthRepo.getMonthsByLanguageCodeAndSeasonID("EN", plan.SeasonID),
            //        crops = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).Select(x => new Crop
            //        {
            //            cropGroupID = x.CropGroupID,
            //            //crop = x.Crop.CropTranslations.First().Text,
            //            crops = x.CropGroup.CropGroupCrops.Select(cgc => new planCropGroup_get
            //            {
            //                cropID = cgc.CropID,
            //                cropName = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == "EN").FirstOrDefault().Text,
            //                filePath = cgc.Crop.CropTranslations.Where(x => x.LanguageCode == "EN").FirstOrDefault().Image
            //            }).ToList(),
            //            acreage = Convert.ToInt32(x.Acre),
            //            products = x.PlanProducts.Where(x => x.Status == EActivityStatus.Active).GroupBy(x => x.Date.Month).Select(x => new getMonthWiseProductCount
            //            {
            //                monthID = x.Key,
            //                totalProducts = x.Sum(p => p.Qty)
            //            }).ToList()
            //        }).ToList(),
            //        products = plan.PlanCrops.Where(x => x.Status == EActivityStatus.Active).SelectMany(x => x.PlanProducts)
            //                  .GroupBy(p => new { p.Date.Month, p.ProductID, p.Product.Name })
            //                  .Select(g => new Product_DTO
            //                  {
            //                      productID = g.Key.ProductID,
            //                      product = g.Key.Name, // You might need to set the product name here
            //                      months = g.Select(p => new Month
            //                      {
            //                          monthID = p.Date.Month,
            //                          productID = p.Product.ID,
            //                          product = p.Product.ProductTranslations.First().Text, // You might need to set the product name here
            //                          totalProducts = g.Sum(x => x.Qty),
            //                          uom = p.Product.UOM.UnitOfMeasureTranslation.First().Text // You might need to set the unit of measurement here
            //                      }).ToList()
            //                  }).ToList(),
            //        warehouseLocation = plan.Warehouse.Name + " - " + getDistance.WarehouseLocations.FirstOrDefault().distanceText,
            //        warehouseDistance = getDistance.WarehouseLocations.FirstOrDefault().distanceText
            //    };
            //}

            #endregion
            //var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            var languageCode = "EN";
            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            //string designationID = User.FindFirst("designationID")?.Value; // Retrieving designation ID from user claims
            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID.TrimStart('0')), languageCode);


            if (string.IsNullOrEmpty(req.planID))
                throw new AmazonFarmerException(_exceptions.planIDRequired);
            else
            {
                //tblPlan plan = await _repoWrapper.PlanRepo.getPlanByPlanID(Convert.ToInt32(req.planID), userID, languageCode);
                if (plan == null)
                    throw new AmazonFarmerException(_exceptions.planNotFound);
                //else if (User.IsInRole("Farmer") && plan.UserID != userID)
                //    throw new AmazonFarmerException(_exceptions.planNotFound);
                //else if (User.IsInRole("Employee"))
                //    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
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
        private async Task ApproveNewPlan(tblPlan plan, List<PlanCropProductPrice> planCropProductPrices,
            decimal AdvancePaymentAmount, EOrderType orderType)
        {

            if (plan != null)
            {
                decimal newProductPrice = 0;
                tblFarmerProfile profile = plan.User.FarmerProfile.First();

                TblOrders order;


                foreach (tblPlanCrops planCrop in plan.PlanCrops)
                {
                    foreach (tblPlanProduct planProduct in planCrop.PlanProducts)
                    {

                        //Get Actual Product Plan Price
                        PlanCropProductPrice? planProductPrice = planCropProductPrices.Where(pp => pp.ProductCode == planProduct.Product.ProductCode).FirstOrDefault();

                        newProductPrice = planProductPrice.UnitTotalAmount * planProduct.Qty;

                        order = await CreateANewOrder(plan, planCrop, planProduct, newProductPrice, EOrderType.Product, planProductPrice, planProduct.Qty);

                    }
                }
                //Advance Payment Order
                if (AdvancePaymentAmount != 0)
                {
                    //decimal AdvancePaymentAmount = (newPlanTotalPrice * percentageValue) / 100;
                    order = await CreateANewOrder(plan, null, null, AdvancePaymentAmount, orderType, planCropProductPrices.FirstOrDefault(), 0);
                }
                else
                {
                    if (orderType != EOrderType.AdvancePaymentReconcile)
                    {
                        throw new AmazonFarmerException(_exceptions.invalidMethod);
                    }
                }
            }
        }
        private async Task UpdateOrderServices(tblPlan plan)
        {

            if (plan != null)
            {
                decimal newProductPrice = 0;

                List<tblPlanService> planServices = new List<tblPlanService>();
                List<TblOrderService> orderServices = new List<TblOrderService>();

                foreach (tblPlanCrops planCrop in plan.PlanCrops)
                {
                    planServices.AddRange(planCrop.PlanServices);
                }

                orderServices.AddRange(plan.OrderServices);
                orderServices.ForEach(order => { order.Status = EActivityStatus.DeActive; });

                foreach (tblPlanService planService in planServices)
                {
                    TblOrderService? orderService = orderServices
                        .Where(os => os.CropGroupID == planService.PlanCrop.CropGroupID && os.ServiceID == planService.ServiceID).FirstOrDefault();
                    if (orderService == null)
                    {
                        orderService = new TblOrderService
                        {
                            Plan = plan,
                            Amount = 0,
                            ClosingQTY = 0,
                            CropGroupID = planService.PlanCrop.CropGroupID,
                            QTY = 0,
                            ServiceID = planService.ServiceID,
                            Status = planService.Status,
                            UnitPrice = 0,
                            UnitTax = 0,
                            UnitTotalAmount = 0,
                            LandPreparationDate = planService.LandPreparationDate,
                            LastHarvestDate = planService.LastHarvestDate,
                            SewingDate = planService.SewingDate,
                        };

                        _repoWrapper.OrderRepo.AddOrderService(orderService);
                    }
                    else
                    {
                        //If order service was created in current iterations
                        if (orderService.OrderServiceID != 0)
                        {
                            orderService.LandPreparationDate = planService.LandPreparationDate;
                            orderService.LastHarvestDate = planService.LastHarvestDate;
                            orderService.SewingDate = planService.SewingDate;
                            orderService.Status = planService.Status;
                            _repoWrapper.OrderRepo.UpdateOrderService(orderService);
                        }
                    }
                }

            }
        }

        private async Task ApprovePlan(tblPlan plan, List<PlanCropProductPrice> planCropProductPrices, List<TblOrders> planOrders)
        {
            //Get Advance payment Percentage from DB
            string configValue = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentPercent);
            int percentageValue = Convert.ToInt32(configValue);

            //Get Sum of Total Plan product prices
            decimal newPlanTotalPrice = planCropProductPrices.Sum(pp => pp.TotalAmount);
            decimal oldOrderTotalPrice = 0;
            List<TblOrders> oldOrdersAdvance = [];

            bool isAdvancedPaymentMade = planOrders
                .Where(o => o.PaymentStatus != EOrderPaymentStatus.NonPaid && o.OrderType == EOrderType.Advance).Any();

            if (plan != null)
            {

                //Get new Plan advance payment percentage total amount
                decimal newPlanAdvancePaymentAmount = GetAdanvcePaymentAmount(newPlanTotalPrice, percentageValue);

                if (isAdvancedPaymentMade)
                {
                    //Get all orders for plan that are advance payment or advance reconcilled with payment Made or processing
                    oldOrdersAdvance = planOrders.Where(o => o.OrderType == EOrderType.Advance
                    || o.OrderType == EOrderType.AdvancePaymentReconcile
                    &&
                    (
                        o.PaymentStatus == EOrderPaymentStatus.Paid || o.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
                    )
                    ).ToList();


                    //Get old orders total price with in plan
                    oldOrderTotalPrice = await GetOrderPrice(plan, planCropProductPrices, planOrders);

                    if (newPlanTotalPrice > oldOrderTotalPrice)
                    {
                        //Create new Advanced Payment 
                        //with balance amount of {newPlanTotalPrice - oldOrderTotalPrice}
                        decimal oldOrderTotalAdvancePayment = oldOrdersAdvance.Sum(o => o.OrderAmount);
                        decimal newOrderReconciliationAmount = newPlanAdvancePaymentAmount - oldOrderTotalAdvancePayment;

                        TblOrders nonPaidAdvanceReconcileOrder = planOrders
                            .Where(o => o.PaymentStatus == EOrderPaymentStatus.NonPaid && o.OrderType == EOrderType.AdvancePaymentReconcile)
                            .FirstOrDefault();

                        //If Non paid advance reconcile order DOESN't exists then create a new Reconcile order
                        if (nonPaidAdvanceReconcileOrder == null)
                        {
                            TblOrders orderNew = await CreateANewOrder(plan, null, null, newOrderReconciliationAmount, EOrderType.AdvancePaymentReconcile, planCropProductPrices.FirstOrDefault(), 0);
                        }
                        //If Non paid advance reconcile order already exists then update the reconcile order
                        else
                        {
                            //For Advance reconcile
                            TblOrders orderNew = await UpdateAdvancePaymentOrder(plan, null, null, newOrderReconciliationAmount, EOrderType.AdvancePaymentReconcile, planCropProductPrices.FirstOrDefault(), nonPaidAdvanceReconcileOrder);
                        }

                    }
                }
                else
                {
                    TblOrders orderWithAdvancePayment = planOrders.Where(o => o.OrderType == EOrderType.Advance).FirstOrDefault();
                    if (orderWithAdvancePayment != null)
                    {
                        //For Advance
                        TblOrders orderNew = await UpdateAdvancePaymentOrder(plan, null, null, newPlanAdvancePaymentAmount, EOrderType.Advance, planCropProductPrices.FirstOrDefault(), orderWithAdvancePayment);
                    }
                }

                //List<tblPlanCrops> deletedPlanCrops = wholePlan.PlanCrops.Where(pc => pc.Status == EActivityStatus.DeActive).ToList();
                tblPlan planNew = await _repoWrapper.PlanRepo.getPlanWithAllProducts(plan.ID);

                List<tblPlanCrops> deletedPlanCrops = planNew.PlanCrops.Where(pc => pc.Status == EActivityStatus.DeActive).ToList();


                await DeletedPlanCrops(plan, deletedPlanCrops, planOrders);

                await DeleteOrderNotAvailableInPlan(planOrders, plan.ID);

                await ActivePlanCrops(plan, planCropProductPrices, planOrders);

                await _repoWrapper.SaveAsync(); //Will be saving at one place so that all fail or all succeed 
            }
        }

        //private async Task ApprovePlan(tblPlan plan, List<PlanCropProductPrice> planCropProductPrices, tblPlan wholePlan)
        //{
        //    int orderWarehouseId = plan.Orders.Where(o => o.DeliveryStatus != EDeliveryStatus.ShipmentComplete
        //    && o.OrderStatus == EOrderStatus.Active && o.OrderType == EOrderType.Product).Select(o => o.WarehouseID).FirstOrDefault(); 

        //    //Get Advance payment Percentage from DB
        //    string configValue = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentPercent);
        //    int percentageValue = Convert.ToInt32(configValue);

        //    //Get Sum of Total Plan product prices
        //    decimal newPlanTotalPrice = planCropProductPrices.Sum(pp => pp.TotalAmount);
        //    decimal oldOrderTotalPrice = 0;
        //    List<TblOrders> oldOrdersAdvance = [];

        //    bool isAnyPaymentMade = plan.Orders.Where(o => o.PaymentStatus != EOrderPaymentStatus.NonPaid).Any();

        //    bool isAdvancedPaymentMade = plan.Orders
        //        .Where(o => o.PaymentStatus != EOrderPaymentStatus.NonPaid && o.OrderType == EOrderType.Advance).Any(); 


        //    if (plan != null)
        //    {


        //        //Get new Plan advance payment percentage total amount
        //        decimal newPlanAdvancePaymentAmount = GetAdanvcePaymentAmount(newPlanTotalPrice, percentageValue);

        //        if (isAdvancedPaymentMade)
        //        {
        //            //Get all orders for plan that are advance payment or advance reconcilled with payment Made or processing
        //            oldOrdersAdvance = plan.Orders.Where(o => o.OrderType == EOrderType.Advance
        //            || o.OrderType == EOrderType.AdvancePaymentReconcile
        //            &&
        //            (
        //                o.PaymentStatus == EOrderPaymentStatus.Paid || o.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
        //            )
        //            ).ToList();


        //            //Get old orders total price with in plan
        //            oldOrderTotalPrice = await GetOrderPrice(plan, planCropProductPrices);

        //            if (newPlanTotalPrice > oldOrderTotalPrice)
        //            {
        //                //Create new Advanced Payment 
        //                //with balance amount of {newPlanTotalPrice - oldOrderTotalPrice}
        //                decimal oldOrderTotalAdvancePayment = oldOrdersAdvance.Sum(o => o.OrderAmount);
        //                decimal newOrderReconciliationAmount = newPlanAdvancePaymentAmount - oldOrderTotalAdvancePayment;
        //                TblOrders orderNew = await CreateANewOrder(plan, null, null, newOrderReconciliationAmount, EOrderType.AdvancePaymentReconcile, planCropProductPrices.FirstOrDefault(), 0);

        //            }
        //        }
        //        else
        //        {
        //            TblOrders orderWithAdvancePayment = plan.Orders.Where(o => o.OrderType == EOrderType.Advance).FirstOrDefault();
        //            if (orderWithAdvancePayment != null)
        //            {
        //                TblOrders orderNew = await UpdateAdvancePaymentOrder(plan, null, null, newPlanAdvancePaymentAmount, EOrderType.Advance, planCropProductPrices.FirstOrDefault(), orderWithAdvancePayment);
        //            }
        //        }

        //        //List<tblPlanCrops> deletedPlanCrops = wholePlan.PlanCrops.Where(pc => pc.Status == EActivityStatus.DeActive).ToList();
        //        tblPlan planNew = await _repoWrapper.PlanRepo.getPlanWithAllProducts(plan.ID);

        //        List<tblPlanCrops> deletedPlanCrops = planNew.PlanCrops.Where(pc => pc.Status == EActivityStatus.DeActive).ToList();


        //        await DeletedPlanCrops(plan, deletedPlanCrops);
        //        await DeleteOrderNotAvailableInPlan(plan.Orders, plan.ID);
        //        await ActivePlanCrops(plan, planCropProductPrices);

        //        await _repoWrapper.SaveAsync(); //Will be saving at one place so that all fail or all succeed 
        //    }
        //}

        private async Task unLockOrders(int planID)
        {
            List<TblOrders> orders = await _repoWrapper.OrderRepo.getOrdersByPlanID(planID);
            if (orders != null && orders.Count() > 0)
            {
                foreach (TblOrders order in orders)
                {
                    order.isLocked = false;
                    await _repoWrapper.OrderRepo.UpdateOrder(order);
                }
                await _repoWrapper.SaveAsync();
            }
        }
        private async Task DeletedPlanCrops(tblPlan plan, List<tblPlanCrops> deletedPlanCrops, List<TblOrders> planOrders)
        {
            foreach (tblPlanCrops deletedPlanCrop in deletedPlanCrops)
            {
                foreach (tblPlanProduct planProduct in deletedPlanCrop.PlanProducts)
                {
                    List<TblOrders> paidPlanOrders = planOrders
                        .Where(o => o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile).ToList()
                        .Where(o => o.Products.Where(op => op.ProductID == planProduct.ProductID).Any()
                           && o.PaymentStatus == EOrderPaymentStatus.Paid
                           && o.CropID == deletedPlanCrop.CropGroupID
                           )
                        .ToList();

                    List<TblOrders> nonPaidPlanOrders = planOrders
                        .Where(o => o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile).ToList()
                        .Where(o => o.Products.Where(op => op.ProductID == planProduct.ProductID
                           ).Any()
                           && o.PaymentStatus == EOrderPaymentStatus.NonPaid
                           && o.CropID == deletedPlanCrop.CropGroupID)
                        .ToList();

                    await DeleteOrder(plan, planProduct, paidPlanOrders, nonPaidPlanOrders);
                }
            }
        }
        private async Task RejectPlan(tblPlan plan, EPlanApprovalRejectionType ePlanApprovalRejectionType)
        {

            bool isRejectionNotAllowed = plan.Orders.Where(ao => ao.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
             || ao.PaymentStatus == EOrderPaymentStatus.LedgerUpdate
             || ao.PaymentStatus == EOrderPaymentStatus.Acknowledged
             ).Any();


            if (isRejectionNotAllowed)
            {
                throw new AmazonFarmerException(_exceptions.rejectionNotAllowed);
            }

            //All product type orders
            List<TblOrders> allOrders = plan.Orders.Where(o => o.OrderType == EOrderType.Product).ToList();


            List<TblOrders> paidOrders = allOrders.Where(o => o.PaymentStatus == EOrderPaymentStatus.Paid)
                .ToList();

            List<TblOrders> nonPaidOrders = allOrders.Where(o => o.PaymentStatus == EOrderPaymentStatus.NonPaid)
                .ToList();

            //Actions for paid orders
            foreach (TblOrders order in paidOrders)
            {
                await DeletePaidOrder(order, null);
            }

            //Actions for non paid orders
            foreach (TblOrders order in nonPaidOrders)
            {
                await DeleteNonPaidOrder(order, null);
            }

            plan.Status = EPlanStatus.Rejected;
            await _repoWrapper.PlanRepo.updatePlan(plan);

            List<TblOrders> allAdvancePaymentOrders = plan.Orders
                .Where(o => o.OrderType == EOrderType.Advance || o.OrderType == EOrderType.AdvancePaymentReconcile)
                .ToList();

            List<TblOrders> allOrderReconcileOrders = plan.Orders
                .Where(o => o.OrderType == EOrderType.OrderReconcile)
                .ToList();

            if (ePlanApprovalRejectionType == EPlanApprovalRejectionType.ForfeitAdvance)
            {
                foreach (TblOrders advanceOrder in allAdvancePaymentOrders)
                {
                    if (advanceOrder.PaymentStatus == EOrderPaymentStatus.NonPaid)
                    {
                        advanceOrder.OrderStatus = EOrderStatus.Deleted;
                    }
                    else
                    {
                        await ChangeCustomerPaymentWSDL(advanceOrder, refundText: "Z042", reasonCode: "FA");
                        advanceOrder.PaymentStatus = EOrderPaymentStatus.Forfeit;
                        advanceOrder.IsConsumed = true;
                    }
                    await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                    _repoWrapper.OrderRepo.AddOrderLog(advanceOrder, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
            }
            else if (ePlanApprovalRejectionType == EPlanApprovalRejectionType.RefundAdvance)
            {
                foreach (TblOrders advanceOrder in allAdvancePaymentOrders)
                {
                    if (advanceOrder.PaymentStatus == EOrderPaymentStatus.NonPaid)
                    {
                        advanceOrder.OrderStatus = EOrderStatus.Deleted;
                    }
                    else
                    {
                        await ChangeCustomerPaymentWSDL(advanceOrder, refundText: "Z045", reasonCode: "");
                        advanceOrder.PaymentStatus = EOrderPaymentStatus.Refund;
                        advanceOrder.IsConsumed = true;
                    }
                    await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                    _repoWrapper.OrderRepo.AddOrderLog(advanceOrder, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
            }

            foreach (TblOrders reconcileOrder in allOrderReconcileOrders)
            {
                if (reconcileOrder.PaymentStatus == EOrderPaymentStatus.NonPaid)
                {
                    reconcileOrder.OrderStatus = EOrderStatus.Deleted;
                }
                else
                {
                    reconcileOrder.PaymentStatus = EOrderPaymentStatus.Refund;
                }
                await _repoWrapper.OrderRepo.UpdateOrder(reconcileOrder);
                _repoWrapper.OrderRepo.AddOrderLog(reconcileOrder, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            await _repoWrapper.SaveAsync();
        }

        private async Task DeleteOrder(tblPlan plan, tblPlanProduct planProductNoTracting, List<TblOrders> paidPlanOrders, List<TblOrders> nonPaidPlanOrders)
        {
            tblPlanCrops planCrop = plan.PlanCrops.Where(pc => pc.ID == planProductNoTracting.PlanCrop.ID).FirstOrDefault();
            tblPlanProduct planProduct = null;
            if (planCrop != null)
            {
                planProduct = planCrop.PlanProducts.Where(pp => pp.ID == planProductNoTracting.ID).FirstOrDefault();
            }

            //Actions for paid orders
            foreach (TblOrders order in paidPlanOrders)
            {
                await DeletePaidOrder(order, planProduct);
            }

            //Actions for non paid orders
            foreach (TblOrders order in nonPaidPlanOrders)
            {
                await DeleteNonPaidOrder(order, planProduct);
            }
        }

        private async Task DeleteNonPaidOrder(TblOrders order, tblPlanProduct planProduct)
        {
            order.OrderStatus = EOrderStatus.Deleted;

            await DeleteAllChildReconciliationOrders(order);
            await _repoWrapper.OrderRepo.UpdateOrder(order);
            _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (planProduct != null)
            {
                planProduct.Status = EActivityStatus.DeActive;
                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);
            }
        }

        private async Task DeletePaidOrder(TblOrders order, tblPlanProduct planProduct)
        {
            //No action needed for Fully Shipped
            if (order.DeliveryStatus == EDeliveryStatus.PartiallyDelivered)
            {
                order.DeliveryStatus = EDeliveryStatus.ShipmentComplete;
                order.PaymentStatus = EOrderPaymentStatus.Refund;
                await DeleteAllChildReconciliationOrders(order);
                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (planProduct != null)
                {
                    planProduct.PaymentStatus = EOrderPaymentStatus.Refund;
                    planProduct.DeliveryStatus = EDeliveryStatus.ShipmentComplete;
                    planProduct.DeliveredQty = order.Products.FirstOrDefault().ClosingQTY;

                }
                await RefundOrderWSDL(order);
            }
            else if (order.DeliveryStatus == EDeliveryStatus.None)
            {
                //Refund order {WSDL request} 
                order.PaymentStatus = EOrderPaymentStatus.Refund;
                await DeleteAllChildReconciliationOrders(order);
                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (planProduct != null)
                {
                    planProduct.PaymentStatus = EOrderPaymentStatus.Refund;
                }
                await RefundOrderWSDL(order);
            }
            await _repoWrapper.OrderRepo.UpdateOrder(order);

            if (planProduct != null)
            {
                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);
            }
        }

        private async Task DeleteAllChildReconciliationOrders(TblOrders parentOrder)
        {
            foreach (var item in parentOrder.ChildOrders)
            {
                item.OrderStatus = parentOrder.OrderStatus;
                item.PaymentStatus = parentOrder.PaymentStatus;
                item.DeliveryStatus = parentOrder.DeliveryStatus;
                await _repoWrapper.OrderRepo.UpdateOrder(item);

            }
        }
        private async Task DeleteOrderNotAvailableInPlan(List<TblOrders> orders, int planId)
        {
            tblPlan planNew = await _repoWrapper.PlanRepo.getPlanWithAllProducts(planId);

            foreach (TblOrders order in orders
                .Where(o => o.OrderStatus != EOrderStatus.Deleted
                    && (o.OrderType == EOrderType.Product || o.OrderType == EOrderType.OrderReconcile)))
            {
                TblOrderProducts? orderProduct = order.Products?.FirstOrDefault();
                if (orderProduct != null)
                {
                    List<tblPlanCrops> planCrops = planNew.PlanCrops
                        .Where(pc => pc.CropGroupID == order.CropID && pc.Status != EActivityStatus.DeActive).ToList();

                    foreach (tblPlanCrops tblPlanCrop in planCrops)
                    {
                        tblPlanProduct planProductNoTracking = tblPlanCrop.PlanProducts.Where(pp =>
                            pp.ID == orderProduct.PlanProductID
                            && pp.Status == EActivityStatus.DeActive)
                            .FirstOrDefault();

                        tblPlanProduct planProduct = null;
                        if (planProductNoTracking != null)
                        {
                            tblPlanCrops planCrop = order.Plan.PlanCrops.Where(pc => pc.ID == planProductNoTracking.PlanCrop.ID).FirstOrDefault();
                            planProduct = planCrop.PlanProducts.Where(pp => pp.ID == planProductNoTracking.ID).FirstOrDefault();
                        }

                        if (planProductNoTracking != null)
                        {
                            //Actions for paid orders
                            if (order.PaymentStatus == EOrderPaymentStatus.Paid || order.PaymentStatus == EOrderPaymentStatus.PaymentProcessing)
                            {
                                await DeletePaidOrder(order, planProduct);

                            }
                            //Actions for non paid orders
                            if (order.PaymentStatus == EOrderPaymentStatus.NonPaid)
                            {
                                await DeleteNonPaidOrder(order, planProduct);
                            }
                        }
                    }
                }
            }

        }

        private async Task ActivePlanCrops(tblPlan plan, List<PlanCropProductPrice> planCropProductPrices, List<TblOrders> planOrders)
        {
            int orderPaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.OrderPaymentBufferTime));


            List<tblPlanCrops> activePlanCrops = plan.PlanCrops.Where(pc => pc.Status == EActivityStatus.Active).ToList();

            List<tblPlanProduct> newlyAddedPlanProducts = new List<tblPlanProduct>();

            foreach (tblPlanCrops activePlanCrop in activePlanCrops)
            {
                foreach (tblPlanProduct planProduct in activePlanCrop.PlanProducts)
                {
                    List<TblOrders> paidPlanOrders = planOrders
                        .Where(o => o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile)
                            .ToList()
                        .Where(o => o.Products.Where(op => op.PlanProductID == planProduct.ID).Any())
                            .ToList()
                        .Where(o => o.PaymentStatus == EOrderPaymentStatus.Paid)
                            .ToList();

                    List<TblOrders> nonPaidPlanOrders = planOrders
                        .Where(o => o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile).ToList()
                        .Where(o => o.Products.Where(op => op.PlanProductID == planProduct.ID
                           && (o.PaymentStatus == EOrderPaymentStatus.NonPaid)
                           ).Any())
                        .ToList();

                    //Actions for paid orders
                    foreach (TblOrders order in paidPlanOrders)
                    {
                        PlanCropProductPrice planCropProductPrice = planCropProductPrices
                            .Where(pp => pp.ProductId == planProduct.ProductID).FirstOrDefault();

                        //No action needed for Fully Shipped
                        if (order.DeliveryStatus == EDeliveryStatus.PartiallyDelivered)
                        {
                            if (order.Products.FirstOrDefault().QTY == planProduct.Qty)
                            {
                                tblPlanProduct newAddedProduct = await UpdateOrderAndCreatePlanWhenWarehouseChange(plan, order, planProduct, activePlanCrop, planCropProductPrice);

                                await _repoWrapper.OrderRepo.UpdateOrder(order);

                                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);

                                if (newAddedProduct != null) newlyAddedPlanProducts.Add(newAddedProduct);

                                await _repoWrapper.OrderRepo.UpdateOrder(order);
                            }
                            else
                            {
                                order.DeliveryStatus = EDeliveryStatus.ShipmentComplete;
                                order.PaymentStatus = EOrderPaymentStatus.Refund;

                                planProduct.PaymentStatus = EOrderPaymentStatus.Refund;
                                planProduct.DeliveryStatus = EDeliveryStatus.ShipmentComplete;
                                planProduct.DeliveredQty = order.Products.FirstOrDefault().ClosingQTY;

                                await DeleteAllChildReconciliationOrders(order);
                                await _repoWrapper.OrderRepo.UpdateOrder(order);
                                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);

                                if (planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY > 0)
                                {
                                    tblPlanProduct newPlanProduct = new tblPlanProduct()
                                    {
                                        Date = planProduct.Date,
                                        DeliveredQty = 0,
                                        DeliveryStatus = EDeliveryStatus.None,
                                        PaymentStatus = EOrderPaymentStatus.NonPaid,
                                        PlanCropID = activePlanCrop.ID,
                                        ProductID = planProduct.ProductID,
                                        Qty = planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY,
                                        Status = EActivityStatus.Active
                                    };
                                    newlyAddedPlanProducts.Add(newPlanProduct);
                                    //Refund order {WSDL request}
                                    await CreateANewOrder(plan, activePlanCrop, planProduct, planCropProductPrice.TotalAmount, EOrderType.Product, planCropProductPrice, newPlanProduct.Qty);
                                }

                            }
                        }
                        else if (order.DeliveryStatus == EDeliveryStatus.None)
                        {
                            if (order.Products.FirstOrDefault().QTY == planProduct.Qty)
                            {
                                //No Action Needed 
                                tblPlanProduct newAddedProduct = await UpdateOrderAndCreatePlanWhenWarehouseChange(plan, order, planProduct, activePlanCrop, planCropProductPrice);

                                await _repoWrapper.OrderRepo.UpdateOrder(order);

                                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);

                                if (newAddedProduct != null)
                                    newlyAddedPlanProducts.Add(newAddedProduct);

                            }
                            else
                            {
                                order.PaymentStatus = EOrderPaymentStatus.Refund;
                                planProduct.PaymentStatus = EOrderPaymentStatus.Refund;


                                await DeleteAllChildReconciliationOrders(order);
                                await _repoWrapper.OrderRepo.UpdateOrder(order);

                                _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                                await _repoWrapper.PlanRepo.updatePlanProduct(planProduct);

                                if (planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY > 0)
                                {
                                    tblPlanProduct newPlanProduct = new tblPlanProduct()
                                    {
                                        Date = planProduct.Date,
                                        DeliveredQty = 0,
                                        DeliveryStatus = EDeliveryStatus.None,
                                        PaymentStatus = EOrderPaymentStatus.NonPaid,
                                        PlanCropID = activePlanCrop.ID,
                                        ProductID = planProduct.ProductID,
                                        Qty = planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY,
                                        Status = EActivityStatus.Active
                                    };
                                    newlyAddedPlanProducts.Add(newPlanProduct);

                                    //Refund order {WSDL request}
                                    await CreateANewOrder(plan, activePlanCrop, planProduct, planCropProductPrice.TotalAmount, EOrderType.Product, planCropProductPrice, newPlanProduct.Qty);
                                }
                            }
                        }
                    }
                    //Actions for non paid orders
                    foreach (TblOrders order in nonPaidPlanOrders)
                    {

                        TblOrderProducts orderProduct = order.Products.FirstOrDefault();
                        orderProduct.QTY = planProduct.Qty;
                        order.ExpectedDeliveryDate = planProduct.Date;
                        order.DuePaymentDate = planProduct.Date.AddDays(-orderPaymentBufferTime);
                        order.WarehouseID = plan.WarehouseID;

                        await _repoWrapper.OrderRepo.UpdateOrder(order);
                        _repoWrapper.OrderRepo.AddOrderLog(order, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        _repoWrapper.OrderRepo.UpdateOrderProduct(orderProduct);
                    }

                    await CreateNewOrdersForPlan(plan, activePlanCrop, planProduct, planCropProductPrices, planOrders);
                }
            }


            if (newlyAddedPlanProducts.Count() > 0)
            {
                foreach (var item in newlyAddedPlanProducts)
                {
                    await _repoWrapper.PlanRepo.addPlanProduct(item);

                }
            }


        }

        private async Task<tblPlanProduct> UpdateOrderAndCreatePlanWhenWarehouseChange(tblPlan plan, TblOrders order, tblPlanProduct planProduct,
            tblPlanCrops activePlanCrop, PlanCropProductPrice planCropProductPrice)
        {
            tblPlanProduct newPlanProduct = null;
            //If warehouse is different then refund and create order
            if (order.WarehouseID != plan.WarehouseID)
            {
                order.PaymentStatus = EOrderPaymentStatus.Refund;
                planProduct.PaymentStatus = EOrderPaymentStatus.Refund;

                await DeleteAllChildReconciliationOrders(order);
                await RefundOrderWSDL(order);

                if (planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY > 0)
                {
                    newPlanProduct = new tblPlanProduct()
                    {
                        Date = planProduct.Date,
                        DeliveredQty = 0,
                        DeliveryStatus = EDeliveryStatus.None,
                        PaymentStatus = EOrderPaymentStatus.NonPaid,
                        PlanCropID = activePlanCrop.ID,
                        ProductID = planProduct.ProductID,
                        Qty = planProduct.Qty - order.Products.FirstOrDefault().ClosingQTY,
                        Status = EActivityStatus.Active
                    };
                    //await _repoWrapper.PlanRepo.addPlanProduct(newPlanProduct);


                    await CreateANewOrder(plan, activePlanCrop, planProduct, planCropProductPrice.TotalAmount, EOrderType.Product, planCropProductPrice, newPlanProduct.Qty);
                }
            }

            return newPlanProduct;
        }
        private async Task CreateNewOrdersForPlan(tblPlan plan, tblPlanCrops planCrop, tblPlanProduct planProduct,
            List<PlanCropProductPrice> planCropProductPrices, List<TblOrders> planOrders)
        {

            List<TblOrders> matchingCropOrders = planOrders.Where(oo => oo.CropID == planCrop.CropGroupID).ToList();
            TblOrders orderNew = null;
            List<TblOrders> NewProductsInPlan = new List<TblOrders>();


            PlanCropProductPrice planCropProductPrice = planCropProductPrices.Where(ppp => ppp.ProductId == planProduct.ProductID && ppp.PlanCropId == planCrop.ID).FirstOrDefault();
            decimal orderAmount = planCropProductPrice.TotalAmount;

            //If there is no Crop that is already present in Old Orders
            if (matchingCropOrders.Count() == 0)
            {


                orderNew = await CreateANewOrder(plan, planCrop, planProduct, orderAmount, EOrderType.Product, planCropProductPrice, planProduct.Qty);
                NewProductsInPlan.Add(orderNew);//Saving for later use
            }
            //If there is Crop that is already present in Old Orders
            else
            {
                TblOrders? currentMatchedOrder = matchingCropOrders.Where(mpc =>
                     mpc.Products.Where(p => p.PlanProductID == planProduct.ID).Any()).FirstOrDefault();

                //Create new order when there is no product matching this plan product
                if (currentMatchedOrder == null)
                {

                    orderNew = await CreateANewOrder(plan, planCrop, planProduct, orderAmount, EOrderType.Product, planCropProductPrice, planProduct.Qty);
                    NewProductsInPlan.Add(orderNew);//Saving for later use
                }
            }

        }

        private int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 10000);
        }
        private async Task<List<PlanCropProductPrice>> GetPlanPrice(tblPlan plan, decimal percentageValue)
        {

            decimal newProductPrice = 0;
            decimal newPlanTotalPrice = 0;
            tblFarmerProfile profile = plan.User.FarmerProfile.First();

            List<PlanCropProductPrice> planCropProductPrices = new List<PlanCropProductPrice>();

            foreach (tblPlanCrops planCrop in plan.PlanCrops)
            {
                foreach (tblPlanProduct planProduct in planCrop.PlanProducts.Where(pp => pp.Status == EActivityStatus.Active))
                {

                    List<PlanCropProductPrice_Services> newServices = planCrop.PlanServices.Where(x => x.Status == EActivityStatus.Active)
                        .Select(ps => new PlanCropProductPrice_Services
                        {
                            ServiceCode = ps.Service.Code
                        }).ToList();

                    PlanCropProductPrice? planProductPrice = planCropProductPrices
                        .Where(pp => pp.ProductCode == planProduct.Product.ProductCode
                            && pp.Services.Count == newServices.Count &&
                            pp.Services.TrueForAll(s => newServices.Exists(ns => ns.ServiceCode == s.ServiceCode)))
                        .FirstOrDefault();


                    if (planProductPrice == null)
                    {

                        RequestType request = new()
                        {
                            condGp1 = planCrop.PlanServices.Count > 0 ? planCrop.PlanServices[0].Service.Code : "",
                            condGp2 = planCrop.PlanServices.Count > 1 ? planCrop.PlanServices[1].Service.Code : "",
                            condGp3 = planCrop.PlanServices.Count > 2 ? planCrop.PlanServices[2].Service.Code : "",
                            condGp4 = planCrop.PlanServices.Count > 3 ? planCrop.PlanServices[3].Service.Code : "",
                            custNum = profile.SAPFarmerCode,
                            custRef = "Created Plan for Farm",
                            division = planProduct.Product.Division,
                            matNum = planProduct.Product.ProductCode,
                            saleDistict = plan.Warehouse.SalePoint,
                            salesOrg = planProduct.Product.SalesOrg,
                            saleUnit = planProduct.Product.UOM.UOM
                        };
                        WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
                        ResponseType wsdlResponse = await wSDLFunctions.PriceSimluate(request);


                        if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0
                            && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S" && !string.IsNullOrEmpty(wsdlResponse.itemNum.TrimStart('0')))
                        {
                            newProductPrice = (Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal)) * planProduct.Qty;
                            planProductPrice = new()
                            {
                                Quantity = planProduct.Qty,
                                UnitPrice = Convert.ToDecimal(wsdlResponse.netVal),
                                UnitTax = Convert.ToDecimal(wsdlResponse.taxVal),
                                UnitTotalAmount = Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal),
                                TotalAmount = newProductPrice,
                                PlanCropId = planCrop.ID,
                                ProductId = planProduct.ProductID,
                                ProductCode = planProduct.Product.ProductCode,
                                AdvancePercentValue = percentageValue,
                                Services = newServices
                            };
                            planCropProductPrices.Add(planProductPrice);
                            newPlanTotalPrice += newProductPrice;

                        }
                        else
                        {
                            throw new AmazonFarmerException(_exceptions.pricingNotMaintained);
                        }
                    }
                    else
                    {
                        newProductPrice = planProduct.Qty * planProductPrice.UnitTotalAmount;
                        planProductPrice = new()
                        {
                            Quantity = planProduct.Qty,
                            UnitPrice = planProductPrice.UnitPrice,
                            UnitTax = planProductPrice.UnitTax,
                            UnitTotalAmount = planProductPrice.UnitTotalAmount,
                            TotalAmount = newProductPrice,
                            PlanCropId = planCrop.ID,
                            ProductId = planProduct.ProductID,
                            ProductCode = planProduct.Product.ProductCode,
                            AdvancePercentValue = percentageValue,
                            Services = newServices
                        };
                        planCropProductPrices.Add(planProductPrice);
                        newPlanTotalPrice += newProductPrice;
                    }
                }
            }

            return planCropProductPrices;
        }

        private async Task<decimal> GetOrderPrice(tblPlan plan, List<PlanCropProductPrice> planCropProductPrices, List<TblOrders> planOrders)
        {
            decimal oldOrderTotalPrice = 0;

            decimal oldProductPrice = 0;
            tblFarmerProfile profile = plan.User.FarmerProfile.First();


            foreach (TblOrders planOrder in planOrders)
            {
                foreach (TblOrderProducts orderProduct in planOrder.Products)
                {
                    List<PlanCropProductPrice_Services> newServices = plan.OrderServices
                        .Select(ps => new PlanCropProductPrice_Services
                        {
                            ServiceCode = ps.Service.Code
                        }).ToList();

                    PlanCropProductPrice? alreadyFetchedProductPrice = planCropProductPrices
                        .Where(pp => pp.ProductCode == orderProduct.Product.ProductCode
                            && pp.Services.Count == newServices.Count &&
                            pp.Services.TrueForAll(s => newServices.Exists(ns => ns.ServiceCode == s.ServiceCode)))
                        .FirstOrDefault();

                    //Check if already fetched the latest price through simulation
                    if (alreadyFetchedProductPrice == null)
                    {
                        RequestType request = new()
                        {
                            condGp1 = plan.OrderServices.Count > 0 ? plan.OrderServices[0].Service.Code : "",
                            condGp2 = plan.OrderServices.Count > 1 ? plan.OrderServices[1].Service.Code : "",
                            condGp3 = plan.OrderServices.Count > 2 ? plan.OrderServices[2].Service.Code : "",
                            condGp4 = plan.OrderServices.Count > 3 ? plan.OrderServices[3].Service.Code : "",
                            custNum = profile.SAPFarmerCode,
                            custRef = "Created Plan for Farm",
                            division = orderProduct.Product.Division,
                            matNum = orderProduct.Product.ProductCode,
                            saleDistict = planOrder.Warehouse.SalePoint,
                            salesOrg = orderProduct.Product.SalesOrg,
                            saleUnit = orderProduct.Product.UOM.UOM
                        };
                        WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
                        //oldProductPrice = await wSDLFunctions.PriceSimluate(request) * orderProduct.QTY;
                        var wsdlResponse = await wSDLFunctions.PriceSimluate(request);

                        if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0
                           && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S" && !string.IsNullOrEmpty(wsdlResponse.itemNum.TrimStart('0')))
                        {
                            int PlanCropID = await _repoWrapper.PlanRepo.getPlanCropIDByPlanProductID(orderProduct.PlanProductID);

                            oldProductPrice = (Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal)) * orderProduct.QTY;
                            PlanCropProductPrice planProductPrice = new()
                            {
                                Quantity = orderProduct.QTY,
                                UnitPrice = Convert.ToDecimal(wsdlResponse.netVal),
                                UnitTax = Convert.ToDecimal(wsdlResponse.taxVal),
                                UnitTotalAmount = Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal),
                                TotalAmount = oldProductPrice,
                                PlanCropId = PlanCropID,
                                ProductId = orderProduct.ProductID,
                                ProductCode = orderProduct.Product.ProductCode,
                                AdvancePercentValue = planOrder.AdvancePercent,
                                Services = newServices
                            };
                            planCropProductPrices.Add(planProductPrice);
                            oldOrderTotalPrice += oldProductPrice;
                        }

                    }
                    else
                    {
                        oldProductPrice = orderProduct.QTY * alreadyFetchedProductPrice.UnitTotalAmount;
                        oldOrderTotalPrice += oldProductPrice;
                    }
                }
            }

            return oldOrderTotalPrice;
        }

        private async Task<TblOrders> CreateANewOrder(tblPlan plan, tblPlanCrops? planCrop, tblPlanProduct? planProduct, decimal orderAmount, EOrderType orderType,
            PlanCropProductPrice? planProductPrice, int quantity)
        {
            DateTime DuePaymentDate = DateTime.UtcNow;
            DateTime deliveryDate = DateTime.UtcNow;
            List<DateTime> deliveryDates = new List<DateTime>();



            if (orderType == EOrderType.Advance || orderType == EOrderType.AdvancePaymentReconcile)
            {
                //Get closest delivery date for non paid plan products
                foreach (var iPlanCrop in plan.PlanCrops)
                {
                    deliveryDate = iPlanCrop.PlanProducts.Where(pp => pp.PaymentStatus == EOrderPaymentStatus.NonPaid).OrderBy(pp => pp.Date).Select(pp => pp.Date).FirstOrDefault();
                    deliveryDates.Add(deliveryDate);
                }

                deliveryDate = deliveryDates.OrderBy(dd => dd).FirstOrDefault();

                int advancePaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentBufferTime));
                DuePaymentDate = deliveryDate.AddDays(-advancePaymentBufferTime);
            }
            else
            {
                deliveryDate = planProduct.Date;
                int orderPaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.OrderPaymentBufferTime));
                DuePaymentDate = deliveryDate.AddDays(-orderPaymentBufferTime);
            }

            TblOrders newOrder = new TblOrders()
            {
                AdvancePercent = planProductPrice != null ? planProductPrice.AdvancePercentValue : 0,
                ApprovalDate = DateTime.UtcNow,
                ApprovalDatePrice = orderAmount,
                CreatedByID = plan.UserID,
                CreatedOn = DateTime.UtcNow,
                DuePaymentDate = DuePaymentDate,//To be figured out
                InvoicedDate = null,
                InvoicedDatePrice = null,
                OrderAmount = orderAmount,
                OrderName = orderType == EOrderType.Advance || orderType == EOrderType.AdvancePaymentReconcile ? "Advance Payment Order" : "Plan Product Order",
                OrderStatus = EOrderStatus.Active,
                OrderType = orderType,
                ParentOrderID = null,
                PaymentDate = null,
                PaymentDatePrice = null,
                SAPTransactionID = null,
                OneLinkTransactionID = null,
                PaymentStatus = EOrderPaymentStatus.NonPaid,
                CropID = planCrop?.CropGroupID,
                PlanID = plan.ID,
                SAPOrderID = null,
                WarehouseID = plan.WarehouseID,
                OrderRandomTransactionID = GetRandomNumber(),
                ExpectedDeliveryDate = planProduct == null ? null : planProduct.Date,
                Products = planProductPrice == null || planProduct == null ? null :
                    [
                        new TblOrderProducts
                        {
                            Amount = orderAmount,
                            ProductID = planProduct.ProductID,
                            QTY = quantity,
                            UnitTax = planProductPrice.UnitTax,
                            UnitPrice = planProductPrice.UnitPrice,
                            UnitTotalAmount = planProductPrice.UnitTotalAmount,
                            PlanProductID = planProduct.ID,
                        }
                    ]
            };
            newOrder = _repoWrapper.OrderRepo.AddOrder(newOrder);

            //);
            return newOrder;
        }
        private async Task<TblOrders> UpdateAdvancePaymentOrder(tblPlan plan, tblPlanCrops? planCrop, tblPlanProduct? planProduct,
            decimal orderAmount, EOrderType orderType, PlanCropProductPrice? planProductPrice, TblOrders order)
        {
            DateTime DuePaymentDate = DateTime.UtcNow;
            DateTime deliveryDate = DateTime.UtcNow;
            List<DateTime> deliveryDates = new List<DateTime>();

            if (orderType == EOrderType.Advance || orderType == EOrderType.AdvancePaymentReconcile)
            {
                //Get closest delivery date for non paid plan products
                foreach (var iPlanCrop in plan.PlanCrops)
                {
                    deliveryDate = iPlanCrop.PlanProducts.Where(pp => pp.PaymentStatus == EOrderPaymentStatus.NonPaid).OrderBy(pp => pp.Date).Select(pp => pp.Date).FirstOrDefault();
                    deliveryDates.Add(deliveryDate);
                }
                if (deliveryDates.Count() > 0)
                {
                    deliveryDate = deliveryDates.OrderBy(dd => dd).FirstOrDefault();
                    int advancePaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentBufferTime));
                    DuePaymentDate = deliveryDate.AddDays(-advancePaymentBufferTime);
                }
            }

            order.AdvancePercent = planProductPrice != null ? planProductPrice.AdvancePercentValue : 0;
            order.ApprovalDate = DateTime.UtcNow;
            order.ApprovalDatePrice = orderAmount;
            order.CreatedByID = plan.UserID;
            order.CreatedOn = DateTime.UtcNow;
            order.DuePaymentDate = DuePaymentDate;
            order.InvoicedDate = null;
            order.InvoicedDatePrice = null;
            order.OrderAmount = orderAmount;
            order.OrderName = orderType == EOrderType.Advance || orderType == EOrderType.AdvancePaymentReconcile ? "Advance Payment Order" : "Plan Product Order";
            order.OrderStatus = EOrderStatus.Active;
            order.OrderType = orderType;
            order.ParentOrderID = null;
            order.PaymentDate = null;
            order.PaymentDatePrice = null;
            order.OneLinkTransactionID = null;
            order.SAPTransactionID = null;
            order.PaymentStatus = EOrderPaymentStatus.NonPaid;
            order.CropID = planCrop?.CropGroupID;
            order.PlanID = plan.ID;
            order.SAPOrderID = null;
            order.WarehouseID = plan.WarehouseID;
            order.ExpectedDeliveryDate = planProduct == null ? null : planProduct.Date;
            await _repoWrapper.OrderRepo.UpdateOrder(order);
            return order;
        }
        private decimal GetAdanvcePaymentAmount(decimal totalAmount, int percentageValue)
        {
            return totalAmount * percentageValue / Convert.ToDecimal(100);
        }
        private async Task<bool> RefundOrderWSDL(TblOrders order)
        {
            ZSD_AMAZON_SALEORD_CHG request = new()
            {
                CONDGRP_1 = "ZZ",
                CONDGRP_2 = "ZZ",
                CONDGRP_3 = "ZZ",
                CONDGRP_4 = "ZZ",
                CUST_REF = "ZZ",
                REQ_DELIVERY_DATE = DateTime.Now.ToString(WSDLFunctions.WSDLDateFormat),
                PLANT = "ZZ",
                STORAGE_LOC = "ZZ",
                REASON_REJ = "00",
                SALE_ORD = order.SAPOrderID

            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            var wsdlResponse = await wSDLFunctions.ChangeSaleOrderRequest(request);

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
        //Unlock locked tranaction on sap
        private async Task<bool> ChangeCustomerPaymentWSDL(TblOrders order, string refundText, string reasonCode)
        {
            ZSD_AMAZON_CUSTOMER_PAY_CHG request = new()
            {
                COMPANY_CODE = order.CompanyCode,
                DOC_NUM = order.SAPTransactionID,
                FISCAL_YEAR = order.FiscalYear,
                REASON_CODE = reasonCode,
                TEXT = refundText
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
