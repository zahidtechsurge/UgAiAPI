using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IIntroRepo // Defining the interface for handling introductions
    {
        Task<List<IntroDTO>> getIntros(getIntroDTO req); // Method signature for getting introductions by language request
        IQueryable<tblIntroLanguages> getIntroLanguageQueryable();
        IQueryable<tblIntro> getIntroQueryable();
        void addIntroLanguage(tblIntroLanguages req);
        void updateIntroLanguage(tblIntroLanguages req);
        Task<tblIntroLanguages?> GetIntroLanguagesByID(int Id);
        Task<tblIntroLanguages?> GetIntroLanguageByIntroIDAndLanguage(int introID, string languageCode);
        void updateLanguage(tblIntroLanguages req);
        Task<tblIntro?> GetIntroByNameAndStatus(int id);
        void UpdateIntro(tblIntro intro);
        void AddIntro(tblIntro intro);
        // void addIntro(tblIntro intro);
        Task<tblIntro?> GetIntroByName(string name);
        //  void AddIntro(tblIntro intro);

    }
}
