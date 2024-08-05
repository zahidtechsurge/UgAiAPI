using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IRoleRepo // Defining the interface for role repository
    {
        Task<List<getRoles_Resp>> GetRoles(string search, int skip, int take); // Method signature for retrieving roles
        Task<List<PermissionWiseClaimsDTO>> getModules(string RoleID); // Method signature for retrieving modules by role ID
        Task<TblRole> getRoleByID(string RoleID);
        Task<TblRole> getRoleByEnum(ERoles role);
    }
}
