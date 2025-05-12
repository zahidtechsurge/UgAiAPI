using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private AmazonFarmerContext _context;

        public OrderRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        //public async Task CreateOrder(tblPlan plan, string createdBy, EOrderType orderType, int parentOrderID)
        //{
        //    //var plan = await _context.Plan.Where(x => x.ID == planID).FirstOrDefaultAsync();

        //    TblOrders order = new TblOrders()
        //    {
        //        PlanID = plan.ID,
        //        OrderType = orderType,
        //        OrderStatus = EOrderStatus.Active,
        //        PaymentStatus = EOrderPaymentStatus.NonPaid,
        //        AdvancePercent = orderType == EOrderType.Advance ? 5 : 100,
        //        ParentOrderID = orderType == EOrderType.OrderReconcile ? parentOrderID : null,
        //        CreatedByID = createdBy,
        //        CreatedOn = DateTime.UtcNow
        //    };
        //    var orderResp = _context.Orders.Add(order).Entity;
        //    _context.SaveChanges();



        //    if (orderType == EOrderType.Product)
        //    {

        //        // Assuming your DbContext is named ApplicationDbContext
        //        var products = from pp in _context.PlanProduct
        //                       join pc in _context.PlanCrop on pp.PlanCropID equals pc.ID
        //                       join ps in _context.PlanService on pc.ID equals ps.PlanCropID
        //                       where pc.PlanID == plan.ID
        //                       group new { pp, ps } by new { pp.ProductID, ps.ServiceID } into grouped
        //                       select new
        //                       {
        //                           ProductID = grouped.Key.ProductID,
        //                           ServiceID = grouped.Key.ServiceID,
        //                           Qty = grouped.Sum(x => x.pp.Qty),
        //                           amount = (10 * grouped.Sum(x => x.pp.Qty))
        //                       };


        //        foreach (var product in products)
        //        {
        //            TblOrderProducts productReq = new TblOrderProducts()
        //            {
        //                OrderID = orderResp.OrderID,
        //                ProductID = product.ProductID,
        //                QTY = Convert.ToInt32(product.Qty),
        //                Amount = product.amount
        //            };
        //        }
        //    }
        //}
        public TblOrders AddOrder(TblOrders order)
        {
            return _context.Orders.Add(order).Entity;
        }
        public async Task<TblOrders?> getOrderByOrderID(Int64 OrderID)
        {
            return await _context.Orders
                .Include(x => x.Warehouse)
                .Include(x => x.Transactions)
                .Include(x => x.Products).ThenInclude(u => u.Product).ThenInclude(p => p.UOM)
                .Include(x => x.User).ThenInclude(u => u.FarmerProfile)
                .Include(x => x.Plan).ThenInclude(p => p.OrderServices).ThenInclude(os => os.Service)
                .Include(p => p.Products).ThenInclude(x => x.PlanProduct)
                .Include(a=>a.AuthorityLetters).ThenInclude(d=>d.AuthorityLetterDetails)
                .Where(x => x.OrderID == OrderID).FirstOrDefaultAsync();
        }
        public async Task<TblOrders?> getOrderByOrderID(Int64 OrderID, string UserId)
        {
            return await _context.Orders
                .Include(x => x.Warehouse)
                .Include(x => x.Transactions)
                .Include(x => x.Products).ThenInclude(u => u.Product).ThenInclude(p => p.UOM)
                .Include(x => x.User).ThenInclude(u => u.FarmerProfile)
                .Include(x => x.Plan).ThenInclude(p => p.OrderServices).ThenInclude(os => os.Service)
                .Include(p => p.Products).ThenInclude(x => x.PlanProduct)
                .Where(x => x.OrderID == OrderID && x.CreatedByID == UserId).FirstOrDefaultAsync();
        }
        public async Task<List<TblOrders>> getAllOrderByPlanID(int PlanID, string CreatedBy)
        {
            return await _context.Orders
                .Include(x => x.Products).ThenInclude(op => op.Product).ThenInclude(p => p.UOM)
                .Include(x => x.Products).ThenInclude(op => op.PlanProduct).ThenInclude(pp => pp.Product)
                .Include(x => x.Warehouse)
                .Include(x => x.User).ThenInclude(u => u.FarmerProfile)
                .Include(x => x.Plan).ThenInclude(u => u.OrderServices).ThenInclude(os => os.Service)
                .Where(x => x.PlanID == PlanID && x.OrderStatus != EOrderStatus.Deleted)
                .ToListAsync();
        }
        public async Task UpdateOrder(TblOrders Order)
        {
            _context.Orders.Update(Order);
        }
        public async Task<List<getNearestPickupDates>> getNearestPickupDatesByUserID(string userID)
        {
            return await _context.Orders
                .Where(x =>
                x.CreatedByID == userID &&
                x.OrderStatus == EOrderStatus.Active &&
                x.PaymentStatus == EOrderPaymentStatus.Paid &&
                x.DeliveryStatus != EDeliveryStatus.ShipmentComplete &&
                (x.OrderType != EOrderType.Advance && x.OrderType != EOrderType.AdvancePaymentReconcile)
                )
                .OrderBy(x => x.ExpectedDeliveryDate)
                .Take(3)
                .Select(x => new getNearestPickupDates
                {
                    pickupText = (x.OrderName + " Pickup"),
                    pickupDate = x.ExpectedDeliveryDate//ToString("yyyy-MM-dd")
                })
                .ToListAsync();
        }

        public async Task<List<getNearestPayableOrders>> getNearestPayableOrdersByUserID(string userID)
        {
            return await _context.Orders
                .Where(x =>
                    x.CreatedByID == userID &&
                    x.OrderStatus == EOrderStatus.Active &&
                    x.PaymentStatus == EOrderPaymentStatus.NonPaid
                )
                .OrderBy(x => x.DuePaymentDate)
                .Take(3)
                .Select(x => new getNearestPayableOrders
                {
                    orderText = x.OrderType == EOrderType.Advance ? (EOrderType.Advance.ToString() + " " + x.AdvancePercent + "%") : (x.OrderName + " Pay"),
                    orderDate = x.DuePaymentDate//ToString("yyyy-MM-dd")
                }).ToListAsync();
        }

        public TblOrderProducts UpdateOrderProduct(TblOrderProducts orderProduct)
        {
            return _context.OrderProducts.Update(orderProduct).Entity;
        }
        public async Task<List<TblOrders>> getOrdersByPlanID(int planID)
        {
            return await _context.Orders.Where(x => x.PlanID == planID).ToListAsync();
        }

        public async Task<TblOrders> getOrderByID(Int64 orderID, string userID, string languageCode)
        {
            return await _context.Orders
                .Include(x=>x.Warehouse).ThenInclude(x=>x.WarehouseIncharge)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.AuthorityLetters).ThenInclude(x => x.AuthorityLetterDetails)
                .Where(x => x.OrderID == orderID && x.CreatedByID == userID && (x.OrderStatus != EOrderStatus.Blocked && x.OrderStatus != EOrderStatus.Deleted))
                .FirstOrDefaultAsync();
        }
        public async Task<TblOrders> getOrderByID(Int64 orderID)
        {
            return await _context.Orders
                .Include(x => x.User).ThenInclude(x => x.FarmerProfile)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductTranslations)
                .Include(x => x.Plan).ThenInclude(x => x.Farm)
                .Where(x => x.OrderID == orderID && (x.OrderStatus != EOrderStatus.Blocked && x.OrderStatus != EOrderStatus.Deleted))
                .FirstOrDefaultAsync();
        }

        public void AddOrderService(TblOrderService orderService)
        {
            _context.OrderServices.Add(orderService);
        }

        public void UpdateOrderService(TblOrderService orderService)
        {
            _context.OrderServices.Update(orderService);
        }

        public async Task<IQueryable<TblOrders>> getOrders()
        {
            return _context.Orders
                .Include(x => x.Products).ThenInclude(x => x.Product)
                .Include(x => x.Plan).ThenInclude(x => x.Farm).Where(x => x.isLocked != true && (x.OrderStatus != EOrderStatus.Blocked && x.OrderStatus != EOrderStatus.Deleted));
        }

        public void AddOrderLog(TblOrders order, string updatedBy)
        {
            TblOrderLog log = new()
            {
                DeliveryStatus = order.DeliveryStatus,
                OrderID = order.OrderID,
                OrderStatus = order.OrderStatus,
                OrderType = order.OrderType,
                PaymentStatus = order.PaymentStatus,
                UpdateBy = updatedBy,
                UpdateDate = DateTime.UtcNow
            };
            _context.OrderLogs.Add(log);
        }
        public async Task<List<SP_OrderDetailsResult>> Get_OrderDetailsResults(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm, int isDownload)
        {
            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder ?? "DESC");
            var sortColumnParam = new SqlParameter("@SortColumn", sortColumn ?? "OrderID");
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var SearchTerm = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);
            var Download = new SqlParameter("@Download", isDownload);
            var sql = @"
            EXEC sp_GetOrderDetails 
                @PageNumber, 
                @PageSize, 
                @SortColumn, 
                @SortOrder,
                @SearchTerm,
                @Download";
            return await _context.SP_OrderDetailsResult.FromSqlRaw(sql, pageNumberParam, pageSizeParam, sortColumnParam, sortOrderParam, SearchTerm, Download).ToListAsync();

        }
    }
}
