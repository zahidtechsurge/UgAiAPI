using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IAuthorityLetterRepo // Defining the interface for handling authority letters
    {
        Task<IQueryable<TblAuthorityLetters>> GetAuthorityLetterLists(); // Method signature for getting a list of authority letters
        Task<int> GetAuthorityLetterHexCount(); // Method signature for getting a count of authority letters hexs
        Task addAuthorityLetterHex(tblAuthorityLetter_Hexs req); // Method signature for adding authority letter hex.
        Task addAuthorityLetterByDetail(TblAuthorityLetterDetails req); // Method signature for adding authority letter.
        Task updateAuthorityLetter(TblAuthorityLetters req); // Method signature for updating the status of authority letter.
        Task<TblAuthorityLetters> getAuthorityLetterByID(int authorityID); // Method signature fetching authority letter by authority letter ID. 
        Task<TblAuthorityLetters> getAuthorityLetterByID(int letterID, string userID, string languageCode); // Method signature fetching authority letter by authority letter ID.
        Task<IQueryable<TblAuthorityLetters>> getAuthorityLetters(string userID, string languageCode); // Method signature fetching list of authority letter by userID and language Code.
        Task<IQueryable<TblAuthorityLetters>> getPendingAuthorityLettersByWarehouseInchangeID(string userID); // Method signature fetching list of authority letter by warehouse incharge userID.
        Task<List<authorityLetter_Invoice>> getAvailableInvoicesForOrder(string sapOrderID, List<authorityLetter_Invoice> invoices); // Method signature for removing consumed invoices by sapOrderID and list of invoices fetched from SAP.
    }
}
