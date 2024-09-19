using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ISeasonRepo // Defining the interface for season repository
    {
        Task<List<SeasonDTO>> getSeasonsByLanguageCode(LanguageReq req); // Method signature for retrieving seasons by language code
        Task<List<tblSeason>> getSeasons();
        Task<tblSeason?> GetSeasonByID(int ID);
        Task<bool> SeasonExistInDateRange(int fromMonth, int toMonth);
        Task<tblSeason?> GetSeasonByName(string name);
        void AddSeason(tblSeason season);
        void UpdateSeason(tblSeason season);
        Task<List<tblSeasonTranslation>> GetSeasonTranslationBySeasonID(int seasonID);
        Task<tblSeasonTranslation?> GetSeasonTranslationBySeasonID(int seasonID, string languageCode);
        void AddSeasonTranslation(tblSeasonTranslation season);
        void UpdateSeasonTranslation(tblSeasonTranslation season);
    }
}
