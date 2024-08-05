using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AmazonFarmer.Helpers
{
    public static class ClaimExtensions
    {
        public static async Task AddPermissionClaim(this RoleManager<TblRole> roleManager,TblRole role,string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a=>a.Type == "Permission" && a.Value == permission)) 
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
