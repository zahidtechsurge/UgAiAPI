using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class PlanDTO
    {
        public int farmID { get; set; }
        public int seasonID { get; set; }
        public List<PlanCrop_Req> crops { get; set; } = new List<PlanCrop_Req>();
        public int requestType { get; set; }
    }
    public class EditPlanDTO
    {
        public int planID { get; set; }
        public int farmID { get; set; }
        public int seasonID { get; set; }
        public List<PlanCrop_Req> crops { get; set; } = new List<PlanCrop_Req>();
        public int requestType { get; set; }
    }

    public class getPlans_Resp
    {
        public string planID { get; set; } = "";
        public string season { get; set; } = "";
        public string farm { get; set; } = "";
        public string farmAcreage { get; set; } = "";
        public int statusID { get; set; }
    }
    public class getPlanOrder_Req
    {
        public string planID { get; set; } = "";
    }
    public class getPlanOrder_Resp
    {
        public string planID { get; set; } = "";
        public string seasonName { get; set; } = "";
        public string farmName { get; set; } = "";
        public string advanceAmount { get; set; } = "";
        public string advancePercent { get; set; } = "";
        public List<getPlanOrder> orders { get; set; } = new List<getPlanOrder>();
    }
    public class getPlanOrder
    {
        public string date { get; set; } = "";
        public List<getOrderProduct> products { get; set; } = new List<getOrderProduct>();
        public string totalBags { get; set; } = "";
        public int statusID { get; set; }
    }
    public class getOrderProduct
    {
        public string productName { get; set; } = "";
        public string bag { get; set; } = "";
    }
    public class updatePlanStatus_Req
    {
        public string planID { get; set; }
        public int statusID { get; set; }
    }
    public class updatePlanStatus_Internal_Req
    {
        public string userID { get; set; }
        public int planID { get; set; }
        public int statusID { get; set; }
    }
}
