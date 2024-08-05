using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IOrderRepo // Defining the interface for order repository
    {
        //Task CreateOrder(tblPlan plan, string createdBy, EOrderType orderType, int parentOrderID); // Method signature for creating an order
        TblOrders AddOrder(TblOrders order);
        void AddOrderService(TblOrderService orderService);
        void UpdateOrderService(TblOrderService orderService);
        TblOrderProducts UpdateOrderProduct(TblOrderProducts orderProduct);
        Task<TblOrders?> getOrderByOrderID(Int64 OrderID);
        Task<TblOrders?> getOrderByOrderID(Int64 OrderID, string UserId);
        Task<List<TblOrders>> getAllOrderByPlanID(int PlanID, string CreatedBy);  
        Task UpdateOrder(TblOrders order);
        void AddOrderLog(TblOrders order, string updatedBy);
        Task<List<getNearestPickupDates>> getNearestPickupDatesByUserID(string userID); // Method signature for getting the 2 Nearest Pickup Dates By FarmerID
        Task<List<getNearestPayableOrders>> getNearestPayableOrdersByUserID(string userID); // Method signature for getting the 3 Nearest Pay Dates By FarmerID
        Task<List<TblOrders>> getOrdersByPlanID(int planID); // Method signature for getting the orders by plan ID
        Task<TblOrders> getOrderByID(Int64 orderID, string userID, string languageCode); // Method signature for getting the orders by order ID and language code
        Task<TblOrders> getOrderByID(Int64 orderID); // Method signature for getting the orders by order ID
        Task<IQueryable<TblOrders>> getOrders(); // Method signature for getting orders



        //For Transaction REPO
    }
}
