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
    public class OrderRepo : IOrderRepo
    {
        private AmazonFarmerContext _context;

        public OrderRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        public async Task CreateOrder(int planID, string createdBy, EOrderType orderType, int parentOrderID)
        {
            var plan = await _context.Plan.Where(x => x.ID == planID).FirstOrDefaultAsync();

            TblOrders order = new TblOrders()
            {
                PlanID = plan.ID,
                OrderType = orderType,
                OrderStatus = EActivityStatus.Active,
                AdvancePercent = orderType == EOrderType.Advance ? 5 : 100,
                ParentOrderID = orderType == EOrderType.OrderReconcile ? parentOrderID : null,
                CreatedByID = createdBy,
                CreatedOn = DateTime.Now
            };
            var orderResp = _context.Orders.Add(order).Entity;
            _context.SaveChanges();



            if (orderType == EOrderType.Product)
            {

                // Assuming your DbContext is named ApplicationDbContext
                var products = from pp in _context.PlanProduct
                               join pc in _context.PlanCrop on pp.PlanCropID equals pc.ID
                               join ps in _context.PlanService on pc.ID equals ps.PlanCropID
                               where pc.PlanID == planID
                               group new { pp, ps } by new { pp.ProductID, ps.ServiceID } into grouped
                               select new
                               {
                                   ProductID = grouped.Key.ProductID,   
                                   ServiceID = grouped.Key.ServiceID,
                                   Qty = grouped.Sum(x => x.pp.Qty),
                                   amount = (10 * grouped.Sum(x => x.pp.Qty))
                               };


                foreach (var product in products)
                {
                    TblOrderProducts productReq = new TblOrderProducts()
                    {
                        OrderID = orderResp.OrderID,
                        ProductID = product.ProductID,
                        QTY = Convert.ToInt32(product.Qty),
                        Amount = product.amount
                    };
                }
            }
        }

        private async Task<decimal> getProductPriceByProductID(int productID)
        {

        }


    }
}
