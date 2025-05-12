using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Administrator.API.Controllers
{
    /// <summary>
    /// Controller for managing language-related operations.
    /// </summary>
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/Warehouses")]
    public class WarehousesController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public WarehousesController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        [AllowAnonymous]
        [HttpPost("getWarehouses")]
        public async Task<APIResponse> GetWarehouses(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();

            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblwarehouse> warehouses = _repoWrapper.WarehouseRepo.getWarehouses();

            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("warehouseID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.ID);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("warehouse"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.Name);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.Name);
                    }
                }
                else if (req.sortColumn.Contains("warehouseCode"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.WHCode);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.WHCode);
                    }
                }
                else if (req.sortColumn.Contains("warehouseAddress"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.Address);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.Address);
                    }
                }
                else if (req.sortColumn.Contains("warehouseInchargeID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.WarehouseIncharge.Id);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.WarehouseIncharge.Id);
                    }
                }
                else if (req.sortColumn.Contains("warehouseIncharge"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.WarehouseIncharge.FirstName);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.WarehouseIncharge.FirstName);
                    }
                }
                else if (req.sortColumn.Contains("warehouseInchargeEmail"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.WarehouseIncharge.Email);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.WarehouseIncharge.Email);
                    }
                }
                else if (req.sortColumn.Contains("warehouseInchargePhone"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.WarehouseIncharge.PhoneNumber);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.WarehouseIncharge.PhoneNumber);
                    }
                }
                else if (req.sortColumn.Contains("district"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.District.Name);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.District.Name);
                    }
                }
                else if (req.sortColumn.Contains("salePoint"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.SalePoint);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.SalePoint);
                    }
                }
                else if (req.sortColumn.Contains("warehouseLat"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.latitude);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.latitude);
                    }
                }
                else if (req.sortColumn.Contains("warehouseLong"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.longitude);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.longitude);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        warehouses = warehouses.OrderBy(x => x.Status);
                    }
                    else
                    {
                        warehouses = warehouses.OrderByDescending(x => x.Status);
                    }
                }
            }
            else
            {
                warehouses = warehouses.OrderByDescending(x => x.ID);
            }
            if (!string.IsNullOrEmpty(req.search))
            {
                warehouses = warehouses.Where(x =>
                    x.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.WHCode.ToLower().Contains(req.search.ToLower()) ||
                    (
                        x.WarehouseIncharge != null && (x.WarehouseIncharge.FirstName+ " "+ x.WarehouseIncharge.LastName).ToString().ToLower().Contains(req.search.ToLower())
                    ) ||
                    (
                        x.WarehouseIncharge != null && (x.WarehouseIncharge.Email).ToLower().Contains(req.search.ToLower())
                    )
                );
            }
            InResp.totalRecord = warehouses.Count();
            warehouses = warehouses.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = warehouses.Count();
            InResp.list = await warehouses.Select(x => new WarehouseDTO
            {
                warehouseID = x.ID,
                warehouse = x.Name,
                warehouseCode = x.WHCode,
                warehouseAddress = x.Address,
                warehouseInchargeID = x.WarehouseIncharge.Id,
                warehouseIncharge = string.Concat(x.WarehouseIncharge.FirstName," ", x.WarehouseIncharge.LastName),
                warehouseInchargeEmail = x.WarehouseIncharge.Email ?? string.Empty,
                warehouseInchargePhone = x.WarehouseIncharge.PhoneNumber ?? string.Empty,
                districtID = x.District == null ? 0 : x.District.ID,
                district = x.District == null ? string.Empty : x.District.Name,
                salePoint = x.SalePoint ?? string.Empty,
                warehouseLat = x.latitude,
                warehouseLong = x.longitude,
                translations = x.WarehouseTranslation.Select(t => new WarehouseTranslationDTO
                {
                    translationID = t.ID,
                    warehouseID = t.WarehouseID,
                    languageCode = t.LanguageCode,
                    language = t.Language.LanguageName,
                    text = t.Text
                }).ToList(),
                status = (int)x.Status
            }).ToListAsync();
            resp.response = InResp;


            return resp;
        }
        [AllowAnonymous]
        [HttpGet("getTranslations/{warehouseID}")]
        public async Task<APIResponse> GetTranslations(int warehouseID)
        {
            APIResponse resp = new APIResponse();
            //var lang = await _repoWrapper.WarehouseRepo.getWarehouseLanguagesByWarehouseID(warehouseID: warehouseID);
            //resp.response = lang.Select(t => new WarehouseTranslationDTO
            //{
            //    translationID = t.WarehouseTranslations.FirstOrDefault() != null ? t.WarehouseTranslations.FirstOrDefault().ID : 0,
            //    warehouseID = t.WarehouseTranslations.FirstOrDefault() != null ? t.WarehouseTranslations.FirstOrDefault().WarehouseID : 0,
            //    languageCode = t.LanguageCode,
            //    language = t.LanguageName,
            //    text = t.WarehouseTranslations.FirstOrDefault() != null ? t.WarehouseTranslations.FirstOrDefault().Text : string.Empty
            //}).ToList();
            var warehouseTranslations = await _repoWrapper.WarehouseRepo.getWarehouseTranslationsByWarehouseID(warehouseID: warehouseID);
            resp.response = warehouseTranslations.Select(t => new WarehouseTranslationDTO
            {
                translationID = t.ID,
                warehouseID = t.WarehouseID,
                languageCode = t.LanguageCode,
                language = t.Language.LanguageName,
                text = t.Text
            }).ToList();
            return resp;
        }
        [HttpGet("getWarehouseDetail/{warehouseID}")]
        public async Task<APIResponse> GetWarehouseDetail(int warehouseID)
        {
            APIResponse resp = new APIResponse();
            tblwarehouse x = await _repoWrapper.WarehouseRepo.getWarehouseByID(warehouseID);
            if (x == null)
                throw new AmazonFarmerException(_exceptions.warehouseNotFound);
            WarehouseDTO inResp = new WarehouseDTO()
            {
                warehouseID = x.ID,
                warehouse = x.Name,
                warehouseCode = x.WHCode,
                warehouseAddress = x.Address,
                warehouseInchargeID = x.WarehouseIncharge.Id,
                warehouseIncharge = string.Concat(x.WarehouseIncharge.FirstName, " ", x.WarehouseIncharge.LastName),
                warehouseInchargeEmail = x.WarehouseIncharge.Email ?? string.Empty,
                warehouseInchargePhone = x.WarehouseIncharge.PhoneNumber ?? string.Empty,
                districtID = x.District == null ? 0 : x.District.ID,
                district = x.District == null ? string.Empty : x.District.Name,
                salePoint = x.SalePoint ?? string.Empty,
                warehouseLat = x.latitude,
                warehouseLong = x.longitude,
                translations = x.WarehouseTranslation.Select(t => new WarehouseTranslationDTO
                {
                    translationID = t.ID,
                    warehouseID = t.WarehouseID,
                    languageCode = t.LanguageCode,
                    language = t.Language.LanguageName,
                    text = t.Text
                }).ToList(),
                status = (int)x.Status
            };
            resp.response = inResp;
            return resp;
        }
        [HttpPost("addWarehouse")]
        public async Task<JSONResponse> AddWarehouse(AddWarehouseDTO req)
        {
            JSONResponse response = new JSONResponse();

            validateWarehouseRequest(req);

            tblwarehouse warehouse = new tblwarehouse()
            {
                Name = req.warehouseName,
                WHCode = req.warehouseCode,
                Address = req.warehouseAddress,
                latitude = req.warehouseLat,
                longitude = req.warehouseLong,
                InchargeID = req.warehouseIncharge,
                DistrictID = req.warehousedistrictID,
                SalePoint = req.warehouseSalePoint,
                Status = req.status ? EActivityStatus.Active : EActivityStatus.DeActive
            };

            _repoWrapper.WarehouseRepo.addWarehouse(warehouse);
            await _repoWrapper.SaveAsync();

            response.message = "warehouse added";

            return response;
        }
        [HttpPut("updateWarehouse")]
        public async Task<JSONResponse> UpdateWarehouse(UpdateWarehouseDTO req)
        {
            JSONResponse response = new JSONResponse();
            if (req.warehouseID == 0)
                throw new AmazonFarmerException(_exceptions.warehouseIDNotFound);
            validateWarehouseRequest(req);

            tblwarehouse warehouse = await _repoWrapper.WarehouseRepo.getWarehouseByID(req.warehouseID);
            if (warehouse == null)
                throw new AmazonFarmerException(_exceptions.warehouseNotFound);

            warehouse.Name = req.warehouseName;
            warehouse.WHCode = req.warehouseCode;
            warehouse.Address = req.warehouseAddress;
            warehouse.latitude = req.warehouseLat;
            warehouse.longitude = req.warehouseLong;
            warehouse.InchargeID = req.warehouseIncharge;
            warehouse.DistrictID = req.warehousedistrictID;
            warehouse.SalePoint = req.warehouseSalePoint;
            warehouse.Status = req.status ? EActivityStatus.Active : EActivityStatus.DeActive;

            _repoWrapper.WarehouseRepo.updateWarehouse(warehouse);
            await _repoWrapper.SaveAsync();

            response.message = "warehouse added";


            return response;
        }
        [HttpPatch("syncWarehouseTranslation")]
        public async Task<JSONResponse> AddWarehouseTranslation(SyncWarehouseTranslationDTO req)
        {
            JSONResponse response = new JSONResponse();

            tblwarehouseTranslation warehouseTramslation = await _repoWrapper.WarehouseRepo.getWarehouseTranslationByWarehouseID(req.warehouseID, req.languageCode);
            if (warehouseTramslation == null)
            {
                warehouseTramslation = new tblwarehouseTranslation()
                {
                    WarehouseID = req.warehouseID,
                    LanguageCode = req.languageCode,
                    Text = req.text
                };
                _repoWrapper.WarehouseRepo.addWarehouseTranslation(warehouseTramslation);
                response.message = "Translation has been added";
            }
            else
            {
                warehouseTramslation.Text = req.text;
                _repoWrapper.WarehouseRepo.updateWarehouseTranslation(warehouseTramslation);
                response.message = "Translation has been updated";
            }
            await _repoWrapper.SaveAsync();
            return response;
        }

        [HttpGet("getWarehouseIncharges")]
        public async Task<APIResponse> GetWarehouseIncharges()
        {
            APIResponse resp = new APIResponse();
            IQueryable<TblUser> users = _repoWrapper.UserRepo.getUsers();
            users = users.Where(x => x.Designation == EDesignation.Warehouse_Incharge && x.Active == EActivityStatus.Active);
            resp.message = "Fetched warehouse incharges";
            resp.response = await users.Select(u => new GetWarehouseInchargeResponse
            {
                fullName = u.FirstName,
                userID = u.Id
            }).ToListAsync();
            return resp;
        }
        private void validateWarehouseRequest(AddWarehouseDTO req)
        {
            bool hasValidationError = false;

            if (string.IsNullOrEmpty(req.warehouseName))
                hasValidationError = true;
            else if (string.IsNullOrEmpty(req.warehouseCode))
                hasValidationError = true;
            else if (string.IsNullOrEmpty(req.warehouseAddress))
                hasValidationError = true;
            else if (req.warehouseLat == 0)
                hasValidationError = true;
            else if (req.warehouseLong == 0)
                hasValidationError = true;
            else if (req.warehousedistrictID == 0)
                hasValidationError = true;
            else if (string.IsNullOrEmpty(req.warehouseSalePoint))
                hasValidationError = true;


            if (hasValidationError)
                throw new AmazonFarmerException(_exceptions.requiredFieldsMissing);
        }
    }
}
