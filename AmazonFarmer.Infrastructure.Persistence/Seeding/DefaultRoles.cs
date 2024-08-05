using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Persistence.Seeding
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<TblUser> userManager, RoleManager<TblRole> roleManager)
        {
            await roleManager.CreateAsync(new TblRole("Admin"));
            await roleManager.CreateAsync(new TblRole("Employee"));
            await roleManager.CreateAsync(new TblRole("Farmer"));
        }
    }
}
