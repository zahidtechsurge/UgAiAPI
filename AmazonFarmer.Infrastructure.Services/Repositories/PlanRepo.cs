using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class PlanRepo : IPlanRepo
    {
        private AmazonFarmerContext _context;
        /// <summary>
        /// This constructor initializes a new instance of the PlanRepo class with the provided AmazonFarmerContext.
        ///It sets the private _context field to the provided context.
        /// </summary>
        /// <param name="context"></param>
        public PlanRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /*
           This method adds a new plan based on the provided PlanDTO and user ID.
           It returns a string representing the ID of the newly created plan.
        */
        public async Task<tblPlan> addPlan(tblPlan plan)
        {
            return _context.Plan.Add(plan).Entity;
        }


        /*
           This method edits a plan based on the provided EditPlanDTO and user ID.
           It returns a string indicating the result of the operation.
        */
        public async Task<tblPlan> editPlan(tblPlan plan)
        {
            return _context.Plan.Add(plan).Entity;
        }


        /*
           This method retrieves plans by user ID and language code.
           It returns a list of getPlans_Resp instances.
        */
        public async Task<IQueryable<tblPlan>> getPlansByUserIDandLanguageCode(string userID, string languageCode)
        {
            return _context.Plan
                // Include related entities to be eager loaded
                .Include(x => x.Season)
                    .ThenInclude(x => x.SeasonTranslations.Where(x => x.LanguageCode == languageCode)) // filter season in the specified language
                .Include(x => x.Farm)
                .Include(x => x.PlanCrops)
                .Include(x => x.Orders.Where(x => x.OrderStatus == EOrderStatus.Active))
                // Filter plans based on user ID, language code, and status
                .Where(x => x.UserID == userID &&
                            x.Status != EPlanStatus.Removed); // Exclude plans with status Removed;
        }


        /*
           This method retrieves plan orders by user ID, language code, and plan ID.
           It returns an instance of getPlanOrder_Resp class.
        */
        public async Task<tblPlan> getPlanOrderByUserIDandLanguageCode(string userID, string languageCode, int planID)
        {

            return await _context.Plan
                .Include(x => x.Farm)
                .Include(x => x.Season)
                    .ThenInclude(x => x.SeasonTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.Orders.Where(x => x.OrderStatus != EOrderStatus.Deleted))
                    .ThenInclude(x => x.Products).ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductTranslations.Where(x => x.LanguageCode == languageCode))
                // Filter plans based on plan ID and user ID
                .Where(x => x.ID == planID && x.UserID == userID)
                .FirstOrDefaultAsync();

            //return await _context.Plan
            //    // Include related entities to be eager loaded
            //    .Include(x => x.Farm)
            //    .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
            //        .ThenInclude(x => x.PlanProducts.Where(x => x.Status == EActivityStatus.Active))
            //            .ThenInclude(x => x.Product)
            //                .ThenInclude(x => x.ProductTranslations.Where(x => x.LanguageCode == languageCode))
            //    .Include(x => x.Season)
            //        .ThenInclude(x => x.SeasonTranslations.Where(x => x.LanguageCode == languageCode))
            //    // Filter plans based on plan ID and user ID
            //    .Where(x => x.ID == planID && x.UserID == userID)
            //    .FirstOrDefaultAsync();
        }


        /*
           This method updates the status of a plan based on the provided request.
           It takes an instance of updatePlanStatus_Internal_Req class as a parameter.
        */
        public async Task updatePlan(tblPlan plan)
        {
            // Save the changes to the database
            _context.Plan.Update(plan);
        }

        ///<summary>
        ///add table of plan crop
        /// </summary>
        public async Task addPlanCrop(tblPlanCrops plancrop)
        {
            _context.PlanCrop.Add(plancrop);
        }
        ///<summary>
        ///update table of plan crop
        /// </summary>
        public async Task updatePlanCrop(tblPlanCrops plancrop)
        {
            _context.PlanCrop.Update(plancrop);
        }
        ///<summary>
        ///add table of plan product
        /// </summary>
        public async Task addPlanProduct(tblPlanProduct planProduct)
        {
            _context.PlanProduct.Add(planProduct);
        }
        ///<summary>
        ///update table of plan product
        /// </summary>
        public async Task updatePlanProduct(tblPlanProduct planProduct)
        {
            _context.PlanProduct.Update(planProduct);
        }
        ///<summary>
        ///add table of plan service
        /// </summary>
        public async Task addPlanService(tblPlanService service)
        {
            _context.PlanService.Add(service);
        }
        ///<summary>
        ///update table of plan service
        /// </summary>
        public async Task updatePlanService(tblPlanService service)
        {
            _context.PlanService.Update(service);
        }

        /*
            This method is created to get the plan based on planID
         */
        public async Task<tblPlan> getPlanByPlanID(int planID, string languageCode = "EN")
        {
            languageCode = string.IsNullOrEmpty(languageCode) ? "EN" : languageCode;

            return await _context.Plan
                .Include(x => x.Orders.Where(x => x.OrderStatus != EOrderStatus.Deleted))
                .Include(x => x.Season).ThenInclude(x => x.SeasonTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.PlanProducts.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.Product).ThenInclude(x => x.ProductTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.PlanProducts.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.Product).ThenInclude(x => x.UOM).ThenInclude(x => x.UnitOfMeasureTranslation.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.PlanServices.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(x => x.Service).ThenInclude(x => x.ServiceTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                //.ThenInclude(x => x.Crop)
                .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops).ThenInclude(x => x.Crop)
                .ThenInclude(x => x.CropTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                //.ThenInclude(x => x.Crop)
                .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops).ThenInclude(x => x.Crop)
                .ThenInclude(x => x.ProductConsumptionMetrics).ThenInclude(x => x.Product).ThenInclude(x => x.ProductTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.Farm).ThenInclude(x => x.City)
                .Include(x => x.Farm).ThenInclude(x => x.Tehsil)
                .Include(x => x.Farm).ThenInclude(x => x.District)
                .Include(x => x.User).ThenInclude(x => x.FarmerProfile).Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                //.ThenInclude(x => x.Crop)
                .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops).ThenInclude(x => x.Crop).ThenInclude(x => x.CropTranslations)
                .Include(x => x.Warehouse).ThenInclude(x => x.WarehouseTranslation.Where(pt => pt.LanguageCode == languageCode))
                .Where(x =>
                    x.ID == planID
                ).FirstOrDefaultAsync();
        }

        public async Task<tblPlan> getPlanForServiceReportByPlanID(int PlanID, string userID)
        {
            return await _context.Plan
                .Include(x => x.Farm)
                .ThenInclude(x => x.Users)
                .ThenInclude(x => x.FarmerProfile)
                .Where(x => x.UserID == userID && x.ID == PlanID)
                .FirstOrDefaultAsync();
        }

        /*
            This method is created to get the plan based on planID, languageCode, userID
         */
        public async Task<tblPlan> getPlanByPlanID(int planID, string userID, string languageCode = "EN")
        {
            return await _context.Plan
                .Include(x => x.Season).ThenInclude(x => x.SeasonTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.PlanProducts.Where(x => x.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.ProductTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.PlanProducts.Where(x => x.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.UOM)
                                .ThenInclude(x => x.UnitOfMeasureTranslation.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.PlanServices.Where(x => x.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.Service)
                            .ThenInclude(x => x.ServiceTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                        //.ThenInclude(x => x.Crop)
                        .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops).ThenInclude(x => x.Crop)
                        .ThenInclude(x => x.CropTranslations.Where(pt => pt.LanguageCode == languageCode /*&& pt.Status == EActivityStatus.Active*/))
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                        //.ThenInclude(x => x.Crop)
                        .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops).ThenInclude(x => x.Crop)
                        .ThenInclude(x => x.ProductConsumptionMetrics)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.ProductTranslations.Where(pt => pt.LanguageCode == languageCode))
                .Include(x => x.Farm).ThenInclude(x => x.City)
                .Include(x => x.Farm).ThenInclude(x => x.Tehsil)
                .Include(x => x.Farm).ThenInclude(x => x.District)
                .Include(x => x.User).ThenInclude(x => x.FarmerProfile)
                .Include(x => x.Orders)
                .Include(x => x.PlanCrops.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.CropGroup)
                        .ThenInclude(x => x.CropGroupCrops)
                .Include(x => x.Warehouse).ThenInclude(x => x.WarehouseTranslation.Where(pt => pt.LanguageCode == languageCode))
                .Where(x =>
                    x.ID == planID &&
                    x.UserID == userID
                ).FirstOrDefaultAsync();
        }

        /*
            This method is created to get the plan based on planID
         */
        public async Task<tblPlan> getPlanByPlanIDForApproval(int planID, List<int> territoryIds)
        {
            IQueryable<tblPlan> planQuery = _context.Plan
                    .Include(p => p.OrderServices)
                    .Include(x => x.Season)
                    .Include(x => x.PlanCrops.Where(pp => pp.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.PlanProducts.Where(pp => pp.Status == EActivityStatus.Active
                        && (pp.PaymentStatus != EOrderPaymentStatus.Refund && pp.DeliveryStatus != EDeliveryStatus.ShipmentComplete)))
                            .ThenInclude(pp => pp.Product).ThenInclude(x => x.UOM)
                    .Include(x => x.PlanCrops.Where(pp => pp.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.PlanServices).ThenInclude(x => x.Service)
                    .Include(x => x.PlanCrops.Where(pp => pp.Status == EActivityStatus.Active))
                        //.ThenInclude(x => x.Crop)
                        .ThenInclude(x => x.CropGroup).ThenInclude(x => x.CropGroupCrops)
                    .Include(x => x.Farm).ThenInclude(f => f.District)
                    .Include(p => p.User).ThenInclude(u => u.FarmerProfile)
                    .Include(w => w.Warehouse)
                    .Where(x => x.ID == planID);

            if (territoryIds.Count != 0)
            {
                planQuery = planQuery.Where(x => territoryIds.Contains(x.Farm.DistrictID));
            }

            return await planQuery.FirstOrDefaultAsync();

        }
        public async Task<List<TblOrders>> getOrdersForPlanForApproval(int planID)
        {
            IQueryable<TblOrders> orderQuery = _context.Orders
                        .Include(o => o.Products)
                        .Include(o => o.Products).ThenInclude(op => op.Product)
                        .Include(o => o.ChildOrders)
                    .Where(x => x.PlanID == planID && x.OrderStatus == EOrderStatus.Active
                        && (x.PaymentStatus != EOrderPaymentStatus.Refund));

            return await orderQuery.ToListAsync();
        }


        public async Task<int> getPlanCropIDByPlanProductID(int? planProductID)
        {
            int planCropID = await _context.PlanProduct.Where(pp => pp.ID == planProductID).Select(pp => pp.PlanCropID).FirstOrDefaultAsync();

            return planCropID;
        }
        /*
            This method is created to get the plan based on planID
         */
        public async Task<tblPlan> getPlanWithAllProducts(int planID)
        {
            return await _context.Plan
                .Include(x => x.PlanCrops)
                    .ThenInclude(x => x.PlanProducts.Where(pp => pp.PaymentStatus != EOrderPaymentStatus.Refund
                        && pp.DeliveryStatus != EDeliveryStatus.ShipmentComplete))
                .Where(x => x.ID == planID).AsNoTracking().FirstAsync();
        }
        public async Task<bool> HasPlanOrdersForCompletion(int planID)
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == EOrderStatus.Active
                    && o.PaymentStatus == EOrderPaymentStatus.Paid &&
                    o.DeliveryStatus != EDeliveryStatus.ShipmentComplete
                    && o.PlanID == planID &&
                    (o.OrderType != EOrderType.Advance && o.OrderType != EOrderType.AdvancePaymentReconcile))
                .AnyAsync();
        }
        /*
            This method is created to get the plan based on planID
         */
        public async Task<tblPlan> getPlanOrdersForRejectectionByPlanID(int planID, List<int> territoryIds)
        {

            IQueryable<tblPlan> planQuery = _context.Plan
                  .Include(x => x.Orders.Where(pp => pp.PaymentStatus != EOrderPaymentStatus.Refund
                      && pp.PaymentStatus != EOrderPaymentStatus.Forfeit
                      && pp.DeliveryStatus != EDeliveryStatus.ShipmentComplete
                      && pp.OrderStatus != EOrderStatus.Deleted
                      )
                  )
                  .ThenInclude(o => o.Products)
                  .Include(x => x.PlanCrops)
                      .ThenInclude(x => x.PlanProducts)
                   .Include(x => x.Farm)
                  .Where(x => x.ID == planID);

            if (territoryIds.Count != 0)
            {
                planQuery = planQuery.Where(x => territoryIds.Contains(x.Farm.DistrictID));
            }

            return await planQuery.FirstOrDefaultAsync();

        }

        public async Task<List<TblOrders>> getPlanOrdersByPlanIDPaidUnConsumed(int planID)
        {

            IQueryable<TblOrders> orderQuery = _context.Orders
                  .Where(x => x.PlanID == planID &&
                  (x.OrderType == EOrderType.Advance || x.OrderType == EOrderType.AdvancePaymentReconcile)
                  && !x.IsConsumed
                  && x.PaymentStatus == EOrderPaymentStatus.Paid
                  );


            return await orderQuery.ToListAsync();

        }

        public async Task<IQueryable<tblPlan>> getPlanList()
        {
            return _context.Plan.Include(x => x.User).Include(x => x.Farm).Include(x => x.Season);
        }
        public async Task<List<planCropGroup_get>> getCropInformationByCropGroupID(int cropGroupID, string languageCode, string baseFile)
        {

            // Retrieve the district ID of the farm



            var cropGroups = await _context.CropGroupCrops
                .Include(x => x.Crop)
                    .ThenInclude(x => x.CropTranslations/*.Where(y => y.Status == EActivityStatus.Active)*/)
                .Include(x => x.Crop)
                    .ThenInclude(x => x.ProductConsumptionMetrics)
                 .Where(g => g.CropGroupID == cropGroupID)
                 .ToListAsync();

            List<planCropGroup_get> cropInformation = new List<planCropGroup_get>();

            foreach (tblCropGroupCrops item in cropGroups)
            {
                if (item.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).Count() > 0)
                {
                    planCropGroup_get xyz = new planCropGroup_get()
                    {
                        cropID = item.CropID,
                        cropName = item.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Text,
                        filePath = string.Concat(baseFile, item.Crop.CropTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Image.Replace("/", "%2F").Replace(" ", "%20")),
                        suggestion = item.Crop.ProductConsumptionMetrics.Select(x => new ConsumptionMatrixDTO
                        {
                            name = x.Product.ProductTranslations
                                        .Where(x => x.LanguageCode == languageCode)
                                        .FirstOrDefault().Text,
                            qty = x.Usage,
                            uom = x.UOM
                        }).ToList()
                    };
                    cropInformation.Add(xyz);
                }
            }
            return cropInformation;
        }

        public async Task<int> getCropGroupIDByCropIDs(List<int> cropIDs)
        {
            int requestCropCount = cropIDs.Count();
            int? cropGroups = await _context.CropGroupCrops
                .Where(cg => cropIDs.Contains(cg.CropID))
                .GroupBy(cg => cg.CropGroupID)
                .Where(g => g.Count() == cropIDs.Count)
                .Where(g => g.Count() == _context.CropGroupCrops.Count(cg => cg.CropGroupID == g.Key))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (cropGroups != null && cropGroups != 0)
            {
                // Existing CropGroupID found
                return cropGroups.Value;
            }
            else
            {
                List<tblCropGroupCrops> cropGroupCrops = new List<tblCropGroupCrops>();

                foreach (int cropID in cropIDs)
                {
                    tblCropGroupCrops cropGroupCrop = new tblCropGroupCrops()
                    {
                        CropID = cropID,
                    };
                    cropGroupCrops.Add(cropGroupCrop);
                }

                tblCropGroup tblCropGroup = new tblCropGroup()
                {
                    Status = EActivityStatus.Active,
                    CropGroupCrops = cropGroupCrops
                };

                tblCropGroup = _context.CropGroup.Add(tblCropGroup).Entity;
                await _context.SaveChangesAsync();
                return tblCropGroup.ID;
            }

        }

        public async Task<IQueryable<tblPlan>> getPlanOrderServices(string userID)
        {
            // Query the Plan table, including related Orders and OrderServices
            return _context.Plan
                .Include(x => x.Orders)                // Include Orders navigation property
                .Include(x => x.OrderServices.Where(x => x.Status == EActivityStatus.Active && (x.ServiceID == (int)EServices.Drone_Footage || x.ServiceID == (int)EServices.Geofencing)))         // Include OrderServices navigation property
                .ThenInclude(x => x.Service)              // Then Services navifation property
                .ThenInclude(x => x.ServiceTranslations)              // Then Service translations navifation property
                .Where(x => x.Orders.Any(order =>
                    order.PaymentStatus != EOrderPaymentStatus.NonPaid && // Filter for paid orders
                    order.OrderType == EOrderType.Advance &&              // Filter for orders of type 'Advance'
                    !order.isLocked                                        // Exclude locked orders
                    ) &&
                    x.UserID == userID
                ); // Execute the query asynchronously and return the result as a list
        }

        public IQueryable<tblSeason> getSeasonProductReport()
        {
            //var s = _context.Products.Include(x => x.PlanProducts).ThenInclude(x => x.PlanCrop).ThenInclude(x => x.Plan).ThenInclude(x => x.Season);
            return _context.Season.Include(x => x.plans).ThenInclude(x => x.Orders).ThenInclude(x => x.Products);
        }
        public async Task<List<PlanStatusResult>> GetPlanStatusPagedAsync(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm, string userId)
        {
            var lst = new List<PlanStatusResult>();
            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder ?? "DESC");
            var sortColumnParam = new SqlParameter("@SortColumn", sortColumn ?? "Season");
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var SearchTerm = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);

            var sql = @"
            EXEC GetPlanStatusPaged 
                @PageNumber, 
                @PageSize, 
                @SortColumn, 
                @SortOrder,
                @SearchTerm";
            //return lst;
            return await _context.SP_PlanStatusResult.FromSqlRaw(sql, pageNumberParam, pageSizeParam, sortColumnParam, sortOrderParam, SearchTerm).ToListAsync();
        }
        public async Task<List<PlanSeasonCropResult>> GetPlanSeasonCropPagedAsync(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm, string userId)
        {
            var lst = new List<PlanSeasonCropResult>();
            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder ?? "DESC");
            var sortColumnParam = new SqlParameter("@SortColumn", sortColumn ?? "Season");
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var SearchTerm = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);
            var UserID = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(userId) ? "" : userId);
            var sql = @"
            EXEC GetPlanCropPaged 
                @PageNumber, 
                @PageSize, 
                @SortColumn, 
                @SortOrder,
                @SearchTerm,
                @UserID";
            //return lst;
            return await _context.SP_PlanSeasonCropResult.FromSqlRaw(sql, pageNumberParam, pageSizeParam, sortColumnParam, sortOrderParam, SearchTerm, userId).ToListAsync();
        }
        public async Task<List<SP_FarmerDetailsResult>> GetSP_FarmerDetailsResult(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm)
        {
            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder ?? "DESC");
            var sortColumnParam = new SqlParameter("@SortColumn", sortColumn ?? "FarmerName");
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var SearchTerm = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);
            var sql = @"
            EXEC sp_GetFarmerDetails 
                @PageNumber, 
                @PageSize, 
                @SortColumn, 
                @SortOrder,
                @SearchTerm";
            //return lst;
            return await _context.SP_FarmerDetailsResult.FromSqlRaw(sql, pageNumberParam, pageSizeParam, sortColumnParam, sortOrderParam, SearchTerm).ToListAsync();
        }
    }
}
