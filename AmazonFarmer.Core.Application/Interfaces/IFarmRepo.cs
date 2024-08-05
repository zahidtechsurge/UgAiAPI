using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IFarmRepo
    {
        Task<int> addFarms(FarmDTO farm, string UserID);
        Task<int> addFarmRequest(FarmDTO farm, string UserID);
        Task<List<farms_Resp>> getFarmsByUserID(string UserID);
        Task<List<farmRequest_Resp>> getFarmRequestsByUserID(string UserID);
        Task<farmSetup_Resp> addFarmApplication();
        //Task farmApprovalAcknowledgement(int applicationID, string farmerID);
        Task<getFarmLocation> getfarmLocationByFarmID(int FarmID);
        Task<List<farmApplication_List>> getFarmApplications();
        Task<List<farmRegistrationRequest_Resp>> getFarmsByApplicationID(int ApplicationID);
        Task changeFarmRegistrationStatus(int farmID, int statusID, string approverID);
    }
}
