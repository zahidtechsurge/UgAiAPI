using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for a farming plan.
    /// </summary>
    public class OLD_PlanDTO
    {
        // The ID of the farm
        public int farmID { get; set; }

        // The ID of the season
        public int seasonID { get; set; }

        // List of crops in the plan
        public List<PlanCrop_Req> crops { get; set; } = new List<PlanCrop_Req>();

        /// <summary>
        /// Draft: 0, 
        /// Submit: for Approval 1, 
        /// The request type of the plan
        /// </summary>
        public int requestType { get; set; }

        // The request type of the plan
        public int warehouseID { get; set; }
        public string farmerComment { get; set; }
    }
    /// <summary>
    /// Data Transfer Object (DTO) for a farming plan.
    /// </summary>
    public class PlanDTO
    {
        // The ID of the farm
        public int farmID { get; set; }

        // The ID of the season
        public int seasonID { get; set; }

        // List of crops in the plan
        public List<PlanCrop_Req> crops { get; set; } = new List<PlanCrop_Req>();

        /// <summary>
        /// Draft: 0, 
        /// Submit: for Approval 1, 
        /// The request type of the plan
        /// </summary>
        public int requestType { get; set; }

        // The request type of the plan
        public int warehouseID { get; set; }
        public string farmerComment { get; set; }
        /// <summary>
        /// 0: partial payment: 5/95
        /// 1: full payment: 100%
        /// </summary>
        public int? modeOfPayment { get; set; } = 0;
    }

    /// <summary>
    /// Data Transfer Object (DTO) for editing a farming plan.
    /// </summary>
    public class EditPlanDTO
    {
        // The ID of the plan
        public int planID { get; set; }

        // The ID of the farm
        public int farmID { get; set; }

        // The ID of the season
        public int seasonID { get; set; }

        // The ID of the warehouse
        public int warehouseID { get; set; }

        // List of crops in the plan
        public List<PlanCrop_Req> crops { get; set; } = new List<PlanCrop_Req>();

        /// <summary>
        /// 1 Pending 
        /// 2 Approved 
        /// 3 Exception 
        /// 4 Requested To Edit
        /// </summary>
        public int requestType { get; set; }
        public string farmerComment { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for getting plans.
    /// </summary>
    public class getPlans_Resp
    {
        // The ID of the plan
        public string planID { get; set; } = "";

        // The name of the season
        public string season { get; set; } = "";

        // The name of the farm
        public string farm { get; set; } = "";

        // The acreage of the farm
        public string farmAcreage { get; set; } = "";

        // The status ID of the plan
        public int statusID { get; set; }

        // The plan change status ID of the plan
        public int planChangeStatusID { get; set; }

        // The reverted reason of the plan
        public string reason { get; set; } = "";
        public bool canRequestForChanges { get; set; }
        public bool canDelete { get; set; }
        public bool canSubmitForApproval { get; set; }
        public bool canViewOrder { get; set; }
        public bool canOrderPayment { get; set; }
        public bool isSummaryAvailable { get; set; } = false;
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for getting a plan order.
    /// </summary>
    public class getPlanOrder_Req
    {
        // The ID of the plan
        public string planID { get; set; } = "";
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for getting a plan order.
    /// </summary>
    public class getPlanOrder_Resp
    {
        // The ID of the plan
        public string planID { get; set; } = "";

        // The name of the season
        public string seasonName { get; set; } = "";

        // The name of the farm
        public string farmName { get; set; } = "";

        // The advance amount
        public string advanceAmount { get; set; } = "";

        // The advance percentage
        public string advancePercent { get; set; } = "";

        // The advance payment status
        public int advancePaymentStatus { get; set; }
        // The advance order status
        public int advanceOrderStatus { get; set; }

        // The advance payment order ID
        public Int64 advancePaymentOrderID { get; set; }

        // List of orders in the plan
        public List<getPlanOrder> advanceReconciledOrders { get; set; } = new List<getPlanOrder>();
        public List<getPlanOrder> productReconciledOrders { get; set; } = new List<getPlanOrder>();
        public List<getPlanOrder> orders { get; set; } = new List<getPlanOrder>();
        public bool canPay { get; set; } = false;
        public string advancestatusDescription { get; set; } = "";
    }

    /// <summary>
    /// Data Transfer Object (DTO) for a plan order.
    /// </summary>
    public class getPlanOrder
    {
        // The SAP order number of the order
        public string sapOrderID { get; set; } = "";
        // The order ID of the order
        public Int64 orderID { get; set; }
        // The date of the order
        public DateTime? date { get; set; }

        // List of products in the order
        public List<getOrderProduct> products { get; set; } = new List<getOrderProduct>();

        // The total number of bags in the order
        public string totalBags { get; set; } = "0";

        // The status ID of the order
        public int statusID { get; set; }

        // The payment ID of the order
        public int paymentStatusID { get; set; }
        public bool canPay { get; set; }
        public bool canViewAuthorityLetter { get; set; }
        public DateTime duePaymentDate { get; set; }
        public string paymentStatusDescription { get; set; } = "";
        public string orderStatusDescription { get; set; } = "";
        public string deliveryStatusDescription { get; set; } = "";
        public string closingQty { get; set; }
        public bool isProductVisible { get; set; } = false;
        public decimal orderPrice { get; set; } = 0.0m;
        public decimal approvalDatePrice { get; set; } = 0.0m;
        public decimal paymentDatePrice { get; set; } = 0.0m;
        public decimal reconciliationPrice { get; set; } = 0.0m;
        public string balanceQTY { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data Transfer Object (DTO) for a product in an order.
    /// </summary>
    public class getOrderProduct
    {
        // The name of the product
        public string productName { get; set; } = "";

        // The bag information
        public string bag { get; set; } = "";
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for updating plan status.
    /// </summary>
    public class updatePlanStatus_Req
    {
        // The ID of the plan
        public string planID { get; set; }

        // The status ID to be updated
        public int statusID { get; set; }

        // The message to be updated
        public string? message { get; set; }
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for updating plan status internally.
    /// </summary>
    public class updatePlanStatus_Internal_Req
    {
        // The ID of the user
        public string userID { get; set; }

        // The ID of the plan
        public int planID { get; set; }

        // The status ID to be updated
        public int statusID { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for Plan Summary.
    /// </summary>
    /// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Crop
    {
        public int cropGroupID { get; set; }
        //public int cropID { get; set; }
        //public string crop { get; set; }
        public List<planCropGroup_get> crops { get; set; }
        public int acreage { get; set; }
        public List<getMonthWiseProductCount> products { get; set; } = new List<getMonthWiseProductCount>();
    }
    public class getMonthWiseProductCount
    {
        public int monthID { get; set; }
        public decimal totalProducts { get; set; }
    }



    public class Product_DTO
    {
        //public int monthID { get; set; }
        public int productID { get; set; }
        public string product { get; set; }
        public List<Month> months { get; set; } = new List<Month>();
    }

    public class planSummary
    {
        public int seasonID { get; set; }
        public string season { get; set; }
        public List<getMonths> months { get; set; } = new List<getMonths>();
        public List<Crop> crops { get; set; } = new List<Crop>();
        public List<Product_DTO> products { get; set; } = new List<Product_DTO>();
        public string? warehouseLocation { get; set; }
        public string? warehouseDistance { get; set; }
        public bool isCropsAvailable { get; set; } = false;
    }
    public class dashboardPlan
    {
        public int advancePercentage { get; set; }
        public List<string> pickupDates { get; set; }
    }
    public class getPlanDetail_Resp
    {
        public int planID { get; set; }
        public string planner { get; set; }
        public int seasonID { get; set; }
        public string season { get; set; }
        public int farmID { get; set; }
        public string farm { get; set; }
        public string warehouse { get; set; }
        public string warehouseDistance { get; set; }
        public int warehouseID { get; set; }
        public EPlanStatus status { get; set; }
        public string reason { get; set; }
        public string farmerComment { get; set; }
        public List<planCrops_getPlanDetail> crops { get; set; } = new List<planCrops_getPlanDetail>();
        public EPlanChangeRequest changeRequestStatus { get; set; }
        public bool isEmptyCropsAllowed { get; set; }
    }
    public class planCropGroup_get
    {
        public int cropID { get; set; }
        public string cropName { get; set; }
        public string filePath { get; set; }
        public List<ConsumptionMatrixDTO> suggestion { get; set; } = new List<ConsumptionMatrixDTO>(); // Property for suggestion list
    }
    public class planCrops_getPlanDetail
    {
        public int planCropID { get; set; }
        //public int cropID { get; set; }
        //public string crop { get; set; }
        //public string imagePath { get; set; }
        public List<planCropGroup_get> cropsGroup { get; set; }
        public int cropGroupID { get; set; }
        public List<int> cropIDs { get; set; } = [];
        public int acreage { get; set; }
        public bool hasException { get; set; }

        //public List<ConsumptionMatrixDTO> suggestion { get; set; } = new List<ConsumptionMatrixDTO>(); // Property for suggestion list
        public List<cropProduct_planCrops_getPlanDetail> products { get; set; } = new List<cropProduct_planCrops_getPlanDetail>();
        public List<cropService_planCrops_getPlanDetail> services { get; set; } = new List<cropService_planCrops_getPlanDetail>();
        public int totalProducts { get; set; }
    }
    public class cropProduct_planCrops_getPlanDetail
    {
        public int planProductID { get; set; }
        public int productID { get; set; }
        public string product { get; set; } = string.Empty;
        public string uom { get; set; } = string.Empty;
        public int qty { get; set; }
        public DateTime date { get; set; }
    }
    public class cropService_planCrops_getPlanDetail
    {
        public int planServiceID { get; set; }
        public int serviceID { get; set; }
        public string? service { get; set; }
        public DateTime lastHarvestDate { get; set; }
        public DateTime landPreparationDate { get; set; }
        public DateTime sewingDate { get; set; }

    }
    public class Employee_getPlanReq
    {
        /// <summary>
        /// acreage-asc
        /// acreage-desc
        /// season-asc
        /// season-desc
        /// createdOn-asc
        /// createdOn-desc
        /// district-asc
        /// district-desc
        /// </summary>
        public string? orderBy { get; set; } = string.Empty;
        public int skip { get; set; }
        public int take { get; set; }
        /// <summary>
        /// 1 Pending
        /// 2 Approved 
        /// 3 Exception 
        /// 4 Requested To Edit
        /// </summary>
        public int requestTypeID { get; set; }
    }
    public class PlanWarehouseChangeRequest
    {
        public string? orderBy { get; set; } = string.Empty;
        public string? search { get; set; } = string.Empty;
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;
        /// <summary>
        /// 1   Default / Approved / Declined
        /// 2   RSM Approval
        /// 3   NSM Approval
        /// </summary>
        public int requestTypeID { get; set; }
    }
    public class PlanWarehouseUpdateRequest
    {
        public int planID { get; set; }
        public int statusID { get; set; }
        public string? reason { get; set; } = string.Empty;
    }
    public class Employee_getPlansDTO
    {
        public int planID { get; set; }
        public string viewablePlanID { get; set; }
        public string season { get; set; }
        public string farm { get; set; }
        public string farmer { get; set; }
        public string status { get; set; }
        public int statusID { get; set; }
    }
    public class Employee_getPlanDetailReq
    {
        public int planID { get; set; }
    }
    public class Employee_getPlanDetailResp
    {
        public int planID { get; set; }
        public string season { get; set; }
        public string pickupLocation { get; set; }
        public List<planCrops_getPlanDetail> crop { get; set; }
        public Employee_getPlanDetail_Farm farm { get; set; }
        public farmerProfileDTO farmer { get; set; }
        public string reason { get; set; }
        public bool canEndorse { get; set; } = false;
        public bool canPerformAction { get; set; } = false;
        public bool isPaid { get; set; } = false;
        public string farmerComment { get; set; } = "";
        public List<DropDownValues> planApprovalRejectionType { get; set; } = new List<DropDownValues>();
        public bool isSummaryAvailable { get; set; } = false;
        public string warehouseChangeRequestReason { get; set; } = string.Empty;
        public int warehouseChangeRequestStatusID { get; set; } = 0;
    }
    public class Employee_getPlanDetail_Farm
    {
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string tehsil { get; set; }
    }
    public class updatePlanStatusReq
    {
        public int planID { get; set; }
        /// <summary>
        /// 1 Approve
        /// 2 Endorse
        /// 3 Send Back
        /// 4 Reject
        /// </summary>
        public int statusID { get; set; }
        public string? reason { get; set; }
        public List<PlanCropsReq> planCrops { get; set; }
        public EPlanApprovalRejectionType planApprovalRejectionType { get; set; } = 0;

    }
    public class PlanCropsReq
    {
        public int planCropID { get; set; }
        public bool hasException { get; set; }
    }
    public class Employee_planChangeReq
    {
        public int planID { get; set; }
        public int statusID { get; set; }
    }
    public class getPlan_Req
    {
        /// <summary>
        /// 0 All 
        /// 1 To Pay
        /// 2 To Pick up
        /// 3 Pending for approval
        /// 4 Change Request is in pending
        /// 5 Draft 
        /// </summary>
        public int requestTypeID { get; set; }
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;
    }
    public class GetSeasonProductResponse
    {
        public string season { get; set; }
        public string product { get; set; }
        public int planned { get; set; }
        public int paid { get; set; }
        public int shipped { get; set; }
        public int toBeShipped { get; set; }
        public int toBePaid { get; set; }
    }

}
