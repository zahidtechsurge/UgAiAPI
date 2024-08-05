using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IRoleRepo
    {
        Task<List<getRoles_Resp>> GetRoles(string search, int skip, int take);
        Task<List<PermissionWiseClaimsDTO>> getModules(string RoleID);
    }
}
