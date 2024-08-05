/*
   This class implements the IRoleRepo interface and provides methods for retrieving roles and modules from the database.
*/
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

        // Constructor to initialize the RoleRepo with an instance of the AmazonFarmerContext
        public RoleRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to retrieve roles based on search criteria, skip, and take
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

        // Method to retrieve modules with their permissions for a specific role
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
        public async Task<TblRole> getRoleByID(string RoleID)
        {
            return await _context.Roles.Where(x=>x.Id == RoleID).FirstOrDefaultAsync();
        }
        public async Task<TblRole> getRoleByEnum(ERoles role)
        {
            return await _context.Roles.Where(x=>x.eRole == role).FirstOrDefaultAsync();
        }
    }
}
