using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing authority letters in the database.
    /// </summary>
    public class AuthorityLetterRepo : IAuthorityLetterRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the AuthorityLetterRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public AuthorityLetterRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a queryable list of authority letters with their details and products.
        /// </summary>
        /// <returns>An IQueryable of TblAuthorityLetters.</returns>
        public async Task<IQueryable<TblAuthorityLetters>> GetAuthorityLetterLists()
        {
            return _context.AuthorityLetters
                .Include(x => x.Order)
                .Include(x => x.AuthorityLetterDetails)
                .ThenInclude(x => x.Products);
        }
        /// <summary>
        /// Retrieves a count of authority letters hexs
        /// </summary>
        /// <returns>An querty of tblAuthorityLetter_Hexs.</returns>
        public async Task<int> GetAuthorityLetterHexCount()
        {
            return await _context.AuthorityLetterHexs.AsNoTracking().CountAsync();
        }
        /// <summary>
        /// POST request for adding Hex on AuthorityLetterHexs
        /// </summary>
        /// <returns>Not returning anything</returns>
        public async Task addAuthorityLetterHex(tblAuthorityLetter_Hexs req)
        {
            await _context.AuthorityLetterHexs.AddAsync(req);
        }
        public async Task addAuthorityLetterByDetail(TblAuthorityLetterDetails req)
        {
            await _context.AuthorityLetterDetails.AddAsync(req);
        }
        public async Task updateAuthorityLetter(TblAuthorityLetters req)
        {
            _context.AuthorityLetters.Update(req);
        }
        public async Task<TblAuthorityLetters> getAuthorityLetterByID(int authorityID)
        {
            return await _context.AuthorityLetters
                .Include(x => x.AuthorityLetterDetails)
                    .ThenInclude(x => x.Products)
                        .ThenInclude(x => x.ProductTranslations)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Plan)
                        .ThenInclude(x => x.Farm)
                .Include(x=>x.Order).ThenInclude(x=>x.Products)
                .Include(x => x.CreatedBy)
                .Include(x=>x.Warehouse)
                .Include(x=>x.Attachment)
                .Where(x => x.AuthorityLetterID == authorityID).FirstOrDefaultAsync();
        }
        public async Task<TblAuthorityLetters> getAuthorityLetterByID(int letterID, string userID, string languageCode)
        {
            return await _context.AuthorityLetters
                .Include(x => x.AuthorityLetterDetails)
                    .ThenInclude(x => x.Products)
                        .ThenInclude(x => x.ProductTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.Order)
                    .ThenInclude(x => x.Plan)
                        .ThenInclude(x => x.Farm)
                .Include(x=>x.Order).ThenInclude(x=>x.Products)
                .Include(x => x.CreatedBy)
                .Include(x=>x.Warehouse)
                .Include(x=>x.Attachment)
                .Where(x => x.AuthorityLetterID == letterID && x.CreatedByID == userID).FirstOrDefaultAsync();
        }
        public async Task<IQueryable<TblAuthorityLetters>> getAuthorityLetters(string userID, string languageCode)
        {
            return _context.AuthorityLetters
                .Include(x => x.AuthorityLetterDetails)
                    .ThenInclude(x => x.Products)
                            .ThenInclude(x => x.ProductTranslations.Where(x=>x.LanguageCode == languageCode))
                .Include(x => x.Order).ThenInclude(x=>x.Products)
                .Include(x => x.Order).ThenInclude(x=>x.Warehouse).ThenInclude(x=>x.WarehouseIncharge)
                .Include(x => x.CreatedBy).Where(x=>x.CreatedByID == userID);
        }
        public async Task<List<authorityLetter_Invoice>> getAvailableInvoicesForOrder(string sapOrderID, List<authorityLetter_Invoice> invoices)
        {
            List<authorityLetter_Invoice> lst = new List<authorityLetter_Invoice>();
            List<string?> UsedInv = new List<string?>();
            UsedInv = await _context.AuthorityLetters
                .Include(x => x.Order)
                .Where(x => x.Order.SAPOrderID == sapOrderID && x.INVNumber != null)
                .Select(x => x.INVNumber)
                .ToListAsync();
            lst = invoices.Where(x => !UsedInv.Contains(x.invoiceNumber)).ToList();
            return lst;
        }
        public async Task<IQueryable<TblAuthorityLetters>> getPendingAuthorityLettersByWarehouseInchangeID(string userID)
        {
            return _context.AuthorityLetters
                .Include(x => x.AuthorityLetterDetails)
                .Include(x=>x.Order).ThenInclude(x=>x.Products).ThenInclude(x=>x.Product)
                .Include(x => x.Warehouse)
                .Include(x=>x.Attachment)
                .Include(x => x.CreatedBy).Where(x=>x.Warehouse.InchargeID == userID && x.Active != EAuthorityLetterStatus.DeActive);
        }


    }
}
