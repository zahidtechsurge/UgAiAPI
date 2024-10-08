﻿using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IServiceRepo // Defining the interface for service repository
    {
        Task<List<ServiceDTO>> getServicesByLanguageID(getServicesRequestDTO req, int postDeliveryIn); // Method signature for retrieving services by language ID
        Task<List<tblService>> getServicesByIDs(List<int> serviceIDs, string languageCode); // Method signature for retrieving services by service IDs
        IQueryable<tblService> GetService();
        Task<tblService?> GetServiceByID(int id);
        Task<tblService?> GetServiceByID(string name, string code);
        void AddService(tblService service);
        void UpdateService(tblService service);
        Task<List<tblServiceTranslation>> GetServiceTranslationByServiceID(int ID);
        Task<tblServiceTranslation?> GetServiceTranslationByID(int ID);
        Task<tblServiceTranslation?> GetServiceTranslationByID(int ServiceID, string LanguageCode);
        Task<tblServiceTranslation?> GetServiceTranslationByID(string name, string code);
        void AddServiceTranslation(tblServiceTranslation serviceTranslation);
        void UpdateServiceTranslation(tblServiceTranslation serviceTranslation);

    }
}
