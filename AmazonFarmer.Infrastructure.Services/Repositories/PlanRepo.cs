using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
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

        public PlanRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<string> addPlan(PlanDTO req, string userID)
        {
            var Farm = await _context.Farms.Where(x =>
                x.FarmID == req.farmID &&
                x.UserID == userID &&
                x.Status == EFarmStatus.Active &&
                x.isApproved == true
            ).FirstOrDefaultAsync();

            if (Farm == null)
                throw new Exception(_exceptions.farmNotAuthorized);

            tblPlan planReq = new tblPlan()
            {
                FarmID = req.farmID,
                SeasonID = req.seasonID,
                UserID = userID,
                Status = (ERequestType)req.requestType
            };
            var plan = _context.Plan.Add(planReq).Entity;
            _context.SaveChanges();
            foreach (var item in req.crops)
            {
                tblPlanCrops cropReq = new tblPlanCrops()
                {
                    PlanID = plan.ID,
                    CropID = item.cropID,
                    Acre = item.crop_acreage
                };

                var crop = _context.PlanCrop.Add(cropReq).Entity;
                _context.SaveChanges();

                foreach (var product in item.products)
                {
                    tblPlanProduct productReq = new tblPlanProduct()
                    {
                        PlanCropID = crop.ID,
                        ProductID = product.productID,
                        Date = DateTime.ParseExact(product.deliveryDate, "yyyy-MM-dd", null),
                        Qty = product.qty
                    };
                    _context.PlanProduct.Add(productReq);
                }

                foreach (var service in item.services)
                {
                    tblPlanService serviceReq = new tblPlanService()
                    {
                        PlanCropID = crop.ID,
                        ServiceID = service.serviceID,
                        StartDate = DateTime.ParseExact(service.startDate, "yyyy-MM-dd", null),
                        EndDate = DateTime.ParseExact(service.endDate, "yyyy-MM-dd", null),
                    };
                    _context.PlanService.Add(serviceReq);
                }

                _context.SaveChanges();
            }

            return plan.ID.ToString().PadLeft(10, '0');
        }

        public async Task<string> editPlan(EditPlanDTO req, string userID)
        {

            var Plan = await _context.Plan.Where(x => x.ID == req.planID && x.Status == ERequestType.Draft).FirstOrDefaultAsync();

            if (Plan == null)
                throw new Exception(_exceptions.planNotFound);

            var Farm = await _context.Farms.Where(x =>
                x.FarmID == req.farmID &&
                x.UserID == userID &&
                x.Status == EFarmStatus.Active &&
                x.isApproved == true
            ).FirstOrDefaultAsync();

            if (Farm == null)
                throw new Exception(_exceptions.farmNotAuthorized);

            Plan.FarmID = req.farmID;
            Plan.SeasonID = req.seasonID;
            Plan.Status = (ERequestType)req.requestType;
            //Plan.
            return "";
        }

        public async Task<List<getPlans_Resp>> getPlansByUserIDandLanguageCode(string userID, string languageCode)
        {
            var test = _context.Plan
                .Include(x => x.Season)
                .ThenInclude(x => x.SeasonTranslations)
                .Include(x => x.Farm)
                .Where(x => x.UserID == userID && x.Season.SeasonTranslations.Where(x => x.LanguageCode == languageCode).Count() > 0)
                .Select(x => new getPlans_Resp
                {
                    planID = x.ID.ToString().PadLeft(10, '0'),
                    farm = x.Farm.FarmName,
                    season = x.Season.SeasonTranslations.FirstOrDefault().Translation,
                    farmAcreage = x.Farm.Acreage.ToString("N2"),
                    statusID = (int)x.Status
                }).ToQueryString();


            return await _context.Plan
                .Include(x => x.Season)
                .ThenInclude(x => x.SeasonTranslations)
                .Include(x => x.Farm)
                .Where(x => x.UserID == userID && x.Season.SeasonTranslations.Where(x => x.LanguageCode == languageCode).Count() > 0 && x.Status != ERequestType.Removed)
                .Select(x => new getPlans_Resp
                {
                    planID = x.ID.ToString().PadLeft(10, '0'),
                    farm = x.Farm.FarmName,
                    season = x.Season.SeasonTranslations.FirstOrDefault().Translation,
                    farmAcreage = x.Farm.Acreage.ToString("N2"),
                    statusID = (int)x.Status
                }).ToListAsync();
        }

        public async Task<getPlanOrder_Resp> getPlanOrderByUserIDandLanguageCode(string userID, string languageCode, int planID)
        {
            return await _context.Plan
                .Include(x => x.Farm)
                .Include(x => x.PlanCrops)
                    .ThenInclude(x => x.PlanProducts)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.ProductTranslations)
                .Include(x => x.Season)
                    .ThenInclude(x => x.SeasonTranslations)
                .Where(x => x.ID == planID && x.UserID == userID)
                .Select(x => new getPlanOrder_Resp
                {
                    planID = x.ID.ToString().PadLeft(10, '0'),
                    farmName = x.Farm != null ? x.Farm.FarmName : null, // Null check
                    advancePercent = (5).ToString(),
                    advanceAmount = (5000).ToString(),
                    seasonName = x.Season != null ?
                        x.Season.SeasonTranslations
                            .Where(y => y.LanguageCode == languageCode)
                            .Select(y => y.Translation)
                            .FirstOrDefault() :
                        null, // Null check
                    orders = x.PlanCrops.Select(pc => new getPlanOrder
                    {
                        date = pc.PlanProducts.Where(x => x.PlanCropID == pc.ID).Select(x => x.Date).ToString(), // Assuming date is a DateTime property
                        statusID = 1,
                        totalBags = pc.PlanProducts.Where(x => x.PlanCropID == pc.ID).Select(x => x.Qty).Sum().ToString("N2"),
                        products = pc.PlanProducts != null ?
                            pc.PlanProducts.GroupBy(p => p.Date) // Group by date
                                .Select(group => new getOrderProduct
                                {
                                    productName = group.SelectMany(p => p.Product.ProductTranslations)
                                        .FirstOrDefault(t => t != null && t.LanguageCode == languageCode) != null ?
                                            group.SelectMany(p => p.Product.ProductTranslations)
                                                .FirstOrDefault(t => t.LanguageCode == languageCode).Text :
                                            null,
                                    bag = group.Sum(p => p.Qty).ToString("N2")
                                }).ToList() :
                            null // Null check
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
        public async Task updatePlanStatus(updatePlanStatus_Internal_Req req)
        {
            var plan = await _context.Plan.Where(x => x.ID == req.planID).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == req.userID).FirstOrDefaultAsync();
            if (plan == null)
                throw new Exception(_exceptions.planNotFound);
            else if (user == null)
                throw new Exception(_exceptions.userNotFound);
            else if (user.Designation == EDesignation.Employee && plan.Status != ERequestType.Active)
                throw new Exception(_exceptions.userNotAuthorized);
            else if (user.Designation == EDesignation.Farmer && plan.Status != ERequestType.Completed)
                throw new Exception(_exceptions.userNotAuthorized);
            else
            {
                plan.Status = (ERequestType)req.statusID;
                _context.Plan.Update(plan);
                _context.SaveChanges();
            }
        }
    }
}
