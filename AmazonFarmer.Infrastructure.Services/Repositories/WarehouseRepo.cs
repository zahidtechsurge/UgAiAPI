using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing Warehouses in the database.
    /// </summary>
    public class WarehouseRepo : IWarehouseRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the WarehouseRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public WarehouseRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        public async Task<tblwarehouse> getWarehouseByID(int warehouseID)
        {
            return await _context.Warehouse
                .Include(x => x.WarehouseTranslation)
                    .ThenInclude(x=>x.Language)
                .Include(x => x.WarehouseIncharge)
                .Include(x => x.District)
                .Where(x => x.ID == warehouseID)
                .FirstOrDefaultAsync();
        }
        public async Task<TblUser> getWarehouseInchargeByWarehouseID(int warehouseID)
        {
            return await _context.Users
                .Include(x => x.warehouseIncharge)
                .Where(x =>
                    x.warehouseIncharge.Any(w => w.ID == warehouseID)
                )
                .FirstOrDefaultAsync();
        }

        public async Task<List<tblwarehouse>> getUserWarehousesByUserID(string userID)
        {
            return await _context.Warehouse
                .Include(x => x.WarehouseTranslation)
                .Include(x => x.WarehouseIncharge)
                .Where(x => x.WarehouseIncharge.Id == userID).ToListAsync();
        }
        public IQueryable<tblwarehouse> getWarehouses()
        {
            return _context.Warehouse.Include(x => x.WarehouseTranslation).ThenInclude(x=>x.Language).Include(x => x.District).Include(x => x.WarehouseIncharge);
        }
        public async Task<List<tblwarehouseTranslation>> getWarehouseTranslationsByWarehouseID(int warehouseID)
        {
            return await _context.WarehouseTranslation.Include(x=>x.Language).Where(x => x.WarehouseID == warehouseID).ToListAsync();
        }
        public async Task<List<tblLanguages>> getWarehouseLanguagesByWarehouseID(int warehouseID)
        {
            return await _context.Languages.Include(x=>x.WarehouseTranslations.Where(x=> x.WarehouseID == warehouseID))
                .Where(x=>x.Status == EActivityStatus.Active)
                .ToListAsync();
        }
        public async void addWarehouse(tblwarehouse tblwarehouse)
        {
            _context.Warehouse.Add(tblwarehouse);
        }
        public async void addWarehouseTranslation(tblwarehouseTranslation translation)
        {
            _context.WarehouseTranslation.Add(translation);
        }
        public async void updateWarehouse(tblwarehouse tblwarehouse)
        {
            _context.Warehouse.Update(tblwarehouse);
        }
        public async void updateWarehouseTranslation(tblwarehouseTranslation translation)
        {
            _context.WarehouseTranslation.Update(translation);
        }
        public async Task<tblwarehouseTranslation> getWarehouseTranslationByWarehouseID(int warehouseID)
        {
            return await _context.WarehouseTranslation.Where(x => x.WarehouseID == warehouseID).FirstOrDefaultAsync();
        }
        public async Task<tblwarehouseTranslation> getWarehouseTranslationByWarehouseID(int warehouseID, string languageCode)
        {
            return await _context.WarehouseTranslation.Where(x => x.WarehouseID == warehouseID && x.LanguageCode == languageCode).FirstOrDefaultAsync();
        }
    }
}
