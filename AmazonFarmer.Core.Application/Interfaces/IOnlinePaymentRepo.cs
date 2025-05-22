using AmazonFarmer.Core.Application.DTOs; // Importing DTOs (Data Transfer Objects) for use in this interface
using AmazonFarmer.Core.Domain.Entities; // Importing domain entities for use in this interface

namespace AmazonFarmer.Core.Application.Interfaces
{
    // Interface for handling online payment repository operations
    public interface IOnlinePaymentRepo
    {
        // Method to add a bill payment request to the repository
        Task<int> addBillPaymentRequest(BillPaymentRequest request, string username, string password);

        // Method to add a bill payment response to the repository
        Task<int> addBillPaymentResponse(BillPaymentResponse response);

        // Method to add a bill inquiry response to the repository
        Task<int> addBillInquiryResponse(BillInquiryResponse response, string consumer_number);

        // Method to add a bill inquiry request to the repository
        Task<int> addBillInquiryRequest(BillInquiryRequest request, string username, string password);

        // Method to get a transaction by its order ID from the repository
        Task<tblTransaction> getTransactionByOrderID(Int64 OrderID);

        // Method to add transaction data to the repository
        Task AddTransactionData(tblTransaction transaction);

        // Method to get a Bill Payment request Data for duplication from the repository
        Task<bool> getDuplicateBillPaymentRequest(string ConsumerNo, string AuthID, string TranDate, string TranTime,int CurrentBillPaymentID);

        Task<tblTransaction?> getTransactionByTranAuthID(string Tran_Auth_ID, string consumerCode);
        Task<tblTransaction?> GetTransactionByID(int Id);
        Task<List<tblTransaction>> getAllPendingTransactions();
        IQueryable<tblTransaction> getTransactions();
        tblTransaction UpdateTransaction(tblTransaction transaction);
        void AddTransactionLog(tblTransactionLog log);
        Task<decimal> getOrderPriceByComsumerNumber(string consumerNumber);
    }
}

