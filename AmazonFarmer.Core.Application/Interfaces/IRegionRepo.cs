using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IRegionRepo
    {
        IQueryable<tblRegion> GetRegions();
        Task<tblRegion?> GetRegion(int regionID);
        Task<tblRegion?> GetRegion(string regionName, string regionCode);
        void AddRegion(tblRegion region);
        void UpdateRegion(tblRegion region);
    }
}
