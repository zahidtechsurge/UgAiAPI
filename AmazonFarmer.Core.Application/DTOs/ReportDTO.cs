using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    [Keyless]
    public class PlanStatusResult
    {
        public int TotalRows { get; set; }
        public string Season { get; set; }
        public string Product { get; set; }
        public int PlannedPlan { get; set; }
        public int PaidPlan { get; set; }
        public int ShippedPlan { get; set; }
        public int ToBeShippedPlan { get; set; }
        public int ToBePaidPlan { get; set; }
    }
    public class PlanStatusResponse
    {
        public string season { get; set; } = string.Empty;
        public string product { get; set; } = string.Empty;
        public int plannedPlan { get; set; }
        public int paidPlan { get; set; }
        public int shippedPlan { get; set; }
        public int toBeShippedPlan { get; set; }
        public int toBePaidPlan { get; set; }
    }
    [Keyless]
    public class PlanSeasonCropResult
    {
        public int TotalRows { get; set; }
        public string SeasonName { get; set; }
        public string FarmName { get; set; }
        public string ProductName { get; set; }
        public string CropName { get; set; }
        public double Acre { get; set; }
        public int DeliveryMonth { get; set; }
        public int Bags { get; set; }
        public decimal Value { get; set; }
    }
    public class SeasonCropResponse
    {
        public string season { get; set; } = string.Empty;
        public string farm { get; set; } = string.Empty;
        public int acreage { get; set; }
        public int month { get; set; } //= string.Empty;
        public string crop { get; set; } = string.Empty;
        public string product { get; set; } = string.Empty;
        public int bag { get; set; }
        public decimal price { get; set; }
    }

    [Keyless]
    public class SP_FarmerDetailsResult
    {
        [Description("Total Rows")]
        public int TotalRows { get; set; }  // Assuming all rows are 172, but keeping it as int for versatility
        [Description("Farmer Name")]
        public string? FarmerName { get; set; } = string.Empty; // Names are stored as strings
        [Description("Farmer Code")]
        public string? FarmerCode { get; set; } = string.Empty;
        [Description("Farmer Email")]
        public string? FarmerEmail { get; set; } = string.Empty;
        [Description("Farmer Username")]
        public string? FarmerUsername { get; set; } = string.Empty;
        [Description("Farmer CNIC")]
        public string? FarmerCNIC { get; set; } = string.Empty; // CNIC numbers are stored as strings due to hyphens and leading zeros
        [Description("Farmer City")]
        public string? FarmerCity { get; set; } = string.Empty;
        [Description("Farmer Territory")]
        public string? FarmerTerritory { get; set; } = string.Empty;
        [Description("Farmer Region")]
        public string? FarmerRegion { get; set; } = string.Empty;
        [Description("Farmer Tehsil")]
        public string? FarmerTehsil { get; set; } = string.Empty;  // Optional, could be null
        [Description("Owned Land")]
        public string? OwnedLand { get; set; } = string.Empty;  // Land size is integer-based
        [Description("Leased Land")]
        public string? LeasedLand { get; set; }
        [Description("Total Land")]
        public int TotalLand { get; set; }
        [Description("No of farms")]
        public int NoofFarmsAdded { get; set; }
        [Description("Farm Name")]
        public string? FarmName { get; set; } = string.Empty;
        [Description("Farm ID")]
        public string? FarmID { get; set; } = string.Empty;
        [Description("Farm City")]
        public string? FarmCity { get; set; } = string.Empty;
        [Description("Farm Territory")]
        public string? FarmTerritory { get; set; } = string.Empty;
        [Description("Farm Region")]
        public string? FarmRegion { get; set; } = string.Empty;
        [Description("Farm Tehsil")]
        public string? FarmTehsil { get; set; } = string.Empty;
        [Description("Farm Acres")]
        public int FarmAcres { get; set; }
        [Description("Application Status")]
        public string? ApplicationStatus { get; set; } = string.Empty;
        [Description("TSO")]
        public string? TSO { get; set; } = string.Empty;
        [Description("RSM")]
        public string? RSM { get; set; } = string.Empty;
        [Description("Address 1")]
        public string Address1 { get; set; } = string.Empty;
        [Description("Address 2")]
        public string Address2 { get; set; } = string.Empty;
        [Description("Latitude")]
        public string latitude { get; set; } = string.Empty;
        [Description("Longitude")]
        public string longitude { get; set; } = string.Empty;
        [Description("Phone No")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Description("Application Submitted Date Time")]
        public DateTime? ApplicationSubmitDateTime { get; set; }  // Storing time as TimeSpan
        [Description("TSO Approval Date Time")]
        public DateTime? TSOApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
        [Description("RSM Approval Date Time")]
        public DateTime? RSMApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
    }
    public class SP_FarmerDetailsDownload
    {
        [Description("Farmer Name")]
        public string? FarmerName { get; set; } = string.Empty; // Names are stored as strings
        [Description("Farmer Code")]
        public string? FarmerCode { get; set; } = string.Empty;
        [Description("Farmer Email")]
        public string? FarmerEmail { get; set; } = string.Empty;
        [Description("Farmer Username")]
        public string? FarmerUsername { get; set; } = string.Empty;
        [Description("Farmer CNIC")]
        public string? FarmerCNIC { get; set; } = string.Empty; // CNIC numbers are stored as strings due to hyphens and leading zeros
        [Description("Farmer City")]
        public string? FarmerCity { get; set; } = string.Empty;
        [Description("Farmer Territory")]
        public string? FarmerTerritory { get; set; } = string.Empty;
        [Description("Farmer Region")]
        public string? FarmerRegion { get; set; } = string.Empty;
        [Description("Owned Land")]
        public string? OwnedLand { get; set; } = string.Empty;  // Land size is integer-based
        [Description("Leased Land")]
        public string? LeasedLand { get; set; }
        [Description("Total Land")]
        public int TotalLand { get; set; }
        [Description("No of farms")]
        public int NoofFarmsAdded { get; set; }
        [Description("Farm ID")]
        public string? FarmID { get; set; } = string.Empty;
        [Description("Farm Name")]
        public string? FarmName { get; set; } = string.Empty;
        [Description("Farm City")]
        public string? FarmCity { get; set; } = string.Empty;
        [Description("Farm Territory")]
        public string? FarmTerritory { get; set; } = string.Empty;
        [Description("Farm Region")]
        public string? FarmRegion { get; set; } = string.Empty;
        [Description("Farm Tehsil")]
        public string? FarmTehsil { get; set; } = string.Empty;
        [Description("Farm Acres")]
        public int FarmAcres { get; set; }
        [Description("Application Status")]
        public string? ApplicationStatus { get; set; } = string.Empty;
        [Description("TSO")]
        public string? TSO { get; set; } = string.Empty;
        [Description("RSM")]
        public string? RSM { get; set; } = string.Empty;
        [Description("Address 1")]
        public string Address1 { get; set; } = string.Empty;
        [Description("Address 2")]
        public string Address2 { get; set; } = string.Empty;
        [Description("Latitude")]
        public string latitude { get; set; } = string.Empty;
        [Description("Longitude")]
        public string longitude { get; set; } = string.Empty;
        [Description("Phone No")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Description("Application Submitted Date Time")]
        public DateTime? ApplicationSubmitDateTime { get; set; }  // Storing time as TimeSpan
        [Description("TSO Approval Date Time")]
        public DateTime? TSOApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
        [Description("RSM Approval Date Time")]
        public DateTime? RSMApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
    }
    public class FarmerDetailsResponse
    {
        public string farmerName { get; set; } = string.Empty;// Names are stored as strings
        public string farmerCode { get; set; } = string.Empty;
        public string? farmerEmail { get; set; } = string.Empty;
        public string? farmerUsername { get; set; } = string.Empty;
        public string farmerCNIC { get; set; } = string.Empty;// CNIC numbers are stored as strings due to hyphens and leading zeros
        public string farmerCity { get; set; } = string.Empty;
        public string farmerTerritory { get; set; } = string.Empty;
        public string farmerRegion { get; set; } = string.Empty;
        public string farmerTehsil { get; set; } = string.Empty; // Optional, could be null
        public string ownedLand { get; set; } = string.Empty; // Land size is integer-based
        public string leasedLand { get; set; } = string.Empty;
        public int totalLand { get; set; }
        public int noofFarmsAdded { get; set; }
        public string farmID { get; set; } = string.Empty;
        public string farmName { get; set; } = string.Empty;
        public string farmCity { get; set; } = string.Empty;
        public string farmTerritory { get; set; } = string.Empty;
        public string farmRegion { get; set; }
        public string farmTehsil { get; set; } = string.Empty;
        public int farmAcres { get; set; }
        public string applicationStatus { get; set; }
        public string tso { get; set; } = string.Empty;
        public string rsm { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? applicationSubmitDateTime { get; set; }  // Storing time as TimeSpan
        public DateTime? tsoApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
        public DateTime? rsmApprovalDateTime { get; set; }  // Nullable TimeSpan for optional values
    }
    [Keyless]
    public class SP_LogEntryResult
    {
        public int totalRows { get; set; }  // Total number of rows
        public int requestId { get; set; }  // ID of the request
        public string? requestHttpMethod { get; set; }  // HTTP method used (e.g., GET, POST, PUT)
        public string? requestURL { get; set; }  // URL of the request
        public string? requestBody { get; set; }  // Body of the request (as a string)
        public DateTime? requestTimestamp { get; set; }  // Timestamp of when the request was made
        public int? responseStatusCode { get; set; }  // HTTP status code of the response (e.g., 200, 404)
        public string? responseBody { get; set; }  // Body of the response (as a string)
        public DateTime? responseTimestamp { get; set; }  // Timestamp of when the response was generated
    }
    [Keyless]
    public class SP_OrderDetailsResult
    {
        public int totalRows { get; set; }
        public Int64 orderID { get; set; }
        public string? sapOrderID { get; set; } = string.Empty;
        public int orderType { get; set; }
        public int orderStatus { get; set; }
        public decimal orderAmount { get; set; }
        public DateTime createdOn { get; set; }
        public decimal? priceOnPayment { get; set; }
        public int paymentStatus { get; set; }
        public int deliveryStatus { get; set; }
        public int planID { get; set; }
        public string? seasonName { get; set; } = string.Empty;
        public string? warehouseName { get; set; } = string.Empty;
        public string? warehouseIncharge { get; set; } = string.Empty;
        public string farmerName { get; set; } = string.Empty;
        public string farmName { get; set; } = string.Empty;
        public string? address1 { get; set; } = string.Empty;
    }
    public class GetOrdersReponse
    {
        [Description("Order ID")]
        public string orderID { get; set; }
        [Description("SAP Order ID")]
        public string sapOrderID { get; set; } = string.Empty;
        [Description("Order Type")]
        public string orderType { get; set; }
        [Description("Order Status")]
        public string orderStatus { get; set; }
        [Description("Order Amount")]
        public decimal orderAmount { get; set; }
        [Description("Created On")]
        public DateTime createdOn { get; set; }
        [Description("Price On Payment")]
        public decimal? priceOnPayment { get; set; }
        [Description("Payment Status")]
        public string paymentStatus { get; set; }
        [Description("Delivery Status")]
        public string deliveryStatus { get; set; }
        [Description("Plan ID")]
        public string planID { get; set; }
        [Description("Season Name")]
        public string? seasonName { get; set; } = string.Empty;
        [Description("Warehouse Name")]
        public string? warehouseName { get; set; } = string.Empty;
        [Description("Warehouse Incharge Name")]
        public string? warehouseIncharge { get; set; } = string.Empty;
        [Description("Farmer Name")]
        public string farmerName { get; set; } = string.Empty;
        [Description("Farm Name")]
        public string farmName { get; set; } = string.Empty;
        [Description("Address 1")]
        public string? address1 { get; set; } = string.Empty;
    }
}
