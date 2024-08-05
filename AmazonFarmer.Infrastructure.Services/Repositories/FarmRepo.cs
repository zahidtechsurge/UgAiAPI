using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class FarmRepo : IFarmRepo
    {
        private AmazonFarmerContext _context;

        public FarmRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<int> addFarms(FarmDTO farm, string UserID)
        {
            tblfarm _req = new tblfarm()
            {
                FarmName = farm.farmName,
                Address1 = farm.address1,
                Address2 = farm.address2,
                CityID = farm.cityID,
                DistrictID = farm.districtID,
                TehsilID = farm.tehsilID,
                Acreage = farm.acreage,
                isLeased = farm.isLeased,
                isPrimary = farm.isPrimary,
                isApproved = false,
                Status = farm.requestType,
                UserID = UserID,
                SAPFarmCode = string.Empty,
                ApplicationID = farm.applicationID,
                latitude = farm.latitude,
                longitude = farm.longitude,
            };
            _req = _context.Farms.Add(_req).Entity;

            _context.SaveChanges();

            return _req.FarmID;
        }

        public async Task<int> addFarmRequest(FarmDTO farm, string UserID)
        {
            try
            {
                if (farm.applicationID.HasValue)
                {
                    tblFarmChangeRequest _req = new tblFarmChangeRequest()
                    {
                        ApplicationID = farm.applicationID.Value,
                        FarmName = farm.farmName,
                        Address1 = farm.address1,
                        Address2 = farm.address2,
                        CityID = farm.cityID,
                        DistrictID = farm.districtID,
                        TehsilID = farm.tehsilID,
                        Acreage = farm.acreage,
                        isLeased = farm.isLeased,
                        isPrimary = farm.isPrimary,
                        isApproved = false,
                        RequestStatus = farm.requestType,
                        UserID = UserID
                    };

                    _req = _context.FarmChangeRequests.Add(_req).Entity;
                    _context.SaveChanges();
                    return _req.ID;
                }
                else
                {
                    throw new Exception(_exceptions.nullApplicationID);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<farms_Resp>> getFarmsByUserID(string UserID)
        {
            return await _context.Farms.Where(x => x.Status == EFarmStatus.Active && x.UserID == UserID)
                .Select(x => new farms_Resp
                {
                    farmID = x.FarmID,
                    farmName = x.FarmName,
                    acreage = x.Acreage,
                    isApproved = x.isApproved,
                    isPrimary = x.isPrimary
                })
                .ToListAsync();
        }
        public async Task<List<farmRequest_Resp>> getFarmRequestsByUserID(string UserID)
        {
            return await _context.FarmChangeRequests.Where(x => x.UserID == UserID)
                .Select(x => new farmRequest_Resp
                {
                    requestID = x.ID,
                    farmID = x.FarmID,
                    farmName = x.FarmName,
                    acreage = x.Acreage,
                    isApproved = x.isApproved,
                    isPrimary = x.isPrimary
                })
                .ToListAsync();
        }
        //public async Task farmApprovalAcknowledgement(int applicationID, string farmerID)
        //{

        //    var farms = await _context.Farms.Where(x => x.ApplicationID == applicationID).ToListAsync();

        //    if (farms != null)
        //    {
        //        foreach (var farm in farms)
        //        {
        //            if (farm.UserID == farmerID)
        //            {
        //                farm.isFarmApprovalAcknowledged = true;
        //                farm.isFarmApprovalAcknowledgedDate = DateTime.UtcNow;
        //                _context.Farms.Update(farm);
        //                await _context.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                throw new Exception(_exceptions.farmNotAuthorized);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(_exceptions.farmNotFound);
        //    }
        //}
        public async Task<farmSetup_Resp> addFarmApplication()
        {
            farmSetup_Resp resp = new farmSetup_Resp();
            tblFarmApplication _req = new tblFarmApplication()
            {
                ApplicationTypeID = EApplicationType.Farm_Application
            };
            _req = _context.FarmApplication.Add(_req).Entity;
            _context.SaveChanges();
            resp.applicationID = _req.ID.ToString();
            return resp;
        }
        public async Task<getFarmLocation> getfarmLocationByFarmID(int FarmID)
        {
            return await _context.Farms
                .Where(x => x.Status == EFarmStatus.Active)
                .Select(x => new getFarmLocation()
                {
                    latitude = x.latitude != null ? x.latitude.Value : 0,
                    longitude = x.longitude != null ? x.longitude.Value : 0
                }).FirstOrDefaultAsync();
        }

        public async Task<List<farmApplication_List>> getFarmApplications()
        {
            var farms = await _context.Farms
            .Include(x => x.FarmApplication)
            .Include(x => x.Users)
            .Where(x => x.Status == EFarmStatus.PendingForApproval)
            .ToListAsync();

            // Now, perform the grouping and projection in-memory
            var groupedFarms = farms
                .GroupBy(x => x.ApplicationID) // Group by ApplicationID
                .Select(group => group.First()) // Select the first element of each group
                .Select(x => new farmApplication_List
                {
                    applicationID = x.ApplicationID.ToString().PadLeft(10, '0'),
                    farmerName = (x.Users.FirstName + " " + x.Users.LastName)
                }).ToList();

            return groupedFarms;
        }
        public async Task<List<farmRegistrationRequest_Resp>> getFarmsByApplicationID(int ApplicationID)
        {
            return await _context.Farms.Where(x => x.ApplicationID == ApplicationID).Select(x => new farmRegistrationRequest_Resp
            {
                farmID = x.FarmID,
                applicationID = x.ApplicationID,
                isPrimary = x.isPrimary,
                acreage = x.Acreage,
                address1 = x.Address1,
                address2 = x.Address2,
                cityID = x.CityID,
                districtID = x.DistrictID,
                farmName = x.FarmName,
                isLeased = x.isLeased,
                tehsilID = x.TehsilID,
                requestType = x.Status
            }).ToListAsync();
        }
        public async Task changeFarmRegistrationStatus(int farmID, int statusID, string approverID)
        {
            var farm = await _context.Farms.Where(x => x.FarmID == farmID).FirstOrDefaultAsync();
            if (farm == null)
                throw new Exception(_exceptions.farmNotFound);
            else if (farm.Status != EFarmStatus.PendingForApproval)
                throw new Exception(_exceptions.farmNotAuthorized);
            else
            {
                farm.UpdatedOn = DateTime.Now;
                farm.UpdatedBy = approverID;
                farm.Status = (EFarmStatus)statusID;
                if (((EFarmStatus)statusID) == EFarmStatus.Active)
                {
                    farm.isApproved = true;
                }
                _context.Farms.Update(farm);
                _context.SaveChanges();
            }
        }
    }
}
