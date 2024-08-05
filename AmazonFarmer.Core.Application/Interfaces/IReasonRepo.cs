using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IReasonRepo
    {
        Task<List<tblReasonTranslation>> getReasonsByLanguageCodeAndReasonOf(string languageCode, EReasonOf reasonOf); // Method signature for getting the reasons on behalf of language code and reason of
        Task<tblReasons> getReasonByReasonID(int reasonID); // Method signature for getting the reason by reasonID
    }
}
