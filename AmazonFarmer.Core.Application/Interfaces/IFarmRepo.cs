using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IFarmRepo // Defining the interface for handling farms
    {
        Task<int> addFarm(FarmDTO farm, string UserID); // Method signature for adding farms
        Task<int> updateFarm(FarmDTO farm, string UserID); // Method signature for adding farms
        Task<int> addFarmRequest(FarmDTO farm, string UserID); // Method signature for adding farm requests
        Task<int> updateFarmRequest(FarmDTO farm, string UserID);
        Task<int?> getApplicationIDByFarmerID(string userID);
        Task<List<tblfarm>> getFarmsByUserID(string UserID); // Method signature for getting farms by user ID
        Task<List<tblfarm>> getFarmsByfarmerID(string userID); // Method signature for getting farms table by farmer ID
        Task<List<farmRequest_Resp>> getFarmRequestsByUserID(string UserID); // Method signature for getting farm requests by user ID
        Task<farmSetup_Resp> addFarmApplication(); // Method signature for adding farm application
        Task farmApprovalAcknowledgement(string farmerID); // Method signature for farm approval acknowledgement
        Task<getFarmLocation> getFarmLocationByFarmID(int FarmID); // Method signature for getting farm location by farm ID
        Task<IQueryable<tblfarm>> getFarmApplications(); // Method signature for getting farm applications
        Task<IQueryable<tblfarm>> getFarmsByApplicationIDandLanguageCode(); // Method signature for getting farms by application ID and LanguageCode
        Task changeFarmRegistrationStatus(tblfarm farm, string approverID); // Method signature for changing farm registration status
        Task<tblfarm> getFarmByFarmID(int farmID); // Method signature for getting farm by farmID
        Task<tblfarm> getFarmByFarmID(int farmID, string userID, string languageCode); // Method signature for getting farm by farmID, userID, languageCode
        Task<List<tblFarmAttachments>> getFarmAttachmentsByFarmID(int farmID); // Method signature for getting farm attachments by farmID
        Task removeFarmsByApplicationID(int applicationID); // Method signature for update the farm
        Task updateFarm(tblfarm farm); // Method signature for update the farm
        Task removeFarmChangeRequestByApplicationID(int applicationID); // Method signature for update the farm change request
        Task<List<DraftedFarmDTO>> getDraftedFarmsByFarmerID(string userID, string languageCode); // Method signature for fetching the drafted farms list by farmer ID and language code
        Task<List<tblfarm>> getFarmerAllFarmsByApplicationID(int applicationID); // Method signature for fetching farms list by application ID 
        void AddFarmUpdateLogs(tblfarm farm, string updatedBy);


        //For Employee
        Task<tblfarm> getFarmByFarmIDForEmployee(int farmID, List<int> territoryIds);
    }
}
