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
    public class RegionRepo : IRegionRepo
    {
        private AmazonFarmerContext _context;
        public RegionRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public IQueryable<tblRegion> GetRegions()
        {
            return _context.Regions.Include(x => x.RegionLanguages);
        }
        public async Task<tblRegion?> GetRegion(int regionID)
        {
            return await _context.Regions.Where(x => x.ID == regionID).FirstOrDefaultAsync();
        }
        public async Task<tblRegion?> GetRegion(string regionName, string regionCode)
        {
            return await _context.Regions.Where(x => x.Name == regionName || x.RegionCode == regionCode).FirstOrDefaultAsync();
        }
        public void AddRegion(tblRegion region)
        {
            _context.Regions.Add(region);
        }
        public void UpdateRegion(tblRegion region)
        {
            _context.Regions.Update(region);
        }

    }
}
