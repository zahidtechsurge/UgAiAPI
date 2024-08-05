using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblTransaction
    {
        [Key]
        public int Id { get; set; }
        public Int64 OrderID { get; set; }
        public string Prefix { get; set; }
        public string ConsumerCode { get; set; }
        public string Tran_Auth_ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime PaidTime { get; set; }
        public string SAPInvoiceNumber { get; set; }
        public string SAPOrderID { get; set; }
        public string? Reason { get; set; }
        public EOrderType OrderType { get; set; }
        public int? BillPaymentRequestID { get; set; }
        public ETransactionStatus TransactionStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [ForeignKey("OrderID")]
        public TblOrders Order { get; set; }
        [ForeignKey("BillPaymentRequestID")]
        public tblBillingPaymentRequest BillingPaymentRequest { get; set; }
        public List<tblTransactionLog> transactionLogs { get; set; }
    }
    public class tblTransactionLog
    {
        [Key]
        public int Id { get; set; }
        public int TransactionID { get; set; }
        public ETransactionStatus TransactionStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("TransactionID")]
        public tblTransaction Transaction { get; set; }

    }
}
