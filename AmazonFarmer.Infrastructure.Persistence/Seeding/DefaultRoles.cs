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
            await roleManager.CreateAsync(new TblRole("Admin",ERoles.Admin));
            await roleManager.CreateAsync(new TblRole("Employee", ERoles.Employee));
            await roleManager.CreateAsync(new TblRole("Farmer", ERoles.Farmer));
            await roleManager.CreateAsync(new TblRole("OneLink", ERoles.OneLink));

            //await roleManager.CreateAsync(new TblRole("Territory Sales Officer", ERoles.Territory_Sales_Officer));
            //await roleManager.CreateAsync(new TblRole("Regional Sales Manager", ERoles.Regional_Sales_Manager));
            //await roleManager.CreateAsync(new TblRole("National Sales Manager", ERoles.National_Sales_Manager));
        }
    }
}
