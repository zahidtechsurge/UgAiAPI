using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IWarehouseRepo
    {
        Task<tblwarehouse> getWarehouseByID(int warehouseID); // Method signature for getting warehouse by warehouseID
        Task<TblUser> getWarehouseInchargeByWarehouseID(int warehouseID); // Method signature for getting warehouse Incharge by warehouseID
        Task<List<tblwarehouse>> getUserWarehousesByUserID(string userID); // Method signature for getting warehouses list by warehouse Incharge ID
        IQueryable<tblwarehouse> getWarehouses();
        Task<List<tblLanguages>> getWarehouseLanguagesByWarehouseID(int warehouseID);
        Task<List<tblwarehouseTranslation>> getWarehouseTranslationsByWarehouseID(int warehouseID);
        void addWarehouse(tblwarehouse tblwarehouse);
        void addWarehouseTranslation(tblwarehouseTranslation translation);
        void updateWarehouse(tblwarehouse tblwarehouse);
        void updateWarehouseTranslation(tblwarehouseTranslation translation);
        Task<tblwarehouseTranslation> getWarehouseTranslationByWarehouseID(int warehouseID);
        Task<tblwarehouseTranslation> getWarehouseTranslationByWarehouseID(int warehouseID, string languageCode);
    }
}
