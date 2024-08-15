using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ITehsilRepo // Defining the interface for Tehsil repository
    {
        Task<List<TehsilDTO>> getTehsils(getTehsil_Req req); // Method signature for retrieving Tehsils
        IQueryable<tblTehsil> GetTehsils();
        Task<tblTehsil?> GetTehsilByID(int id);
        Task<tblTehsil?> GetTehsilByID(string name, string code);
        void AddTehsil(tblTehsil tehsil);
        void UpdateTehsil(tblTehsil tehsil);
        Task<List<tblTehsilLanguages>> GetTehsilLanguagesByTehsilID(int id);
        Task<tblTehsilLanguages?> GetTehsilLanguageByID(int id);
        Task<tblTehsilLanguages?> GetTehsilLanguageByID(int tehsilID, string code);
        void AddTehsilTrasnaltion(tblTehsilLanguages tehsilLanguages);
        void UpdateTehsilTranslation(tblTehsilLanguages tehsilLanguages);
    }
}
