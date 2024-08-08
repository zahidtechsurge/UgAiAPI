using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IPlanRepo // Defining the interface for plan repository
    {
        Task<tblPlan> addPlan(tblPlan plan); // Method signature for adding a plan
        Task<tblPlan> editPlan(tblPlan plan); // Method signature for editing a plan
        Task<IQueryable<tblPlan>> getPlansByUserIDandLanguageCode(string userID, string languageCode); // Method signature for retrieving plans by user ID and language code
        Task<tblPlan> getPlanOrderByUserIDandLanguageCode(string userID, string languageCode, int planID); // Method signature for retrieving plan order by user ID, language code, and plan ID
        Task updatePlan(tblPlan plan); // Method signature for updating plan
        Task addPlanCrop(tblPlanCrops plancrop); // Method signature for add plan crop
        Task updatePlanCrop(tblPlanCrops plancrop); // Method signature for updating plan crop
        Task addPlanProduct(tblPlanProduct product); // Method signature for adding plan product
        Task updatePlanProduct(tblPlanProduct product); // Method signature for updating plan product
        Task addPlanService(tblPlanService service); // Method signature for adding plan service
        Task updatePlanService(tblPlanService service); // Method signature for updating plan service
        Task<tblPlan> getPlanByPlanID(int planID, string languageCode); // Method signature for getting plan by planID
        Task<tblPlan> getPlanForServiceReportByPlanID(int PlanID, string userID);
        Task<tblPlan> getPlanByPlanID(int planID, string userID, string languageCode); // Method signature for getting plan by planID
        Task<tblPlan> getPlanByPlanIDForApproval(int planID, List<int> territoryIds);//Method to get basic plan details to create order
        Task<List<TblOrders>> getOrdersForPlanForApproval(int planID);
        Task<tblPlan> getPlanWithAllProducts(int planID);//Method to get basic plan details to create order
        Task<IQueryable<tblPlan>> getPlanList(); // Method signature to get plan list
        Task<tblPlan> getPlanOrdersForRejectectionByPlanID(int planID, List<int> territoryIds);

        Task<bool> HasPlanOrdersForCompletion(int planID);

        Task<List<TblOrders>> getPlanOrdersByPlanIDPaidUnConsumed(int planID);
        Task<int> getCropGroupIDByCropIDs(List<int> cropIDs);
        Task<List<planCropGroup_get>> getCropInformationByCropGroupID(int cropGroupID, string languageCode, string baseFile);
        Task<int> getPlanCropIDByPlanProductID(int? planProductID);
        Task<IQueryable<tblPlan>> getPlanOrderServices(string userID);
        IQueryable<tblSeason> getSeasonProductReport();
        Task<List<PlanStatusResult>> GetPlanStatusPagedAsync(int pageNumber, int pageSize, string sortColumn, string sortOrder);
    }
}
