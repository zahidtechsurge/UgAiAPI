using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AmazonFarmer.Infrastructure.Persistence.Seeding
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAsync(UserManager<TblUser> userManager, RoleManager<TblRole> roleManager)
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
                    NormalizedUserName = "syedtalha",
                    Designation = EDesignation.Admin
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Engro123@");
                if (temp.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            if (await userManager.FindByNameAsync("employee") == null)
            {
                var user = new TblUser
                {
                    Id = "a35b9178-2859-4826-9583-430c71887b342",
                    FirstName = "Syed Talha",
                    LastName = "Employee",
                    Email = "noreply@engro.com",
                    UserName = "employee",
                    NormalizedUserName = "employee",
                    Designation = EDesignation.Employee
                };

                IdentityResult temp = await userManager.CreateAsync(user, "Engro123@");
                if (temp.Succeeded == true)
                {
                    //tblFarmerProfile profile = new tblFarmerProfile()
                    //{
                    //    UserID = user.Id,
                    //    Address1 = "some where in pakistan",
                    //    FatherName = "Syed Salman Ahmed",
                    //    CNICNumber = "42401-6487231-1",
                    //    NTNNumber = "0000000000",
                    //    OwnedLand = "0",
                    //    LeasedLand = "0",
                    //    TotalArea = 0,
                    //    CityID = 1,
                    //    DistrictID = 1,
                    //    SelectedLangCode = "EN"
                    //};
                    

                    await userManager.AddToRoleAsync(user, "Employee");
                }
            }
            //else
            //{
            //    var user = new TblUser
            //    {
            //        Id = "a35b9178-2859-4826-9583-430c71887b341",
            //        FirstName = "Syed Talha",
            //        LastName = "Admin",
            //        Email = "syed.talha@techsurgeinc.com",
            //        UserName = "syedtalha",
            //        NormalizedUserName = "syedtalha",
            //        Designation = EDesignation.Employee
            //    };

            //    IdentityResult temp = await userManager.UpdateAsync(user);
            //    temp = await userManager.ChangePasswordAsync(user, "123456789", "Engro123@");
            //    if (temp.Succeeded == true)
            //    {
            //        await userManager.RemoveFromRoleAsync(user, "Admin");
            //        await userManager.AddToRoleAsync(user, "Employee");
            //    }
            //}


        }
    }
}
