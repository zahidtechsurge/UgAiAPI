using AmazonFarmer.Core.Application.DTOs;
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
    public class RoleRepo : IRoleRepo
    {
        private AmazonFarmerContext _context;

        public RoleRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<getRoles_Resp>> GetRoles(string search, int skip, int take)
        {
            IQueryable<TblRole> roles = _context.Roles; 
            if (!string.IsNullOrEmpty(search))
            {
                roles.Where(x => x.Name.Contains(search));
            }
            return await roles.Skip(skip).Take(take).Select(x => new getRoles_Resp
            {
                roleID = x.Id,
                roleName = x.Name,
                status = x.Status == EActivityStatus.Active ? true : false
            }).ToListAsync();
        }

        public async Task<List<PermissionWiseClaimsDTO>> getModules(string RoleID)
        {
            return await _context.NavigationModules
                .Include(x => x.Pages)
                .ThenInclude(p => p.Claims)
                .ThenInclude(c => c.RoleClaims.Where(x => x.RoleId == RoleID))
                .Select(s => new PermissionWiseClaimsDTO
                {
                    ModuleID = s.ModuleId,
                    ModuleName = s.ModuleName,
                    ModuleClaims = s.Pages.SelectMany(pg => pg.Claims.Select(x => new RoleClaimDTO
                    {
                        Selected = x.RoleClaims.Where(x => x.RoleId == RoleID).Count() > 0,
                        Type = "Permission",
                        Value = x.ClaimId,
                        Description = x.ClaimDescription
                    })).ToList(),
                })
                .ToListAsync();
        }

        //public async Task<List<pagesInRole>> getModules(string RoleID)
        //{
        //    return await _context.NavigationModules
        //        .Include(x => x.Pages)
        //        .ThenInclude(p => p.Claims)
        //        .ThenInclude(c => c.RoleClaims.Where(x => x.RoleId == RoleID))
        //        .Select(s => new pagesInRole
        //        {
        //            ModuleID = s.ModuleId,
        //            ModuleName = s.ModuleName,
        //            ModuleClaims = s.Pages.SelectMany(pg => pg.Claims.Select(x => new RoleClaimDTO
        //            {
        //                Selected = x.RoleClaims.Where(x => x.RoleId == RoleID).Count() > 0,
        //                Type = "Permission",
        //                Value = x.ClaimId,
        //                Description = x.ClaimDescription
        //            })).ToList(),
        //        })
        //        .ToListAsync();
        //}
    }
}
