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
    }
}
