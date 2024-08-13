using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
}
