using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class WarehouseDTO
    {
        public int warehouseID {  get; set; }
        public string warehouse {  get; set; } = string.Empty;
        public string warehouseCode {  get; set; } = string.Empty;
        public string warehouseAddress {  get; set; } = string.Empty;
        public string warehouseInchargeID {  get; set; } = string.Empty;
        public string warehouseIncharge {  get; set; } = string.Empty;
        public string warehouseInchargeEmail {  get; set; } = string.Empty;
        public string warehouseInchargePhone {  get; set; } = string.Empty;
        public int districtID {  get; set; }
        public string district {  get; set; } = string.Empty;
        public string salePoint {  get; set; } = string.Empty;
        public double warehouseLat {  get; set; }
        public double warehouseLong {  get; set; }
        public List<WarehouseTranslationDTO> translations { get; set; } = new List<WarehouseTranslationDTO>();
        public int status {  get; set; }
    }
    public class WarehouseTranslationDTO
    {
        public int translationID { get; set; }
        public int warehouseID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class AddWarehouseDTO
    {
        public required string warehouseName { get; set; }
        public required string warehouseCode { get; set; }
        public required string warehouseAddress { get; set; }
        public required double warehouseLat { get; set; }
        public required double warehouseLong { get; set; }
        public string warehouseIncharge { get; set; } = string.Empty;
        public required int warehousedistrictID { get; set; }
        public required string warehouseSalePoint { get; set; }
        public bool status { get; set; } = false;
    }
    public class UpdateWarehouseDTO : AddWarehouseDTO
    {
        public int warehouseID { get; set; }
    }
    public class SyncWarehouseTranslationDTO
    {
        public int warehouseID { get; set; }
        public required string languageCode { get; set; }
        public required string text { get; set; }
    }
    public class GetWarehouseInchargeResponse
    {
        public required string userID { get; set; }
        public required string fullName { get; set; }
    }
}
