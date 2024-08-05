using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IPlanRepo
    {
        Task<string> addPlan(PlanDTO req, string userID);
        Task<string> editPlan(EditPlanDTO req, string userID);
        Task<List<getPlans_Resp>> getPlansByUserIDandLanguageCode(string userID, string languageCode);
        Task<getPlanOrder_Resp> getPlanOrderByUserIDandLanguageCode(string userID, string languageCode, int planID);
        Task updatePlanStatus(updatePlanStatus_Internal_Req req);
    }
}
