using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class ReasonRepo : IReasonRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the ReasonRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public ReasonRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<tblReasonTranslation>> getReasonsByLanguageCodeAndReasonOf(string languageCode, EReasonOf reasonOf)
        {
            return await _context.ReasonTranslations
                .Include(x => x.Reason)
                .Where(x =>
                    x.LanguageCode == languageCode &&
                    x.Reason.ReasonOfID == reasonOf
                )
                .ToListAsync();
        }

        public async Task<tblReasons> getReasonByReasonID(int reasonID)
        {
            return await _context.Reasons.Include(x=>x.ReasonTranslation).Where(x => x.ID == reasonID).FirstOrDefaultAsync();
        }
    }
}
