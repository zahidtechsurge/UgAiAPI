using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace AmazonFarmer.Infrastructure.Persistence.Seeding
{
    public static class DefaultUsers
    {
        public static async Task SeedDefaultUsersAsync(UserManager<TblUser> userManager, RoleManager<TblRole> roleManager, AmazonFarmerContext context)
        {
            if (await userManager.FindByNameAsync("syedtalha") == null)
            {
                var user = new TblUser
                {
                    Id = "a35b9178-2859-4826-9583-430c71887b341",
                    FirstName = "Syed Talha",
                    LastName = "Admin",
                    Email = "syed.talha@techsurgeinc.com",
                    UserName = "syedtalha",
                    NormalizedUserName = "syedtalha" ,
                    Designation = EDesignation.Admin
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            //if (await userManager.FindByNameAsync("employee") == null)
            //{
            //    var user = new TblUser
            //    {
            //        Id = "a35b9178-2859-4826-9583-430c71887b342",
            //        FirstName = "Syed Talha",
            //        LastName = "Employee",
            //        Email = "noreply@engro.com",
            //        UserName = "employee",
            //        NormalizedUserName = "employee",
            //        Designation = EDesignation.Employee
            //    };

            //    IdentityResult temp = await userManager.CreateAsync(user, "Bakar123@");
            //    if (temp.Succeeded == true)
            //    {
            //        await userManager.AddToRoleAsync(user, "Employee");
            //    }
            //}
            if (await userManager.FindByNameAsync("bakar") == null)
            {
                var user = new TblUser
                {
                    Id = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                    FirstName = "Bakar",
                    LastName = "Farmer",
                    Email = "noreply@engro.com",
                    UserName = "bakar",
                    PhoneNumber= "0305-2806778",
                    NormalizedUserName = "bakar" 
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    try
                    {  
                        await userManager.AddToRoleAsync(user, "Farmer");
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            if (await userManager.FindByNameAsync("Am@zonEngro") == null)
            {
                var user = new TblUser
                {
                    Id = "11040e4d-c19d-4ab5-abe3-837798768182",
                    FirstName = "One Link",
                    LastName = "onelink",
                    Email = "noreply@engro.com",
                    UserName = "Am@zonEngro",
                    NormalizedUserName = "onelink"
                };

                IdentityResult temp = await userManager.CreateAsync(user, "@Am@z0n$engr0+=");
                if (temp.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "OneLink");
                }
            }
            if (await userManager.FindByNameAsync("ts_manager") == null)
            {
                var user = new TblUser
                {
                    Id = "11040e4d-c19d-4ab5-abe3-837798768183",
                    FirstName = "Territory Sales",
                    LastName = "Manager",
                    Email = "noreply@engro.com",
                    UserName = "ts_manager",
                    NormalizedUserName = "ts_manager",
                    Designation = EDesignation.Territory_Sales_Officer
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "Territory Sales Officer");
                }
            }
            if (await userManager.FindByNameAsync("rs_manager") == null)
            {
                var user = new TblUser
                {
                    Id = "11040e4d-c19d-4ab5-abe3-837798768184",
                    FirstName = "Regional Sales",
                    LastName = "Manager",
                    Email = "noreply@engro.com",
                    UserName = "rs_manager",
                    NormalizedUserName = "rs_manager",
                    Designation = EDesignation.Regional_Sales_Manager
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "Regional Sales Officer");
                }
            }
        }
    }
}
