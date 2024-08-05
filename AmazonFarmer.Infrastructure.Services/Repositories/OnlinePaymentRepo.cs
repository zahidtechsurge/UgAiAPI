using AmazonFarmer.Core.Application.DTOs; // Importing DTOs (Data Transfer Objects) for use in this class
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces; // Importing interfaces for use in this class
using AmazonFarmer.Core.Domain.Entities; // Importing domain entities for use in this class
using AmazonFarmer.Infrastructure.Persistence; // Importing the persistence layer for database access
using Microsoft.EntityFrameworkCore;
using System.Globalization; // Importing Entity Framework Core for database operations

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    // Repository class implementing the IOnlinePaymentRepo interface
    public class OnlinePaymentRepo : IOnlinePaymentRepo
    {
        private AmazonFarmerContext _context; // Database context for interacting with the database

        // Constructor to initialize the repository with the database context
        public OnlinePaymentRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to add a bill inquiry request to the database
        public async Task<int> addBillInquiryRequest(BillInquiryRequest request, string username, string password)
        {
            // Creating a new instance of tblBillingIquiryRequest entity
            tblBillingIquiryRequest billingIquiryRequest = new tblBillingIquiryRequest
            {
                BankMnemonic = request.bank_mnemonic,
                OrderID = request.order_id == null ? null : Convert.ToInt64(request.order_id),
                RequestTime = DateTime.Now,
                ConsumerCode = request.consumer_number,
                Prefix = request.prefix,
                Reserved = request.reserved,
                UserName = username,
                Password = password,
            };

            // Adding the new entity to the database context and saving changes
            billingIquiryRequest = _context.BillingIquiryRequest.Add(billingIquiryRequest).Entity;
            await _context.SaveChangesAsync();

            return billingIquiryRequest.Id;
        }

        // Method to add a bill inquiry response to the database
        public async Task<int> addBillInquiryResponse(BillInquiryResponse response, string consumer_number)
        {
            // Parsing the date paid from the response
            DateTime? TranDate = string.IsNullOrWhiteSpace(response.date_paid) ? null : DateTime.ParseExact(response.date_paid, "yyyyMMdd", null);

            // Creating a new instance of tblBillingIquiryResponse entity
            tblBillingIquiryResponse billInquiry = new tblBillingIquiryResponse
            {
                AmountAfterDueDate = response.amount_after_dueDate,
                AmountPaid = response.amount_paid,
                AmountWithInDueDate = response.amount_within_dueDate,
                BillingInquiryRequestID = response.BillInquiryRequestID,
                BillingMonth = response.billing_month,
                BillStatus = response.bill_status,
                ConsumerDetail = response.consumer_Detail,
                DatePaid = TranDate,
                DueDate = !string.IsNullOrEmpty(response.due_date)
                            ? (DateTime.TryParseExact(response.due_date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)
                                ? parsedDate
                                : (DateTime?)null)
                            : (DateTime?)null,
                Reserved = response.reserved,
                ResponseCode = response.response_Code,
                ResponseTime = DateTime.Now,
                TimePaid = TranDate,
                Tran_auth_ID = response.tran_auth_Id,
                ConsumerNumber = consumer_number
            };

            // Adding the new entity to the database context
            _context.BillingIquiryResponse.Add(billInquiry);
            return 0;
        }

        // Method to add a bill payment request to the database
        public async Task<int> addBillPaymentRequest(BillPaymentRequest request, string username, string password)
        {
            // Creating a new instance of tblBillingPaymentRequest entity
            tblBillingPaymentRequest billPaymentRequest = new tblBillingPaymentRequest
            {
                BankMemonic = request.bank_mnemonic,
                OrderID = request.orderid == null ? null : Convert.ToInt64(request.orderid),
                Reserved = request.reserved,
                consumer_number = request.consumer_number,
                prefix = request.prefix,
                UserName = username,
                Password = password,
                Amount = request.Transaction_amount,
                TransactionDate = request.tran_date,
                TransactionTime = request.tran_time,
                Tran_Auth_ID = request.tran_auth_id,
                RequestTime = DateTime.Now
            };

            // Adding the new entity to the database context and saving changes
            billPaymentRequest = _context.BillingPaymentRequest.Add(billPaymentRequest).Entity;
            await _context.SaveChangesAsync();

            return billPaymentRequest.Id;
        }

        // Method to add a bill payment response to the database
        public async Task<int> addBillPaymentResponse(BillPaymentResponse response)
        {
            // Creating a new instance of tblBillingPaymentResponse entity
            tblBillingPaymentResponse billPayment = new tblBillingPaymentResponse
            {
                BillingPaymentRequestID = response.BillPaymentRequestID,
                Identification_parameter = response.Identification_parameter,
                Reserved = response.reserved,
                ResponseTime = DateTime.Now,
                Response_Code = response.response_Code
            };

            // Adding the new entity to the database context
            _context.BillingPaymentResponse.Add(billPayment);
            return 1;
        }

        // Method to add transaction data to the database
        public async Task AddTransactionData(tblTransaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        // Method to retrieve a transaction by its order ID from the database
        public async Task<tblTransaction?> getTransactionByOrderID(Int64 OrderID)
        {
            return await _context.Transactions.Where(x => x.OrderID == OrderID).FirstOrDefaultAsync();
        }


        public async Task<bool> getDuplicateBillPaymentRequest(string ConsumerNo, string AuthID, string TranDate, string TranTime, int CurrentBillPaymentID)
        {
            return _context.BillingPaymentRequest.Any(x => x.consumer_number == ConsumerNo
                                                               && x.Tran_Auth_ID == AuthID
                                                               && x.TransactionDate == TranDate
                                                               && x.TransactionTime == TranTime
                                                               && x.Id != CurrentBillPaymentID);
        }



        // Method to retrieve a transaction by its order ID from the database
        public async Task<tblTransaction?> getTransactionByTranAuthID(string Tran_Auth_ID)
        {
            return await _context.Transactions
                .Include(t => t.Order).ThenInclude(o => o.User).ThenInclude(u => u.FarmerProfile)
                .Include(t => t.Order).ThenInclude(o => o.Plan).ThenInclude(p => p.OrderServices).ThenInclude(os => os.Service)
                .Include(t => t.Order).ThenInclude(o => o.Products).ThenInclude(op => op.Product).ThenInclude(p => p.UOM)
                .Include(t => t.Order).ThenInclude(o => o.Warehouse)
                .Where(x => x.Tran_Auth_ID == Tran_Auth_ID).FirstOrDefaultAsync();
        }

        public tblTransaction UpdateTransaction(tblTransaction transaction)
        {
            return _context.Transactions.Update(transaction).Entity;
        }
        public void AddTransactionLog(tblTransactionLog log)
        {
            _context.TransactionLogs.Add(log);
        }
        public async Task<decimal> getOrderPriceByComsumerNumber(string consumerNumber)
        {
            tblBillingIquiryResponse response = await _context.BillingIquiryResponse.Where(br => br.ConsumerNumber == consumerNumber)
                .OrderByDescending(br => br.BillingInquiryRequestID)
                .FirstOrDefaultAsync();

            if (response == null) throw new AmazonFarmerException("Invalid consumer number");

            return Convert.ToDecimal(response.AmountWithInDueDate) / 100;
        }
    }
}