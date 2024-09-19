using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing Farms in the database.
    /// </summary>
    public class FarmRepo : IFarmRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the FarmRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public FarmRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new farm to the database.
        /// </summary>
        /// <param name="farm">The FarmDTO containing the farm details.</param>
        /// <param name="UserID">The ID of the user associated with the farm.</param>
        /// <returns>The ID of the newly added farm.</returns>
        public async Task<int> addFarm(FarmDTO farm, string UserID)
        {
            // Create a new tblfarm object with the provided farm details
            tblfarm _req = new tblfarm()
            {
                FarmName = farm.farmName,
                Address1 = farm.address1,
                Address2 = farm.address2,
                Address3 = farm.address3,
                CityID = farm.cityID,
                DistrictID = farm.districtID,
                TehsilID = farm.tehsilID,
                Acreage = farm.acreage,
                isLeased = farm.isLeased,
                isPrimary = false,
                isApproved = false,
                Status = EFarmStatus.Draft,
                UserID = UserID,
                SAPFarmCode = string.Empty,
                ApplicationID = farm.applicationID,
                latitude = farm.latitude,
                longitude = farm.longitude,
                FarmerComment = farm.farmerComment
            };

            // Add the new farm to the Farms DbSet and save changes to the database
            _req = _context.Farms.Add(_req).Entity;


            _context.SaveChanges();

            // Return the ID of the newly added farm
            return _req.FarmID;
        }
        public async Task<int> updateFarm(FarmDTO farm, string UserID)
        {
            // Create a new tblfarm object with the provided farm details
            tblfarm _req = new tblfarm()
            {
                FarmID = farm.farmID,
                FarmName = farm.farmName,
                Address1 = farm.address1,
                Address2 = farm.address2,
                Address3 = farm.address3,
                CityID = farm.cityID,
                DistrictID = farm.districtID,
                TehsilID = farm.tehsilID,
                Acreage = farm.acreage,
                isLeased = farm.isLeased,
                isPrimary = false,
                isApproved = false,
                Status = EFarmStatus.Draft,
                UserID = UserID,
                SAPFarmCode = string.Empty,
                ApplicationID = farm.applicationID,
                latitude = farm.latitude,
                longitude = farm.longitude,
                FarmerComment = farm.farmerComment
            };

            // Add the new farm to the Farms DbSet and save changes to the database
            _req = _context.Farms.Update(_req).Entity;
            //_context.SaveChanges();

            // Return the ID of the newly added farm
            return _req.FarmID;
        }

        /// <summary>
        /// Adds a new farm change request to the database.
        /// </summary>
        /// <param name="farm">The FarmDTO containing the farm change request details.</param>
        /// <param name="UserID">The ID of the user associated with the farm change request.</param>
        /// <returns>The ID of the newly added farm change request.</returns>
        public async Task<int> addFarmRequest(FarmDTO farm, string UserID)
        {
            // Check if the farm DTO contains a non-null application ID
            if (farm.applicationID.HasValue)
            {
                // Create a new tblFarmChangeRequest object with the provided farm change request details
                tblFarmChangeRequest _req = new tblFarmChangeRequest()
                {
                    ApplicationID = farm.applicationID.Value,
                    FarmName = farm.farmName,
                    Address1 = farm.address1,
                    Address2 = farm.address2,
                    Address3 = farm.address3,
                    CityID = farm.cityID,
                    DistrictID = farm.districtID,
                    TehsilID = farm.tehsilID,
                    Acreage = farm.acreage,
                    isLeased = farm.isLeased,
                    isPrimary = false,
                    isApproved = false,
                    RequestStatus = EFarmStatus.Draft,
                    UserID = UserID,
                    FarmerComment = farm.farmerComment
                };

                // Add the new farm change request to the FarmChangeRequests DbSet and save changes to the database
                _req = _context.FarmChangeRequests.Add(_req).Entity;
                _context.SaveChanges();

                // Return the ID of the newly added farm change request
                return _req.ID;
            }
            else
            {
                throw new AmazonFarmerException("Application ID cannot be null.");
            }
        }
        public async Task<int> updateFarmRequest(FarmDTO farm, string UserID)
        {
            // Check if the farm DTO contains a non-null application ID
            if (farm.applicationID.HasValue)
            {
                // Create a new tblFarmChangeRequest object with the provided farm change request details
                tblFarmChangeRequest _req = new tblFarmChangeRequest()
                {
                    FarmID = farm.farmID,
                    ApplicationID = farm.applicationID.Value,
                    FarmName = farm.farmName,
                    Address1 = farm.address1,
                    Address2 = farm.address2,
                    Address3 = farm.address3,
                    CityID = farm.cityID,
                    DistrictID = farm.districtID,
                    TehsilID = farm.tehsilID,
                    Acreage = farm.acreage,
                    isLeased = farm.isLeased,
                    isPrimary = false,
                    isApproved = false,
                    RequestStatus = EFarmStatus.Draft,
                    UserID = UserID,
                    FarmerComment = farm.farmerComment
                };

                // Add the new farm change request to the FarmChangeRequests DbSet and save changes to the database
                _req = _context.FarmChangeRequests.Update(_req).Entity;
                //_context.SaveChanges();

                // Return the ID of the newly added farm change request
                return _req.ID;
            }
            else
            {
                throw new AmazonFarmerException("Application ID cannot be null.");
            }
        }
        public async Task<int?> getApplicationIDByFarmerID(string userID)
        {
            return await _context.Farms.Where(x => x.UserID == userID).Select(x => x.ApplicationID).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a list of farms associated with the specified user ID.
        /// </summary>
        /// <param name="UserID">The ID of the user.</param>
        /// <returns>A list of farms associated with the user ID.</returns>
        //public async Task<List<farms_Resp>> getFarmsByUserID(string UserID)
        public async Task<List<tblfarm>> getFarmsByUserID(string UserID)
        {
            var query = await _context.Farms
            .Include(x => x.plans).ThenInclude(x => x.PlanCrops)
            .Where(x => x.Status == EFarmStatus.Approved && x.UserID == UserID).ToListAsync();

            return query;
        }

        /// <summary>
        /// Retrieves a list of farms table data associated with the specified farmer ID.
        /// </summary>
        /// <param name="farmerID">The ID of the user.</param>
        /// <returns>A list of farms associated with the user ID.</returns>
        public async Task<List<tblfarm>> getFarmsByfarmerID(string farmerID)
        {
            return await _context.Farms.Include(f => f.Users).Include(x => x.City).Where(x => x.UserID == farmerID).ToListAsync();
        }
        /// <summary>
        /// Retrieves a list of farm change requests associated with the specified user ID.
        /// </summary>
        /// <param name="UserID">The ID of the user.</param>
        /// <returns>A list of farm change requests associated with the user ID.</returns>
        public async Task<List<farmRequest_Resp>> getFarmRequestsByUserID(string UserID)
        {
            return await _context.FarmChangeRequests
                .Where(x => x.UserID == UserID)
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
        public async Task farmApprovalAcknowledgement(string farmerID)
        {

            var farms = await _context.Farms.Where(x => x.UserID == farmerID).OrderBy(x => x.UpdatedOn).ToListAsync();

            if (farms != null)
            {
                foreach (var farm in farms)
                {
                    if (farm.ApplicationID != null && farm.Status == EFarmStatus.Approved)
                    {
                        farm.isFarmApprovalAcknowledged = true;
                        farm.isFarmApprovalAcknowledgedDate = DateTime.UtcNow;
                        _context.Farms.Update(farm);
                    }
                    //else
                    //{
                    //    throw new AmazonFarmerException(_exceptions.farmNotAuthorized);
                    //}
                }
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.farmNotFound);
            }
        }


        /// <summary>
        /// Adds a new farm application and returns the application ID.
        /// </summary>
        /// <returns>A response containing the application ID.</returns>
        public async Task<farmSetup_Resp> addFarmApplication()
        {
            farmSetup_Resp resp = new farmSetup_Resp();

            tblFarmApplication _req = new tblFarmApplication()
            {
                ApplicationTypeID = EApplicationType.Farm_Application
            };

            _req = _context.FarmApplication.Add(_req).Entity;
            await _context.SaveChangesAsync();

            resp.applicationID = _req.ID.ToString();

            return resp;
        }
        /// <summary>
        /// Retrieves the location (latitude and longitude) of a farm by its ID.
        /// </summary>
        /// <param name="FarmID">The ID of the farm.</param>
        /// <returns>The location of the farm.</returns>
        public async Task<getFarmLocation> getFarmLocationByFarmID(int FarmID)
        {
            return await _context.Farms
                .Where(x => x.FarmID == FarmID && x.Status == EFarmStatus.Approved)
                .Select(x => new getFarmLocation
                {
                    latitude = x.latitude ?? 0,
                    longitude = x.longitude ?? 0
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a list of farm applications with their respective farmer names.
        /// </summary>
        /// <returns>A list of farm applications.</returns>
        public async Task<IQueryable<tblfarm>> getFarmApplications()
        {
            //// Retrieve farms with related entities

            return _context.Farms
                    .Include(x => x.FarmApplication)
                    .Include(x => x.Users)
                        .ThenInclude(x => x.FarmerProfile);

        }
        /// <summary>
        /// Retrieves farms by their ApplicationID.
        /// </summary>
        /// <param name="ApplicationID">The ApplicationID to filter farms by.</param>
        /// <returns>A list of farm registration request responses.</returns>
        public async Task<IQueryable<tblfarm>> getFarmsByApplicationIDandLanguageCode()
        {

            return _context.Farms
                .Include(x => x.City)
                    .ThenInclude(x => x.CityLanguages)
                .Include(x => x.District)
                    .ThenInclude(x => x.DistrictLanguages)
                .Include(x => x.Tehsil)
                    .ThenInclude(x => x.TehsilLanguagess)
                .Include(x => x.FarmAttachments)
                    .ThenInclude(x => x.Attachment)
                        .ThenInclude(x => x.AttachmentTypes)
                .Include(x => x.Users)
                    .ThenInclude(x => x.UserAttachments)
                        .ThenInclude(x => x.Attachment)
                            .ThenInclude(x => x.AttachmentTypes)
                .Include(x => x.Users)
                    .ThenInclude(x => x.FarmerProfile)
                .Where(x => x.Status != EFarmStatus.Deleted);
        }
        /// <summary>
        /// Changes the registration status of a farm.
        /// </summary>
        /// <param name="farm">The farm.</param>
        /// <param name="statusID">The ID of the new status.</param>
        /// <param name="approverID">The ID of the approver.</param>
        public async Task changeFarmRegistrationStatus(tblfarm farm, string approverID)
        {
            farm.UpdatedOn = DateTime.UtcNow;
            farm.UpdatedBy = approverID;

            _context.Farms.Update(farm);

            AddFarmUpdateLogs(farm, approverID);
        }

        public void AddFarmUpdateLogs(tblfarm farm, string updatedBy)
        {
            _context.FarmUpdateLog.Add(new FarmUpdateLog { FarmId = farm.FarmID, UpdatedDate = DateTime.UtcNow, Status = farm.Status, UserId = updatedBy });
        }

        /// <summary>
        /// Get Farm by FarmID.
        /// </summary>
        /// <param name="farmID">The ID of the farm.</param>
        public async Task<tblfarm> getFarmByFarmID(int farmID)
        {
            return await _context.Farms
                .Include(x => x.City)
                    .ThenInclude(x => x.CityLanguages)
                .Include(x => x.District)
                    .ThenInclude(x => x.DistrictLanguages)
                .Include(x => x.Tehsil)
                    .ThenInclude(x => x.TehsilLanguagess)
                .Include(x => x.Users)
                    .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.City).ThenInclude(c => c.Tehsils)
                .Include(x => x.Users)
                    .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.District)
                .Include(x => x.FarmAttachments.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.Attachment)
                .Include(x => x.Users)
                    .ThenInclude(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.Attachment)
                            .ThenInclude(x => x.AttachmentTypes)
                .Where(x => x.FarmID == farmID)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Farm by FarmID.
        /// </summary>
        /// <param name="farmID">The ID of the farm.</param>
        /// <param name="userID">The ID of the farmer.</param>
        /// <param name="languageCode">The selected Language of the farmer.</param>
        public async Task<tblfarm> getFarmByFarmID(int farmID, string userID, string languageCode)
        {
            return await _context.Farms
                .Include(x => x.City)
                    .ThenInclude(x => x.CityLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.District)
                    .ThenInclude(x => x.DistrictLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.Tehsil)
                    .ThenInclude(x => x.TehsilLanguagess.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.Users)
                    .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.City).ThenInclude(c => c.Tehsils)
                .Include(x => x.Users)
                    .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.District)
                .Include(x => x.FarmAttachments.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.Attachment)
                .Include(x => x.Users)
                    .ThenInclude(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                        .ThenInclude(x => x.Attachment)
                            .ThenInclude(x => x.AttachmentTypes)
                .Where(x => x.FarmID == farmID && x.UserID == userID)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Farm by FarmID.
        /// </summary>
        /// <param name="farmID">The ID of the farm.</param>
        /// <param name="userID">The ID of the farmer.</param>
        /// <param name="languageCode">The selected Language of the farmer.</param>
        public async Task<tblfarm> getFarmByFarmIDForEmployee(int farmID, List<int> territoryIds)
        {
            IQueryable<tblfarm> farmQuery = _context.Farms
                     .Include(x => x.City)
                     .Include(x => x.District)
                     .Include(x => x.Tehsil)
                     .Include(x => x.Users)
                         .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.City).ThenInclude(c => c.Tehsils)
                     .Include(x => x.Users)
                         .ThenInclude(x => x.FarmerProfile).ThenInclude(p => p.District)
                     .Include(x => x.FarmAttachments.Where(x => x.Status == EActivityStatus.Active))
                         .ThenInclude(x => x.Attachment)
                     .Include(x => x.Users)
                         .ThenInclude(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                             .ThenInclude(x => x.Attachment)
                                 .ThenInclude(x => x.AttachmentTypes)
                     .Where(x => x.FarmID == farmID);

            if (territoryIds.Count != 0)
            {
                farmQuery = farmQuery.Where(x => territoryIds.Contains(x.DistrictID));
            }

            return await farmQuery.FirstOrDefaultAsync();
        }

        public async Task<List<tblFarmAttachments>> getFarmAttachmentsByFarmID(int farmID)
        {
            return await _context.FarmAttachments.Include(x => x.Attachment).Where(x => x.FarmID == farmID && x.Status == EActivityStatus.Active).ToListAsync();
        }

        /// <summary>
        /// Remove Farm by applicationID.
        /// </summary>
        /// <param name="applicationID">The applicationID.</param>
        public async Task removeFarmsByApplicationID(int applicationID)
        {
            List<tblfarm> farms = await _context.Farms.Where(x => x.ApplicationID == applicationID).ToListAsync();
            if (farms != null)
            {
                foreach (var farm in farms)
                {
                    farm.Status = EFarmStatus.Deleted;
                    _context.Update(farm);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task updateFarm(tblfarm farm)
        {
            _context.Update(farm);
        }
        /// <summary>
        /// Remove Farm Change Request by applicationID.
        /// </summary>
        /// <param name="applicationID">The applicationID.</param>
        public async Task removeFarmChangeRequestByApplicationID(int applicationID)
        {
            List<tblFarmChangeRequest> farms = await _context.FarmChangeRequests.Where(x => x.ApplicationID == applicationID).ToListAsync();
            if (farms != null)
            {
                foreach (var farm in farms)
                {
                    farm.RequestStatus = EFarmStatus.Deleted;
                    _context.Update(farm);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<DraftedFarmDTO>> getDraftedFarmsByFarmerID(string userID, string languageCode)
        {
            return await _context.Farms
                .Include(x => x.City)
                    .ThenInclude(x => x.CityLanguages)
                .Include(x => x.Tehsil)
                    .ThenInclude(x => x.TehsilLanguagess)
                .Include(x => x.District)
                    .ThenInclude(x => x.DistrictLanguages)
                .Include(x => x.FarmAttachments)
                    .ThenInclude(x => x.Attachment)
                .Where(x => x.Status == EFarmStatus.Draft && x.UserID == userID)
                .Select(x => new DraftedFarmDTO
                {
                    applicationID = x.ApplicationID,
                    farmID = x.FarmID,
                    isPrimary = x.isPrimary,
                    farmName = x.FarmName,
                    address1 = x.Address1,
                    address2 = x.Address2,
                    cityID = x.CityID,
                    city = x.City.CityLanguages.Where(x => x.LanguageCode == languageCode).Select(x => x.Translation).First(),
                    tehsilID = x.TehsilID,
                    tehsil = x.Tehsil.TehsilLanguagess.Where(x => x.LanguageCode == languageCode).Select(x => x.Translation).First(),
                    districtID = x.DistrictID,
                    district = x.District.DistrictLanguages.Where(x => x.LanguageCode == languageCode).Select(x => x.Translation).First(),
                    acreage = x.Acreage,
                    isLeased = x.isLeased,
                    attachments = x.FarmAttachments.Select(y => new uploadAttachmentResp
                    {
                        id = y.AttachmentID,
                        guid = y.Attachment.Guid.ToString(),
                        name = y.Attachment.Name,
                        type = y.Attachment.FileType
                    }).ToList()
                }).ToListAsync();
        }
        public async Task<List<tblfarm>> getFarmerAllFarmsByApplicationID(int applicationID)
        {
            return await _context.Farms
                .Include(f => f.Users)
                .Where(x => x.ApplicationID == applicationID).ToListAsync();
        }
        public async Task<List<int>> getFarmDistrictIDsByUserID(string userID)
        {
            return await _context.Farms
                .Where(x =>
                    x.UserID == userID &&
                    x.Status == EFarmStatus.Approved
                )
                .Select(x => x.DistrictID)
                .ToListAsync();
        }
    }
}
