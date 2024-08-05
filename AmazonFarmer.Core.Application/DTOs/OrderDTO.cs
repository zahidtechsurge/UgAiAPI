using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class OrderPaymentDetailRequest
    {
        public int PlanID { get; set; }
        public Int64 OrderID { get; set; }
    }
    public class PlaceOrderRequest
    {
        public Int64 OrderID { get; set; }
    }
    public class OrderPaymentInitiateRequest
    {
        public int PlanID { get; set; }
        public Int64 OrderID { get; set; }
        public string OneLinkTransactionID { get; set; }
    }
    public class OrderDoPaymentRequest
    {
        public int PlanID { get; set; }
        public Int64 OrderID { get; set; }
        public string OneLinkTransactionID { get; set; }
        public string SAPOrderID { get; set; }
        public string SAPTransactionID { get; set; }
        public string Tran_Auth_ID { get; set; }
    }
    public class PaymentAcknowledgmentRequest
    {
        public string Tran_Auth_ID { get; set; }
        public string ConsumerNumber { get; set; }
        public decimal Amount { get; set; }
    }
    public class ChangeCustomerPaymentRequest
    {
        public Int64 OrderID { get; set; }
    }
    public class OrderPaymentDetailResponse
    {
        public Int64 OrderID { get; set; }
        public string TransactionID { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public bool DoPlabceOrder { get; set; }
        public decimal AvailableBalance { get; set; }
    }
    public class getNearestPayableOrders
    {
        public string orderText { get; set; } = string.Empty;
        public DateTime orderDate { get; set; }
    }
    public class getNearestPickupDates
    {
        public string pickupText { get; set; } = string.Empty;
        public DateTime pickupDate { get; set; }
    }





    public partial class CustomerBalanceDTO
    {

        public decimal bILLING_VALField { get; set; }

        public decimal cREDIT_EXP_VALField { get; set; }

        public decimal dELIVERY_VALField { get; set; }

        public decimal CustomerBalance { get; set; }

        public decimal oPEN_INVOICE_VALField { get; set; }

        public decimal oPEN_ORDER_VALField { get; set; }

        public string? MessageType { get; set; }
        public string? Message { get; set; }
    }
    public class getBlockedOrders_Req
    {
        public int skip { get; set; }
        public int take { get; set; }
    }
    public class getBlockedOrders_Resp
    {
        public string orderID { get; set; }
        public string? product { get; set; }
        public int qty { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DateTime expiredDate { get; set; }
    }

    public class updateBlockedOrder_Req
    {
        public string orderID { get; set; }
        /// <summary>
        /// Date Format: yyyy-mm-dd
        /// </summary>
        public string nextExpiryDate { get; set; }
        /// <summary>
        /// 1 change Date
        /// 2 block Order
        /// </summary>
        public int statusID { get; set; }
    }

}
