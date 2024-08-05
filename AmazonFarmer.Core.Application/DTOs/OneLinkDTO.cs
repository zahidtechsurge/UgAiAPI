using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class OneLinkDTO
    {
    }
    public class BillInquiryRequest
    {
        public string? order_id { get; set; }
        public string consumer_number { get; set; }
        public string? prefix { get; set; }
        public string bank_mnemonic { get; set; }
        public string reserved { get; set; }
    }
    public class BillInquiryResponse
    {
        public string response_Code { get; set; }
        public string consumer_Detail { get; set; }
        public string bill_status { get; set; }
        public string? due_date { get; set; }
        public string amount_within_dueDate { get; set; }
        public string amount_after_dueDate { get; set; }
        public string billing_month { get; set; }
        public string? date_paid { get; set; }
        public string amount_paid { get; set; }
        public string tran_auth_Id { get; set; }
        [JsonIgnore]
        public int BillInquiryRequestID { get; set; }
        public string reserved { get; set; }
    }
    public class BillPaymentRequest
    {
        public string? orderid { get; set; }
        public string? prefix { get; set; }
        public string consumer_number { get; set; }
        public string tran_auth_id { get; set; }
        public string Transaction_amount { get; set; }
        public string tran_date { get; set; }
        public string tran_time { get; set; }
        public string bank_mnemonic { get; set; }
        public string reserved { get; set; }

    }
    public class BillPaymentResponse
    {
        public string response_Code { get; set; }
        public string Identification_parameter { get; set; }
        public string reserved { get; set; }
        [JsonIgnore]
        public int BillPaymentRequestID { get; set; }

    }
}
