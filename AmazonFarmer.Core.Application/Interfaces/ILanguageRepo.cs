using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ILanguageRepo // Defining the interface for handling languages
    {
        Task<List<FarmerLanguageDTO>> GetLanguages(); // Method signature for getting all languages
        IQueryable<tblLanguages> getLangauges();
        Task<tblLanguages> getLanguageByCodeOrName(string Code, string Name);
        void addLanguage(tblLanguages Languages);
        void updateLanguage(tblLanguages Languages);

    }
}
