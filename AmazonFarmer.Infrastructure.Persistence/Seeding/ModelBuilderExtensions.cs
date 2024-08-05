//using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonFarmer.Infrastructure.Persistence.Seeding
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(AmazonFarmerContext context)
        {
            SeedModules(context);
            SeedPages(context);
            SeedClaimHeaders(context);
            SeedLangauges(context);
            SeedAttachmentTypes(context);
            SeedWarehouse(context);
            SeedWarehouseLanguages(context);
            SeedDistrict(context);
            SeedDistrictLanguages(context);
            SeedCities(context);
            SeedCityLangugaes(context);
            SeedBanner(context);
            SeedBannerLanguages(context);
            SeedTehsil(context);
            SeedTehsilLanguages(context);
            SeedIntro(context);
            SeedIntroLanguges(context);
            SeedSeasons(context);
            SeedSeasonTranslation(context);
            SeedCrops(context);
            SeedCropTranslation(context);
            SeedCropTimings(context);
            SeedCategory(context);
            SeedCategoryTranslation(context);
            SeedUnitOfMeasure(context);
            SeedUnitOfMeasureTranslation(context);
            SeedProduct(context);
            SeedProductTranslation(context);
            SeedServices(context);
            SeedServiceTranslation(context);
            SeedProductConsumptionMatrix(context);
            SeedNotifications(context);
            SeedFarmApplication(context);
            SeedFarm(context);
            SeedPlan(context);
            //SeedPlanCrop(context);
            SeedPlanProducts(context);
            //SeedOrder(context);
            //SeedOrderProduct(context);
            SeedSeasonMonths(context);
            SeedSeasonMonthTranslations(context);
            SeedWeather(context);
            SeedWeatherTranslation(context);
            SeedSequenceTranslation(context);
            SeedFarmer(context);
            //SeedAuthorityLetter(context);
            //SeedAuthorityLetterDetails(context);
            SeedReasons(context);
            SeedReasonTranslations(context);
            SeedFarmerCNIC(context);
            SeedFarmerNTN(context);


        }

        private static void SeedFarmer(AmazonFarmerContext context)
        {
            if (context.FarmerProfile.Count() > 0) return;


            using (var transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FarmerProfile ON");

                //Seesing Languages
                context.FarmerProfile.Add(
                    new tblFarmerProfile
                    {
                        ProfileID = 1,
                        UserID = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                        FatherName = "Izhar Ahmed",
                        CNICNumber = "42501-3638910-9",
                        NTNNumber = "0000000000",
                        STRNNumber = "0000000000",
                        CellNumber = "0305-2806778",
                        OwnedLand = "2000",
                        LeasedLand = "2000",
                        TotalArea = 4000,
                        DateOfBirth = "2000-01-30",
                        Address1 = "A-204 Gulshan-e-Hadidth Phase 1, Karachi",
                        CityID = 1,
                        isApproved = EFarmerProfileStatus.Approved,
                        DistrictID = 1,
                        SelectedLangCode = "EN",
                        ApprovedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture)
                    }
                    );

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FarmerProfile OFF");

                transaction.Commit();
            }

        }
        private static void SeedModules(AmazonFarmerContext context)
        {
            if (context.NavigationModules.Count() > 0) return;


            using (var transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblNavigationModule ON");

                //Seesing Languages
                context.NavigationModules.AddRange(
                new TblNavigationModule { ModuleId = 1, IsActive = EActivityStatus.Active, ModuleName = "Dashboard", ShowInMenu = true, ModuleOrder = 1 },
                new TblNavigationModule { ModuleId = 2, IsActive = EActivityStatus.Active, ModuleName = "Farmer Management", ShowInMenu = true, ModuleOrder = 2 },
                new TblNavigationModule { ModuleId = 3, IsActive = EActivityStatus.Active, ModuleName = "Role Management", ShowInMenu = true, ModuleOrder = 2 }
                );

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblNavigationModule OFF");

                transaction.Commit();
            }
        }
        private static void SeedPages(AmazonFarmerContext context)
        {

            if (context.Pages.Count() > 0) return;



            using (var transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblPage ON");

                //Seesing Permissions for different Modules
                context.Pages.AddRange(
                 new TblPage { PageID = 1, Controller = "Dashboard", ActionMethod = "Index", IsActive = EActivityStatus.Active, ModuleID = 1, PageIcon = "", PageName = "Dashboard", PageOrder = 1, PageUrl = "/Dashboard", ProjectModule = "DMS", ShowOnMenu = true },
                 new TblPage { PageID = 2, Controller = "Dashboard", ActionMethod = "Index", IsActive = EActivityStatus.Active, ModuleID = 1, PageIcon = "", PageName = "Dashboard", PageOrder = 2, PageUrl = "/Dashboard", ProjectModule = "DMS", ShowOnMenu = true },
                 //new TblPage { PageID = 3, Controller = "Farmer", IsActive = EActivityStatus.Active, ModuleID = 2, PageIcon = "", PageName = "Farmers", PageOrder = 3, PageUrl = "/Employee/Farmer", ProjectModule = "DMS", ShowOnMenu = true },
                 new TblPage { PageID = 3, Controller = "Farmer", ActionMethod = "Create", IsActive = EActivityStatus.Active, ModuleID = 2, PageIcon = "", PageName = "Create Farmer", PageOrder = 1, PageUrl = "/Employee/Farmer/Create", ProjectModule = "DMS", ShowOnMenu = true },
                 new TblPage { PageID = 4, Controller = "Farmer", ActionMethod = "Farmer", IsActive = EActivityStatus.Active, ModuleID = 2, PageIcon = "", PageName = "Farmers", PageOrder = 2, PageUrl = "/Employee/Farmer", ProjectModule = "DMS", ShowOnMenu = true },
                 new TblPage { PageID = 5, Controller = "RoleManager", ActionMethod = "Index", IsActive = EActivityStatus.Active, ModuleID = 3, PageIcon = "", PageName = "Role Manager", PageOrder = 1, PageUrl = "/RoleManager/", ProjectModule = "DMS", ShowOnMenu = true },
                 new TblPage { PageID = 6, Controller = "RoleManager", ActionMethod = "getRoles", IsActive = EActivityStatus.Active, ModuleID = 3, PageIcon = "", PageName = "get Roles", PageOrder = 2, PageUrl = "/RoleManager/getRoles", ProjectModule = "DMS", ShowOnMenu = false },
                 new TblPage { PageID = 7, Controller = "RoleManager", ActionMethod = "PermissionManager", IsActive = EActivityStatus.Active, ModuleID = 3, PageIcon = "", PageName = "Permission Manager", PageOrder = 3, PageUrl = "/RoleManager/PermissionManager", ProjectModule = "DMS", ShowOnMenu = false },
                 new TblPage { PageID = 8, Controller = "RoleManager", ActionMethod = "getPermissionsByRoleID", IsActive = EActivityStatus.Active, ModuleID = 3, PageIcon = "", PageName = "get Permissions By RoleID", PageOrder = 4, PageUrl = "/RoleManager/getPermissionsByRoleID", ProjectModule = "DMS", ShowOnMenu = false },
                 new TblPage { PageID = 9, Controller = "RoleManager", ActionMethod = "updatePermissions", IsActive = EActivityStatus.Active, ModuleID = 3, PageIcon = "", PageName = "update Permissions", PageOrder = 5, PageUrl = "/RoleManager/updatePermissions", ProjectModule = "DMS", ShowOnMenu = false }
             );

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblPage OFF");

                transaction.Commit();
            }

        }
        private static void SeedClaimHeaders(AmazonFarmerContext context)
        {
            if (context.Claims.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {
                //Seesing Permission groups of claim for product controller/page
                context.Claims.AddRange(
                    new TblClaim { ClaimId = "63d97de3-54ad-4d81-b6e5-3f94c4e22515", ClaimDescription = "Employee Dashboard", PageId = 1 },
                    new TblClaim { ClaimId = "e28bf186-99f1-49c2-aef1-1c88d020cc69", ClaimDescription = "Farmer Dashboard", PageId = 2 },
                    //new TblClaim { ClaimId = "ce61d733-694c-49be-87e1-9c245e85f33a", ClaimDescription = "Farmer Management", PageId = 3 },
                    new TblClaim { ClaimId = "e7b4543e-f884-4da2-a8a3-75189981bccf", ClaimDescription = "Create Farmer", PageId = 3 },
                    new TblClaim { ClaimId = "6ffc4e4b-c2b6-4fb1-b0f4-65d9472cba17", ClaimDescription = "Farmer Listing", PageId = 4 },
                    new TblClaim { ClaimId = "674b8bea-c21b-4a2b-a16b-8ac2fc382c32", ClaimDescription = "Role Manager", PageId = 5 },
                    new TblClaim { ClaimId = "6dea7f8b-a4af-436c-bdbd-bfe3dd8215d1", ClaimDescription = "get Roles", PageId = 6 },
                    new TblClaim { ClaimId = "0f845fc3-0fbb-4939-8f5f-cb720d9e9517", ClaimDescription = "Permission Manager", PageId = 7 },
                    new TblClaim { ClaimId = "f00c42ba-d2ae-4cee-bb55-ef9678bcfcb4", ClaimDescription = "get Permissions By RoleID", PageId = 8 },
                    new TblClaim { ClaimId = "0099a508-30eb-4d7c-9a43-49b7fa87c9f7", ClaimDescription = "update Permissions", PageId = 9 }
                );



                context.SaveChanges();

            }
        }
        private static void SeedProductClaimsPermissions(AmazonFarmerContext context)
        {
            if (context.ClaimActions.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                //Seesing Permission groups of claim for product controller/page
                context.ClaimActions.AddRange(
                    new TblClaimAction { ClaimActionId = "507ab462-2551-4aa3-abd7-88136e45692f", ClaimDescription = "index", ClaimId = "63d97de3-54ad-4d81-b6e5-3f94c4e22515" },
                    new TblClaimAction { ClaimActionId = "637b0e72-84fd-46da-9a41-9ddb7e313f72", ClaimDescription = "index", ClaimId = "e28bf186-99f1-49c2-aef1-1c88d020cc69" },
                    //new TblClaimAction { ClaimActionId = "14afc402-2b42-4d17-94d7-f1970f957427", ClaimDescription = "index", ClaimId = "ce61d733-694c-49be-87e1-9c245e85f33a" },
                    new TblClaimAction { ClaimActionId = "923ff1d6-9b30-4d42-adca-168c7454ec78", ClaimDescription = "index", ClaimId = "ce61d733-694c-49be-87e1-9c245e85f33a" },
                    new TblClaimAction { ClaimActionId = "86c627d0-8530-493a-a98f-33794d759619", ClaimDescription = "index", ClaimId = "e7b4543e-f884-4da2-a8a3-75189981bccf" },
                    new TblClaimAction { ClaimActionId = "e4ba7097-b608-40ce-ba91-e06436e69175", ClaimDescription = "index", ClaimId = "6ffc4e4b-c2b6-4fb1-b0f4-65d9472cba17" },
                    new TblClaimAction { ClaimActionId = "79b0e37f-024e-4d0e-be22-d62c41eaa14e", ClaimDescription = "index", ClaimId = "674b8bea-c21b-4a2b-a16b-8ac2fc382c32" },
                    new TblClaimAction { ClaimActionId = "e6143e40-afef-4d69-8849-a7316e724fd1", ClaimDescription = "getRoles", ClaimId = "6dea7f8b-a4af-436c-bdbd-bfe3dd8215d1" },
                    new TblClaimAction { ClaimActionId = "596f352f-3598-41e7-bc58-5053cc5ddc7d", ClaimDescription = "PermissionManager", ClaimId = "0f845fc3-0fbb-4939-8f5f-cb720d9e9517" },
                    new TblClaimAction { ClaimActionId = "2e9ba24d-cb30-4ce0-8851-e51fff95a882", ClaimDescription = "getPermissionsByRoleID", ClaimId = "f00c42ba-d2ae-4cee-bb55-ef9678bcfcb4" },
                    new TblClaimAction { ClaimActionId = "1bd27af4-356a-4b14-bbe0-a5cd86f702f6", ClaimDescription = "updatePermissions", ClaimId = "0099a508-30eb-4d7c-9a43-49b7fa87c9f7" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblClaimAction ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblClaimAction OFF"); transaction.Commit();
            }
        }
        private static void SeedLangauges(AmazonFarmerContext context)
        {
            if (context.Languages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                //Seesing Languages
                context.Languages.AddRange(
                    new tblLanguages
                    {
                        LanguageCode = "EN",
                        LanguageName = "English",
                        Status = EActivityStatus.Active
                    },
                    new tblLanguages
                    {
                        LanguageCode = "UR",
                        LanguageName = "اردو (Urdu)",
                        Status = EActivityStatus.Active
                    },
                    new tblLanguages
                    {
                        LanguageCode = "PA",
                        LanguageName = "پنجابی (Punjabi)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "PS",
                        LanguageName = "پښتو (Pashto)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "SD",
                        LanguageName = "سنڌي (Sindhi)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "SK",
                        LanguageName = "سرائیکی (Saraiki)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "BL",
                        LanguageName = "بلوچی (Balochi)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "KS",
                        LanguageName = "کٲشُر (Kashmiri)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "BH",
                        LanguageName = "بروہی (Brahui)",
                        Status = EActivityStatus.DeActive
                    },
                    new tblLanguages
                    {
                        LanguageCode = "SN",
                        LanguageName = "شینا (Shina)",
                        Status = EActivityStatus.DeActive
                    }
                );



                context.SaveChanges();

                transaction.Commit();
            }
        }
        private static void SeedAttachmentTypes(AmazonFarmerContext context)
        {
            if (context.AttachmentType.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.AttachmentType.AddRange(
                    new tblAttachmentTypes { ID = 1, TypeName = "User CNIC Attachment", AttachmentType = EAttachmentType.User_CNIC_Document },
                    new tblAttachmentTypes { ID = 2, TypeName = "User NTN Attachment", AttachmentType = EAttachmentType.User_NTN_Document },
                    new tblAttachmentTypes { ID = 3, TypeName = "Farm Document", AttachmentType = EAttachmentType.Farm_Document },
                    new tblAttachmentTypes { ID = 4, TypeName = "General Document", AttachmentType = EAttachmentType.General_Document },
                    new tblAttachmentTypes { ID = 5, TypeName = "Verify AuthorityLetter CNIC", AttachmentType = EAttachmentType.Verify_AuthorityLetter_NIC }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AttachmentType ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AttachmentType OFF"); transaction.Commit();
            }
        }
        private static void SeedCities(AmazonFarmerContext context)
        {
            if (context.Cities.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Cities.AddRange(
                    new tblCity { ID = 1, DistrictID = 1, CityCode="", Name = "Karachi", Status = EActivityStatus.Active },
                    new tblCity { ID = 2, DistrictID = 1, CityCode="", Name = "Hyderabad", Status = EActivityStatus.Active },
                    new tblCity { ID = 3, DistrictID = 1, CityCode="", Name = "Sukkur", Status = EActivityStatus.Active },
                    new tblCity { ID = 4, DistrictID = 1, CityCode="", Name = "Larkana", Status = EActivityStatus.Active },
                    new tblCity { ID = 5, DistrictID = 1, CityCode="", Name = "Nawabshah", Status = EActivityStatus.Active },
                    new tblCity { ID = 6, DistrictID = 1, CityCode="", Name = "Mirpur Khas", Status = EActivityStatus.Active },
                    new tblCity { ID = 7, DistrictID = 2, CityCode="", Name = "Lahore", Status = EActivityStatus.Active },
                    new tblCity { ID = 8, DistrictID = 2, CityCode="", Name = "Faisalabad", Status = EActivityStatus.Active },
                    new tblCity { ID = 9, DistrictID = 2, CityCode = "", Name = "Rawalpindi", Status = EActivityStatus.Active },
                    new tblCity { ID = 10, DistrictID = 2, CityCode="", Name = "Multan", Status = EActivityStatus.Active },
                    new tblCity { ID = 11, DistrictID = 2, CityCode="", Name = "Gujranwala", Status = EActivityStatus.Active },
                    new tblCity { ID = 12, DistrictID = 2, CityCode = "", Name = "Bahawalpur", Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Cities ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Cities OFF"); transaction.Commit();
            }
        }
        private static void SeedCityLangugaes(AmazonFarmerContext context)
        {
            if (context.CityLanguages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.CityLanguages.AddRange(
                    new tblCityLanguages { ID = 1, CityID = 1, LanguageCode = "EN", Translation = "Karachi" },
                    new tblCityLanguages { ID = 2, CityID = 2, LanguageCode = "EN", Translation = "Hyderabad" },
                    new tblCityLanguages { ID = 3, CityID = 3, LanguageCode = "EN", Translation = "Sukkur" },
                    new tblCityLanguages { ID = 4, CityID = 4, LanguageCode = "EN", Translation = "Larkana" },
                    new tblCityLanguages { ID = 5, CityID = 5, LanguageCode = "EN", Translation = "Nawabshah" },
                    new tblCityLanguages { ID = 6, CityID = 6, LanguageCode = "EN", Translation = "Mirpur Khas" },
                    new tblCityLanguages { ID = 7, CityID = 1, LanguageCode = "UR", Translation = "کراچی" },
                    new tblCityLanguages { ID = 8, CityID = 2, LanguageCode = "UR", Translation = "حیدرآباد" },
                    new tblCityLanguages { ID = 9, CityID = 3, LanguageCode = "UR", Translation = "شکر" },
                    new tblCityLanguages { ID = 10, CityID = 4, LanguageCode = "UR", Translation = "کانپنا" },
                    new tblCityLanguages { ID = 11, CityID = 5, LanguageCode = "UR", Translation = "نواب شاہ" },
                    new tblCityLanguages { ID = 12, CityID = 6, LanguageCode = "UR", Translation = "میرپور خاص" },
                    new tblCityLanguages { ID = 13, CityID = 7, LanguageCode = "EN", Translation = "Lahore" },
                    new tblCityLanguages { ID = 14, CityID = 8, LanguageCode = "EN", Translation = "Faisalabad" },
                    new tblCityLanguages { ID = 15, CityID = 9, LanguageCode = "EN", Translation = "Rawalpindi" },
                    new tblCityLanguages { ID = 16, CityID = 10, LanguageCode = "EN", Translation = "Multan" },
                    new tblCityLanguages { ID = 17, CityID = 11, LanguageCode = "EN", Translation = "Gujranwala" },
                    new tblCityLanguages { ID = 18, CityID = 12, LanguageCode = "EN", Translation = "Bahawalpur" },
                    new tblCityLanguages { ID = 19, CityID = 7, LanguageCode = "UR", Translation = "لاہور" },
                    new tblCityLanguages { ID = 20, CityID = 8, LanguageCode = "UR", Translation = "فیصل آباد" },
                    new tblCityLanguages { ID = 21, CityID = 9, LanguageCode = "UR", Translation = "راولپنڈی" },
                    new tblCityLanguages { ID = 22, CityID = 10, LanguageCode = "UR", Translation = "ملتان" },
                    new tblCityLanguages { ID = 23, CityID = 11, LanguageCode = "UR", Translation = "گرینولا" },
                    new tblCityLanguages { ID = 24, CityID = 12, LanguageCode = "UR", Translation = "بہاولپور" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CityLanguages ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CityLanguages OFF"); transaction.Commit();
            }
        }
        private static void SeedDistrict(AmazonFarmerContext context)
        {
            if (context.District.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.District.AddRange(
                    new tblDistrict { ID = 1, Name = "Sindh", Status = EActivityStatus.Active, DistrictCode = "SN" },
                    new tblDistrict { ID = 2, Name = "Punjab", Status = EActivityStatus.Active, DistrictCode = "PN" },
                    new tblDistrict { ID = 3, Name = "Khyber Pakhtunkhwa (KP)", Status = EActivityStatus.Active, DistrictCode = "KPK" },
                    new tblDistrict { ID = 4, Name = "Balochistan", Status = EActivityStatus.Active, DistrictCode = "KPK" },
                    new tblDistrict { ID = 5, Name = "Gilgit-Baltistan (GB)", Status = EActivityStatus.Active, DistrictCode = "KPK" },
                    new tblDistrict { ID = 6, Name = "Azad Jammu and Kashmir (AJK)", Status = EActivityStatus.Active, DistrictCode = "KPK" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT District ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT District OFF"); transaction.Commit();
            }
        }
        private static void SeedDistrictLanguages(AmazonFarmerContext context)
        {
            if (context.DistrictLanguages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.DistrictLanguages.AddRange(
                    new tblDistrictLanguages { ID = 1, DistrictID = 1, LanguageCode = "EN", Translation = "Sindh" },
                    new tblDistrictLanguages { ID = 2, DistrictID = 2, LanguageCode = "EN", Translation = "Punjab" },
                    new tblDistrictLanguages { ID = 3, DistrictID = 3, LanguageCode = "EN", Translation = "Khyber Pakhtunkhwa" },
                    new tblDistrictLanguages { ID = 4, DistrictID = 4, LanguageCode = "EN", Translation = "Balochistan" },
                    new tblDistrictLanguages { ID = 5, DistrictID = 5, LanguageCode = "EN", Translation = "Gilgit-Baltistan" },
                    new tblDistrictLanguages { ID = 6, DistrictID = 6, LanguageCode = "EN", Translation = "Azad Jammu and Kashmir" },
                    new tblDistrictLanguages { ID = 7, DistrictID = 1, LanguageCode = "UR", Translation = "سندھ" },
                    new tblDistrictLanguages { ID = 8, DistrictID = 2, LanguageCode = "UR", Translation = "پنجاب" },
                    new tblDistrictLanguages { ID = 9, DistrictID = 3, LanguageCode = "UR", Translation = "خیبر پختونخواہ" },
                    new tblDistrictLanguages { ID = 10, DistrictID = 4, LanguageCode = "UR", Translation = "بلوچستان" },
                    new tblDistrictLanguages { ID = 11, DistrictID = 5, LanguageCode = "UR", Translation = "گلگت بلتستان" },
                    new tblDistrictLanguages { ID = 12, DistrictID = 6, LanguageCode = "UR", Translation = "آزاد جموں و کشمیر" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DistrictLanguages ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DistrictLanguages OFF"); transaction.Commit();
            }
        }
        private static void SeedTehsil(AmazonFarmerContext context)
        {
            if (context.Tehsils.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Tehsils.AddRange(
                    new tblTehsil { ID = 1, CityID = 1, TehsilCode="", Name = "Karachi Central", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 2, CityID = 1, TehsilCode="", Name = "Karachi East", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 3, CityID = 1, TehsilCode="", Name = "Karachi West", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 4, CityID = 1, TehsilCode="", Name = "Karachi South", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 5, CityID = 1, TehsilCode="", Name = "Malir", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 6, CityID = 1, TehsilCode="", Name = "Korangi", Status = EActivityStatus.Active },
                    new tblTehsil { ID = 7, CityID = 2, TehsilCode = "", Name = "Hyderabad", Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tehsils ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tehsils OFF"); transaction.Commit();
            }
        }
        private static void SeedTehsilLanguages(AmazonFarmerContext context)
        {
            if (context.TehsilLanguages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.TehsilLanguages.AddRange(
                    new tblTehsilLanguages { ID = 1, LanguageCode = "EN", TehsilID = 1, Translation = "Karachi Central" },
                    new tblTehsilLanguages { ID = 2, LanguageCode = "UR", TehsilID = 1, Translation = "کراچی سینٹرل" },
                    new tblTehsilLanguages { ID = 3, LanguageCode = "EN", TehsilID = 2, Translation = "Karachi East" },
                    new tblTehsilLanguages { ID = 4, LanguageCode = "UR", TehsilID = 2, Translation = "کراچی ایسٹ" },
                    new tblTehsilLanguages { ID = 5, LanguageCode = "EN", TehsilID = 3, Translation = "Karachi West" },
                    new tblTehsilLanguages { ID = 6, LanguageCode = "UR", TehsilID = 3, Translation = "کراچی ویسٹ" },
                    new tblTehsilLanguages { ID = 7, LanguageCode = "EN", TehsilID = 4, Translation = "Karachi South" },
                    new tblTehsilLanguages { ID = 8, LanguageCode = "UR", TehsilID = 4, Translation = "کراچی جنوبی" },
                    new tblTehsilLanguages { ID = 9, LanguageCode = "EN", TehsilID = 5, Translation = "Malir" },
                    new tblTehsilLanguages { ID = 10, LanguageCode = "UR", TehsilID = 5, Translation = "ملیر" },
                    new tblTehsilLanguages { ID = 11, LanguageCode = "EN", TehsilID = 6, Translation = "Hyderabad" },
                    new tblTehsilLanguages { ID = 12, LanguageCode = "UR", TehsilID = 6, Translation = "حیدرآباد" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TehsilLanguages ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TehsilLanguages OFF"); transaction.Commit();
            }
        }
        private static void SeedWarehouse(AmazonFarmerContext context)
        {
            if (context.Warehouse.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Warehouse.AddRange(
                    new tblwarehouse { ID = 1, Name = "Burns Road Food Street",Address= "Burns Road Food Street", WHCode="BRFS", latitude = 24.8557115, longitude = 67.0138647, Status = EActivityStatus.Active },
                    new tblwarehouse { ID = 2, Name = "National Museum Of Pakistan", Address = "National Museum Of Pakistan", WHCode = "NMP", latitude = 24.8573786, longitude = 67.014654, Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Warehouse ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Warehouse OFF"); transaction.Commit();
            }
        }
        private static void SeedWarehouseLanguages(AmazonFarmerContext context)
        {
            if (context.WarehouseTranslation.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.WarehouseTranslation.AddRange(
                    new tblwarehouseTranslation { ID = 1, WarehouseID = 1, LanguageCode = "EN", Text = "WH-Eng-1" },
                    new tblwarehouseTranslation { ID = 2, WarehouseID = 2, LanguageCode = "EN", Text = "WH-Eng-2" },
                    new tblwarehouseTranslation { ID = 3, WarehouseID = 1, LanguageCode = "UR", Text = "WH-Ur-1" },
                    new tblwarehouseTranslation { ID = 4, WarehouseID = 2, LanguageCode = "UR", Text = "WH-Ur-2" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WarehouseTranslation ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WarehouseTranslation OFF"); transaction.Commit();
            }
        }
        private static void SeedBanner(AmazonFarmerContext context)
        {
            if (context.Banners.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Banners.AddRange(
                    new tblBanner { ID = 1, Name = "Login Screen Banner", BannerType = EBannerType.loginScreen, Status = EActivityStatus.Active },
                    new tblBanner { ID = 2, Name = "Home Screen Banner", BannerType = EBannerType.homeScreen, Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Banners ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Banners OFF"); transaction.Commit();
            }
        }
        private static void SeedBannerLanguages(AmazonFarmerContext context)
        {
            if (context.BannerLanguages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.BannerLanguages.AddRange(
                    new tblBannerLanguages { ID = 1, BannerID = 1, LanguageCode = "EN", Image = "/attachments/banners/loginScreenBanner.JPG" },
                    new tblBannerLanguages { ID = 2, BannerID = 1, LanguageCode = "UR", Image = "/attachments/banners/loginScreenBanner.JPG" },
                    new tblBannerLanguages { ID = 3, BannerID = 2, LanguageCode = "EN", Image = "/attachments/banners/homeScreenBanner.JPG" },
                    new tblBannerLanguages { ID = 4, BannerID = 2, LanguageCode = "UR", Image = "/attachments/banners/homeScreenBanner.JPG" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BannerLanguages ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BannerLanguages OFF"); transaction.Commit();
            }
        }
        private static void SeedIntro(AmazonFarmerContext context)
        {
            if (context.Intros.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Intros.AddRange(
                    new tblIntro { ID = 1, Name = "First", Status = EActivityStatus.Active },
                    new tblIntro { ID = 2, Name = "Second", Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Intros ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Intros OFF"); transaction.Commit();
            }
        }
        private static void SeedIntroLanguges(AmazonFarmerContext context)
        {
            if (context.IntroLanguages.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.IntroLanguages.AddRange(
                    new tblIntroLanguages
                    {
                        ID = 1,
                        IntroID = 1,
                        Text = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.",
                        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU",
                        LanguageCode = "EN"
                    },
                    new tblIntroLanguages
                    {
                        ID = 2,
                        IntroID = 1,
                        Text = "Lorem Ipsum کے اقتباسات کے بہت سے تغیرات دستیاب ہیں، لیکن اکثریت کو کسی نہ کسی شکل میں، انجکشن شدہ مزاح، یا بے ترتیب الفاظ کے ذریعے تبدیلی کا سامنا کرنا پڑا ہے جو قدرے قابل اعتبار بھی نہیں لگتے ہیں۔",
                        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU",
                        LanguageCode = "UR"
                    },
                    new tblIntroLanguages
                    {
                        ID = 3,
                        IntroID = 2,
                        Text = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.",
                        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU",
                        LanguageCode = "EN"
                    },
                    new tblIntroLanguages
                    {
                        ID = 4,
                        IntroID = 2,
                        Text = "Lorem Ipsum کے اقتباسات کے بہت سے تغیرات دستیاب ہیں، لیکن اکثریت کو کسی نہ کسی شکل میں، انجکشن شدہ مزاح، یا بے ترتیب الفاظ کے ذریعے تبدیلی کا سامنا کرنا پڑا ہے جو قدرے قابل اعتبار بھی نہیں لگتے ہیں۔",
                        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU",
                        LanguageCode = "UR"
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT IntroLanguages ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT IntroLanguages OFF"); transaction.Commit();
            }
        }
        private static void SeedCrops(AmazonFarmerContext context)
        {
            if (context.Crops.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Crops.AddRange(
                    new tblCrop { ID = 1, Name = "Wheat", Status = EActivityStatus.Active },
                    new tblCrop { ID = 2, Name = "Sunflower", Status = EActivityStatus.Active },
                    new tblCrop { ID = 3, Name = "Cotton", Status = EActivityStatus.Active },
                    new tblCrop { ID = 4, Name = "Sugarcane", Status = EActivityStatus.Active },
                    new tblCrop { ID = 5, Name = "Onion", Status = EActivityStatus.Active },
                    new tblCrop { ID = 6, Name = "Tomato", Status = EActivityStatus.Active },
                    new tblCrop { ID = 7, Name = "Potato", Status = EActivityStatus.Active },
                    new tblCrop { ID = 8, Name = "Rice", Status = EActivityStatus.Active },
                    new tblCrop { ID = 9, Name = "Chillies", Status = EActivityStatus.Active },
                    new tblCrop { ID = 10, Name = "Soyabean", Status = EActivityStatus.Active },
                    new tblCrop { ID = 11, Name = "Bajra", Status = EActivityStatus.Active },
                    new tblCrop { ID = 12, Name = "Jowar", Status = EActivityStatus.Active },
                    new tblCrop { ID = 13, Name = "Garlic", Status = EActivityStatus.Active },
                    new tblCrop { ID = 14, Name = "Coriander", Status = EActivityStatus.Active },
                    new tblCrop { ID = 15, Name = "Barley", Status = EActivityStatus.Active },
                    new tblCrop { ID = 16, Name = "Maize", Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Crops ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Crops OFF"); transaction.Commit();
            }
        }
        private static void SeedCropTranslation(AmazonFarmerContext context)
        {
            if (context.CropTranslation.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.CropTranslation.AddRange(
                    new tblCropTranslation { ID = 1, CropID = 1, LanguageCode = "EN", Text = "Wheat", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 2, CropID = 2, LanguageCode = "EN", Text = "Sunflower", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 3, CropID = 3, LanguageCode = "EN", Text = "Cotton", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 4, CropID = 4, LanguageCode = "EN", Text = "Sugarcane", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 5, CropID = 5, LanguageCode = "EN", Text = "Onion", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 6, CropID = 6, LanguageCode = "EN", Text = "Tomato", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 7, CropID = 7, LanguageCode = "EN", Text = "Potato", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 8, CropID = 8, LanguageCode = "EN", Text = "Rice", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 9, CropID = 9, LanguageCode = "EN", Text = "Chillies", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 10, CropID = 10, LanguageCode = "EN", Text = "Soyabean", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 11, CropID = 11, LanguageCode = "EN", Text = "Bajra", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 12, CropID = 12, LanguageCode = "EN", Text = "Jowar", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 13, CropID = 13, LanguageCode = "EN", Text = "Garlic", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 14, CropID = 14, LanguageCode = "EN", Text = "Coriander", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 15, CropID = 15, LanguageCode = "EN", Text = "Barley", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 16, CropID = 16, LanguageCode = "EN", Text = "Maize", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 17, CropID = 1, LanguageCode = "UR", Text = "گندم", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 18, CropID = 2, LanguageCode = "UR", Text = "سورج مکھی", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 19, CropID = 3, LanguageCode = "UR", Text = "کپاس", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 20, CropID = 4, LanguageCode = "UR", Text = "گنا", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 21, CropID = 5, LanguageCode = "UR", Text = "پیاز", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 22, CropID = 6, LanguageCode = "UR", Text = "ٹماٹر", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 23, CropID = 7, LanguageCode = "UR", Text = "آلو", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 24, CropID = 8, LanguageCode = "UR", Text = "چاول", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 25, CropID = 9, LanguageCode = "UR", Text = "مرچیں", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 26, CropID = 10, LanguageCode = "UR", Text = "سویا بین", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 27, CropID = 11, LanguageCode = "UR", Text = "باجرہ", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 28, CropID = 12, LanguageCode = "UR", Text = "جوار", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 29, CropID = 13, LanguageCode = "UR", Text = "Garlic", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 30, CropID = 14, LanguageCode = "UR", Text = "Coriander", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 31, CropID = 15, LanguageCode = "UR", Text = "Barley", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblCropTranslation { ID = 32, CropID = 16, LanguageCode = "UR", Text = "Maize", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CropTranslation ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CropTranslation OFF"); transaction.Commit();
            }
        }
        private static void SeedSeasons(AmazonFarmerContext context)
        {
            if (context.Season.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Season.AddRange(
                    new tblSeason { ID = 1, Name = "Rabbi", StartDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), EndDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1), Status = EActivityStatus.Active },
                    new tblSeason { ID = 2, Name = "Kharif", StartDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), EndDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1), Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Season ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Season OFF"); transaction.Commit();
            }
        }
        private static void SeedSeasonTranslation(AmazonFarmerContext context)
        {
            if (context.SeasonTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.SeasonTranslations.AddRange(
                    new tblSeasonTranslation { ID = 1, SeasonID = 1, LanguageCode = "EN", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Translation = "Rabbi" },
                    new tblSeasonTranslation { ID = 2, SeasonID = 2, LanguageCode = "EN", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Translation = "Kharif" },
                    new tblSeasonTranslation { ID = 3, SeasonID = 1, LanguageCode = "UR", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Translation = "ربی" },
                    new tblSeasonTranslation { ID = 4, SeasonID = 2, LanguageCode = "UR", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Translation = "خزاں" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT SeasonTranslations ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT SeasonTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedSeasonMonths(AmazonFarmerContext context)
        {
            if (context.Months.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Months.AddRange(
                    new tblMonth { ID = 1, Name = "Jan", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 4 },
                    new tblMonth { ID = 2, Name = "Feb", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 5 },
                    new tblMonth { ID = 3, Name = "Mar", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 6 },
                    new tblMonth { ID = 4, Name = "Apr", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 1 },
                    new tblMonth { ID = 5, Name = "May", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 2 },
                    new tblMonth { ID = 6, Name = "Jun", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 3 },
                    new tblMonth { ID = 7, Name = "Jul", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 4 },
                    new tblMonth { ID = 8, Name = "Aug", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 5 },
                    new tblMonth { ID = 9, Name = "Sept", SeasonID = 2, Status = EActivityStatus.Active, orderBy = 6 },
                    new tblMonth { ID = 10, Name = "Oct", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 1 },
                    new tblMonth { ID = 11, Name = "Nov", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 2 },
                    new tblMonth { ID = 12, Name = "Dec", SeasonID = 1, Status = EActivityStatus.Active, orderBy = 3 }
                );
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Months ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Months OFF"); transaction.Commit();
            }
        }
        private static void SeedSeasonMonthTranslations(AmazonFarmerContext context)
        {
            if (context.MonthTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.MonthTranslations.AddRange(
                    new tblMonthTranslation { ID = 1, MonthID = 1, LanguageCode = "EN", Text = "January" },
                    new tblMonthTranslation { ID = 2, MonthID = 2, LanguageCode = "EN", Text = "February" },
                    new tblMonthTranslation { ID = 3, MonthID = 3, LanguageCode = "EN", Text = "March" },
                    new tblMonthTranslation { ID = 4, MonthID = 4, LanguageCode = "EN", Text = "April" },
                    new tblMonthTranslation { ID = 5, MonthID = 5, LanguageCode = "EN", Text = "May" },
                    new tblMonthTranslation { ID = 6, MonthID = 6, LanguageCode = "EN", Text = "June" },
                    new tblMonthTranslation { ID = 7, MonthID = 7, LanguageCode = "EN", Text = "July" },
                    new tblMonthTranslation { ID = 8, MonthID = 8, LanguageCode = "EN", Text = "August" },
                    new tblMonthTranslation { ID = 9, MonthID = 9, LanguageCode = "EN", Text = "September" },
                    new tblMonthTranslation { ID = 10, MonthID = 10, LanguageCode = "EN", Text = "October" },
                    new tblMonthTranslation { ID = 11, MonthID = 11, LanguageCode = "EN", Text = "November" },
                    new tblMonthTranslation { ID = 12, MonthID = 12, LanguageCode = "EN", Text = "December" },
                    new tblMonthTranslation { ID = 13, MonthID = 1, LanguageCode = "UR", Text = "جنوری" },
                    new tblMonthTranslation { ID = 14, MonthID = 2, LanguageCode = "UR", Text = "فروری" },
                    new tblMonthTranslation { ID = 15, MonthID = 3, LanguageCode = "UR", Text = "مارچ" },
                    new tblMonthTranslation { ID = 16, MonthID = 4, LanguageCode = "UR", Text = "اپریل" },
                    new tblMonthTranslation { ID = 17, MonthID = 5, LanguageCode = "UR", Text = "مئی" },
                    new tblMonthTranslation { ID = 18, MonthID = 6, LanguageCode = "UR", Text = "جون" },
                    new tblMonthTranslation { ID = 19, MonthID = 7, LanguageCode = "UR", Text = "جولائی" },
                    new tblMonthTranslation { ID = 20, MonthID = 8, LanguageCode = "UR", Text = "اگست" },
                    new tblMonthTranslation { ID = 21, MonthID = 9, LanguageCode = "UR", Text = "ستمبر" },
                    new tblMonthTranslation { ID = 22, MonthID = 10, LanguageCode = "UR", Text = "اکتوبر" },
                    new tblMonthTranslation { ID = 23, MonthID = 11, LanguageCode = "UR", Text = "نومبر" },
                    new tblMonthTranslation { ID = 24, MonthID = 12, LanguageCode = "UR", Text = "دسمبر" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT MonthTranslations ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT MonthTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedCropTimings(AmazonFarmerContext context)
        {
            if (context.CropTimings.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.CropTimings.AddRange(
                    new tblCropTimings { ID = 1, CropID = 1, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 2, CropID = 2, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 3, CropID = 3, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 4, CropID = 4, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 5, CropID = 5, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 6, CropID = 6, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 7, CropID = 7, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 8, CropID = 8, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 9, CropID = 9, DistrictID = 1, SeasonID = 1, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 10, CropID = 10, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 11, CropID = 11, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 12, CropID = 12, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 13, CropID = 13, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 14, CropID = 14, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 15, CropID = 15, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) },
                    new tblCropTimings { ID = 16, CropID = 16, DistrictID = 1, SeasonID = 2, FromDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), ToDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddMonths(1) }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CropTimings ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CropTimings OFF"); transaction.Commit();
            }
        }
        private static void SeedCategory(AmazonFarmerContext context)
        {
            if (context.ProductCategory.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.ProductCategory.AddRange(
                    new tblProductCategory { ID = 1, Name = "UREA", Status = EActivityStatus.Active },
                    new tblProductCategory { ID = 2, Name = "DAP", Status = EActivityStatus.Active },
                    new tblProductCategory { ID = 3, Name = "Micronutrient", Status = EActivityStatus.Active },
                    new tblProductCategory { ID = 4, Name = "Potash", Status = EActivityStatus.Active }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductCategory ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductCategory OFF"); transaction.Commit();
            }
        }
        private static void SeedCategoryTranslation(AmazonFarmerContext context)
        {
            if (context.ProductCategoryTranslation.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.ProductCategoryTranslation.AddRange(
                    new tblProductCategoryTranslation { ID = 1, ProductCategoryID = 1, LanguageCode = "EN", Image = "", Text = "UREA" },
                    new tblProductCategoryTranslation { ID = 2, ProductCategoryID = 1, LanguageCode = "UR", Image = "", Text = "یوریا" },
                    new tblProductCategoryTranslation { ID = 3, ProductCategoryID = 2, LanguageCode = "EN", Image = "", Text = "DAP" },
                    new tblProductCategoryTranslation { ID = 4, ProductCategoryID = 2, LanguageCode = "UR", Image = "", Text = "ڈی اے پی" },
                    new tblProductCategoryTranslation { ID = 5, ProductCategoryID = 3, LanguageCode = "EN", Image = "", Text = "Micronutrient" },
                    new tblProductCategoryTranslation { ID = 6, ProductCategoryID = 3, LanguageCode = "UR", Image = "", Text = "مائیکرو نیوٹرینٹ" },
                    new tblProductCategoryTranslation { ID = 7, ProductCategoryID = 4, LanguageCode = "EN", Image = "", Text = "Potash" },
                    new tblProductCategoryTranslation { ID = 8, ProductCategoryID = 4, LanguageCode = "UR", Image = "", Text = "پوٹاش" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductCategoryTranslation ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductCategoryTranslation OFF"); transaction.Commit();
            }
        }
        private static void SeedUnitOfMeasure(AmazonFarmerContext context)
        {
            if (context.tblUnitOfMeasures.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.tblUnitOfMeasures.Add(new tblUnitOfMeasure { ID = 1, UOM = "Bags", Status = EActivityStatus.Active });

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblUnitOfMeasures ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT tblUnitOfMeasures OFF"); transaction.Commit();
            }
        }
        private static void SeedUnitOfMeasureTranslation(AmazonFarmerContext context)
        {
            if (context.UnitOfMeasureTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.UnitOfMeasureTranslations.AddRange(
                    new tblUnitOfMeasureTranslation { ID = 1, UOMID = 1, LanguageCode = "EN", Text = "Bags" },
                    new tblUnitOfMeasureTranslation { ID = 2, UOMID = 1, LanguageCode = "UR", Text = "بوریان" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UnitOfMeasureTranslations ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UnitOfMeasureTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedProduct(AmazonFarmerContext context)
        {
            if (context.Products.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Products.AddRange(
                    new TblProduct { ID = 1, SalesOrg = "2000", CategoryID = 1, UOMID = 1, Name = "UREA", ProductCode = "00001", CreatedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), Active = EActivityStatus.Active },
                    new TblProduct { ID = 2, SalesOrg = "2000", CategoryID = 1, UOMID = 1, Name = "Ammonium Sulphate", ProductCode = "00002", CreatedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), Active = EActivityStatus.Active },
                    new TblProduct { ID = 3, SalesOrg = "2000", CategoryID = 2, UOMID = 1, Name = "Zabardast Urea", ProductCode = "00003", CreatedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture), Active = EActivityStatus.Active }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Products ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Products OFF"); transaction.Commit();
            }
        }
        private static void SeedProductTranslation(AmazonFarmerContext context)
        {
            if (context.ProductTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.ProductTranslations.AddRange(
                    new tblProductTranslation { ID = 1, ProductID = 1, LanguageCode = "EN", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "UREA" },
                    new tblProductTranslation { ID = 2, ProductID = 1, LanguageCode = "UR", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "یوریا" },
                    new tblProductTranslation { ID = 3, ProductID = 2, LanguageCode = "EN", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "Ammonium Sulphate" },
                    new tblProductTranslation { ID = 4, ProductID = 2, LanguageCode = "UR", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "امونیم سلفیٹ" },
                    new tblProductTranslation { ID = 5, ProductID = 3, LanguageCode = "EN", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "Zabardast Urea" },
                    new tblProductTranslation { ID = 6, ProductID = 3, LanguageCode = "UR", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png", Text = "بھاری یوریا" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductTranslations ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedServices(AmazonFarmerContext context)
        {
            if (context.Service.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Service.AddRange(
                    new tblService { ID = 1, Name = "Soil Sampling", Active = EActivityStatus.Active },
                    new tblService { ID = 2, Name = "Geofencing", Active = EActivityStatus.Active },
                    new tblService { ID = 3, Name = "Drone Footage", Active = EActivityStatus.Active }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Service ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Service OFF"); transaction.Commit();
            }
        }
        private static void SeedServiceTranslation(AmazonFarmerContext context)
        {
            if (context.ServiceTranslation.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.ServiceTranslation.AddRange(
                    new tblServiceTranslation { ID = 1, ServiceID = 1, LanguageCode = "EN", Text = "Soil Sampling", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblServiceTranslation { ID = 2, ServiceID = 1, LanguageCode = "UR", Text = "مٹی کے نمونے لینے", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblServiceTranslation { ID = 3, ServiceID = 2, LanguageCode = "EN", Text = "Geofencing", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblServiceTranslation { ID = 4, ServiceID = 2, LanguageCode = "UR", Text = "جیوفینسنگ", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblServiceTranslation { ID = 5, ServiceID = 3, LanguageCode = "EN", Text = "Drone Footage", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" },
                    new tblServiceTranslation { ID = 6, ServiceID = 3, LanguageCode = "UR", Text = "ڈرون فوٹیج", Image = "http://207.198.103.185:88/lib/amazonfarmer/crop.png" }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ServiceTranslation ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ServiceTranslation OFF"); transaction.Commit();
            }
        }
        private static void SeedProductConsumptionMatrix(AmazonFarmerContext context)
        {
            if (context.ProductConsumptionMetric.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.ProductConsumptionMetric.AddRange(
                    new tblProductConsumptionMetrics { ID = 1, CropID = 1, ProductID = 1, TerritoryID = 1, UOM = "Bags", Usage = Convert.ToDecimal(1.2) },
                    new tblProductConsumptionMetrics { ID = 2, CropID = 1, ProductID = 2, TerritoryID = 1, UOM = "Bags", Usage = Convert.ToDecimal(2) }
                );

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductConsumptionMetric ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductConsumptionMetric OFF"); transaction.Commit();
            }
        }
        private static void SeedNotifications(AmazonFarmerContext context)
        {
            //if (context.EmailNotifications.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            //{

            //    context.EmailNotifications.AddRange(
            //        new tblEmailNotifications { ID = 1, Title = "Signup on Amazon Farmer", Body = "congratulations, <br> You're successfully signed up on Amazon Farmer by the username: [username]", Type = ENotificationBody.signup, Status = EActivityStatus.Active },
            //        new tblEmailNotifications { ID = 2, Title = "Signin on Amazon Farmer", Body = "Dear [firstname] <br> You've successfully logged in on Amazon Farmer.", Type = ENotificationBody.signin, Status = EActivityStatus.Active },
            //        new tblEmailNotifications { ID = 3, Title = "Farm Registration Request on Amazon Farmer", Body = "Dear [firstname] <br/> Your Farm registration request has been submitted. <br/> Your Application ID is: [applicationId]", Type = ENotificationBody.farmRequestIsInPending, Status = EActivityStatus.Active },
            //        new tblEmailNotifications { ID = 4, Title = "Farm Registration Request on Amazon Farmer", Body = "Dear [firstname] <br/> Your Farm registration request has been added in Draft, You can make changes anytime you want while your request is in this state. <br/> Your Application ID is: [applicationId]", Type = ENotificationBody.farmChangeRequest, Status = EActivityStatus.Active },
            //        new tblEmailNotifications { ID = 5, Title = "OTP Request for Amazon Farmer", Body = "Dear [firstname] <br/>, Your verification code is: [OTP]", Type = ENotificationBody.OTP, Status = EActivityStatus.Active },
            //        new tblEmailNotifications { ID = 6, Title = "Authority Letter – Order Number: [OrderID] -  Authority #:[AuthorityLetterNo]", Body = "Dear [firstname] <br/>, Authority Letter – Order Number: [OrderID] -  Authority #:[AuthorityLetterNo]", Type = ENotificationBody.AuthorityLetter_Warehouse, Status = EActivityStatus.Active }
            //    );

            //    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EmailNotifications ON");

            //    context.SaveChanges();

            //    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EmailNotifications OFF"); transaction.Commit();
            //}
        }
        private static void SeedFarmApplication(AmazonFarmerContext context)
        {
            if (context.FarmApplication.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.FarmApplication.AddRange(
                    new tblFarmApplication
                    {
                        ID = 1,
                        ApplicationTypeID = EApplicationType.Farm_Application
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT FarmApplication ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT FarmApplication OFF"); transaction.Commit();
            }
        }
        private static void SeedFarm(AmazonFarmerContext context)
        {
            if (context.Farms.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Farms.AddRange(
                    new tblfarm
                    {
                        FarmID = 1,
                        UserID = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                        FarmName = "Bakar poultry farm",
                        Address1 = "Dawood Center",
                        Address2 = "R2WF+C7C, MT Khan Rd, Civil Lines Karachi, Karachi City, Sindh, Pakistan",
                        latitude = 24.8460606,
                        longitude = 67.020563,
                        CityID = 1,
                        DistrictID = 1,
                        TehsilID = 1,
                        Acreage = 2000,
                        isLeased = true,
                        isPrimary = true,
                        Status = EFarmStatus.Approved,
                        isApproved = true,
                        isFarmApprovalAcknowledged = true,
                        isFarmApprovalAcknowledgedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddHours(2),
                        ApplicationID = 1,
                        UpdatedBy = "a35b9178-2859-4826-9583-430c71887b342",
                        UpdatedOn = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture).AddHours(1),
                        SAPFarmCode = string.Empty
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Farms ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Farms OFF"); transaction.Commit();
            }
        }
        private static void SeedPlan(AmazonFarmerContext context)
        {
            if (context.Plan.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Plan.AddRange(
                    new tblPlan
                    {
                        ID = 1,
                        UserID = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                        FarmID = 1,
                        SeasonID = 1,
                        Status = EPlanStatus.TSOProcessing,
                        WarehouseID = 1
                    }
                );



                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Plan] ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Plan] OFF"); transaction.Commit();
            }
        }

        

        private static void SeedPlanProducts(AmazonFarmerContext context)
        {
            if (context.PlanProduct.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.PlanProduct.AddRange(
                    new tblPlanProduct { ID = 1, PlanCropID = 1, ProductID = 1, Qty = 1000, Date = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture) },
                    new tblPlanProduct { ID = 2, PlanCropID = 1, ProductID = 2, Qty = 1000, Date = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture) },
                    new tblPlanProduct { ID = 3, PlanCropID = 2, ProductID = 2, Qty = 1000, Date = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture) },
                    new tblPlanProduct { ID = 4, PlanCropID = 2, ProductID = 3, Qty = 1000, Date = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture) }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PlanProduct ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PlanProduct OFF"); transaction.Commit();
            }
        }
          
        private static void SeedWeather(AmazonFarmerContext context)
        {
            if (context.WeatherIcon.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.WeatherIcon.AddRange(
                    new tblWeatherIcon { ID = 1, WeatherType = EWeatherType.Sunny, Title = "Sunny", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 2, WeatherType = EWeatherType.Mostly_Sunny, Title = "Mostly Sunny", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 3, WeatherType = EWeatherType.Partly_Sunny, Title = "Partly Sunny", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 4, WeatherType = EWeatherType.Partly_Cloudy, Title = "Intermittent Clouds", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 5, WeatherType = EWeatherType.Hazy_Sunshine, Title = "Hazy Sunshine", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 6, WeatherType = EWeatherType.Mostly_Cloudy, Title = "Mostly Cloudy", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 7, WeatherType = EWeatherType.Cloudy, Title = "Cloudy", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 8, WeatherType = EWeatherType.Dreary_Overcast, Title = "Dreary (Overcast)", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 9, WeatherType = EWeatherType.Fog, Title = "Fog", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 10, WeatherType = EWeatherType.Showers, Title = "Showers", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 11, WeatherType = EWeatherType.Mostly_Cloudy_Showers, Title = "Mostly Cloudy w/ Showers", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 12, WeatherType = EWeatherType.Partly_Cloudy_Showers, Title = "Partly Sunny w/ Showers", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 13, WeatherType = EWeatherType.T_Storms, Title = "T-Storms", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 14, WeatherType = EWeatherType.Mostly_Cloudy_Storm, Title = "Mostly Cloudy w/ T-Storms", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 15, WeatherType = EWeatherType.Partly_Sunny_Storms, Title = "Partly Sunny w/ T-Storms", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 16, WeatherType = EWeatherType.Rain, Title = "Rain", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 17, WeatherType = EWeatherType.Flurries, Title = "Flurries", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 18, WeatherType = EWeatherType.Mostly_Cloudy_Flurries, Title = "Mostly Cloudy w/ Flurries", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 19, WeatherType = EWeatherType.Partly_Sunny_Flurries, Title = "Partly Sunny w/ Flurries", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 20, WeatherType = EWeatherType.Snow, Title = "Snow", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 21, WeatherType = EWeatherType.Mostly_Cloudy_Snow, Title = "Mostly Cloudy w/ Snow", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 22, WeatherType = EWeatherType.Ice, Title = "Ice", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 23, WeatherType = EWeatherType.Sleet, Title = "Sleet", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 24, WeatherType = EWeatherType.Freezing_Rain, Title = "Freezing Rain", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 25, WeatherType = EWeatherType.Rain_and_Snow, Title = "Rain and Snow", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 26, WeatherType = EWeatherType.Hot, Title = "Hot", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 27, WeatherType = EWeatherType.Cold, Title = "Cold", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 28, WeatherType = EWeatherType.Windy, Title = "Windy", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 29, WeatherType = EWeatherType.Clear, Title = "Clear", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 30, WeatherType = EWeatherType.Mostly_Clear, Title = "Mostly Clear", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 31, WeatherType = EWeatherType.Partly_Cloudy, Title = "Partly Cloudy", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 32, WeatherType = EWeatherType.Intermittent_Clouds, Title = "Intermittent Clouds", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 33, WeatherType = EWeatherType.Hazy_Moonlight, Title = "Hazy Moonlight", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 34, WeatherType = EWeatherType.Night_Mostly_Cloudy, Title = "Mostly Cloudy (Night)", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 35, WeatherType = EWeatherType.Partly_Cloudy_w_Showers, Title = "Partly Cloudy w/ Showers", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 36, WeatherType = EWeatherType.Mostly_Cloudy_w_Showers, Title = "Mostly Cloudy w/ Showers", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 37, WeatherType = EWeatherType.Partly_Cloudy_Storms, Title = "Partly Cloudy w/ T-Storms", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 38, WeatherType = EWeatherType.Mostly_Cloudy_Storms, Title = "Mostly Cloudy w/ T-Storms", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 39, WeatherType = EWeatherType.Mostly_Cloudy_w_Flurries, Title = "Mostly Cloudy w/ Flurries", Status = EActivityStatus.Active },
                    new tblWeatherIcon { ID = 40, WeatherType = EWeatherType.Mostly_Cloudy_w_Snow, Title = "Mostly Cloudy w/ Snow", Status = EActivityStatus.Active }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WeatherIcon ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WeatherIcon OFF"); transaction.Commit();
            }
        }
        private static void SeedWeatherTranslation(AmazonFarmerContext context)
        {
            if (context.WeatherIconTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.WeatherIconTranslations.AddRange(
                    new tblWeatherIconTranslation { ID = 1, WeatherID = 1, LanguageCode = "EN", Text = "Sunny", Image = "/attachments/weather-icons/01-s.png" },
                    new tblWeatherIconTranslation { ID = 2, WeatherID = 2, LanguageCode = "EN", Text = "Mostly Sunny", Image = "/attachments/weather-icons/02-s.png" },
                    new tblWeatherIconTranslation { ID = 3, WeatherID = 3, LanguageCode = "EN", Text = "Partly Sunny", Image = "/attachments/weather-icons/03-s.png" },
                    new tblWeatherIconTranslation { ID = 4, WeatherID = 4, LanguageCode = "EN", Text = "Intermittent Clouds", Image = "/attachments/weather-icons/04-s.png" },
                    new tblWeatherIconTranslation { ID = 5, WeatherID = 5, LanguageCode = "EN", Text = "Hazy Sunshine", Image = "/attachments/weather-icons/05-s.png" },
                    new tblWeatherIconTranslation { ID = 6, WeatherID = 6, LanguageCode = "EN", Text = "Mostly Cloudy", Image = "/attachments/weather-icons/06-s.png" },
                    new tblWeatherIconTranslation { ID = 7, WeatherID = 7, LanguageCode = "EN", Text = "Cloudy", Image = "/attachments/weather-icons/07-s.png" },
                    new tblWeatherIconTranslation { ID = 8, WeatherID = 8, LanguageCode = "EN", Text = "Dreary (Overcast)", Image = "/attachments/weather-icons/08-s.png" },
                    new tblWeatherIconTranslation { ID = 9, WeatherID = 9, LanguageCode = "EN", Text = "Fog", Image = "/attachments/weather-icons/11-s.png" },
                    new tblWeatherIconTranslation { ID = 10, WeatherID = 10, LanguageCode = "EN", Text = "Showers", Image = "/attachments/weather-icons/12-s.png" },
                    new tblWeatherIconTranslation { ID = 11, WeatherID = 11, LanguageCode = "EN", Text = "Mostly Cloudy w/ Showers", Image = "/attachments/weather-icons/13-s.png" },
                    new tblWeatherIconTranslation { ID = 12, WeatherID = 12, LanguageCode = "EN", Text = "Partly Sunny w/ Showers", Image = "/attachments/weather-icons/14-s.png" },
                    new tblWeatherIconTranslation { ID = 13, WeatherID = 13, LanguageCode = "EN", Text = "T-Storms", Image = "/attachments/weather-icons/15-s.png" },
                    new tblWeatherIconTranslation { ID = 14, WeatherID = 14, LanguageCode = "EN", Text = "Mostly Cloudy w/ T-Storms", Image = "/attachments/weather-icons/16-s.png" },
                    new tblWeatherIconTranslation { ID = 15, WeatherID = 15, LanguageCode = "EN", Text = "Partly Sunny w/ T-Storms", Image = "/attachments/weather-icons/17-s.png" },
                    new tblWeatherIconTranslation { ID = 16, WeatherID = 16, LanguageCode = "EN", Text = "Rain", Image = "/attachments/weather-icons/18-s.png" },
                    new tblWeatherIconTranslation { ID = 17, WeatherID = 17, LanguageCode = "EN", Text = "Flurries", Image = "/attachments/weather-icons/19-s.png" },
                    new tblWeatherIconTranslation { ID = 18, WeatherID = 18, LanguageCode = "EN", Text = "Mostly Cloudy w/ Flurries", Image = "/attachments/weather-icons/20-s.png" },
                    new tblWeatherIconTranslation { ID = 19, WeatherID = 19, LanguageCode = "EN", Text = "Partly Sunny w/ Flurries", Image = "/attachments/weather-icons/21-s.png" },
                    new tblWeatherIconTranslation { ID = 20, WeatherID = 20, LanguageCode = "EN", Text = "Snow", Image = "/attachments/weather-icons/22-s.png" },
                    new tblWeatherIconTranslation { ID = 21, WeatherID = 21, LanguageCode = "EN", Text = "Mostly Cloudy w/ Snow", Image = "/attachments/weather-icons/23-s.png" },
                    new tblWeatherIconTranslation { ID = 22, WeatherID = 22, LanguageCode = "EN", Text = "Ice", Image = "/attachments/weather-icons/24-s.png" },
                    new tblWeatherIconTranslation { ID = 23, WeatherID = 23, LanguageCode = "EN", Text = "Sleet", Image = "/attachments/weather-icons/25-s.png" },
                    new tblWeatherIconTranslation { ID = 24, WeatherID = 24, LanguageCode = "EN", Text = "Freezing Rain", Image = "/attachments/weather-icons/26-s.png" },
                    new tblWeatherIconTranslation { ID = 25, WeatherID = 25, LanguageCode = "EN", Text = "Rain and Snow", Image = "/attachments/weather-icons/29-s.png" },
                    new tblWeatherIconTranslation { ID = 26, WeatherID = 26, LanguageCode = "EN", Text = "Hot", Image = "/attachments/weather-icons/30-s.png" },
                    new tblWeatherIconTranslation { ID = 27, WeatherID = 27, LanguageCode = "EN", Text = "Cold", Image = "/attachments/weather-icons/31-s.png" },
                    new tblWeatherIconTranslation { ID = 28, WeatherID = 28, LanguageCode = "EN", Text = "Windy", Image = "/attachments/weather-icons/32-s.png" },
                    new tblWeatherIconTranslation { ID = 29, WeatherID = 29, LanguageCode = "EN", Text = "Clear", Image = "/attachments/weather-icons/33-s.png" },
                    new tblWeatherIconTranslation { ID = 30, WeatherID = 30, LanguageCode = "EN", Text = "Mostly Clear", Image = "/attachments/weather-icons/34-s.png" },
                    new tblWeatherIconTranslation { ID = 31, WeatherID = 31, LanguageCode = "EN", Text = "Partly Cloudy", Image = "/attachments/weather-icons/35-s.png" },
                    new tblWeatherIconTranslation { ID = 32, WeatherID = 32, LanguageCode = "EN", Text = "Intermittent Clouds", Image = "/attachments/weather-icons/36-s.png" },
                    new tblWeatherIconTranslation { ID = 33, WeatherID = 33, LanguageCode = "EN", Text = "Hazy Moonlight", Image = "/attachments/weather-icons/37-s.png" },
                    new tblWeatherIconTranslation { ID = 34, WeatherID = 34, LanguageCode = "EN", Text = "Mostly Cloudy (Night)", Image = "/attachments/weather-icons/38-s.png" },
                    new tblWeatherIconTranslation { ID = 35, WeatherID = 35, LanguageCode = "EN", Text = "Partly Cloudy w/ Showers", Image = "/attachments/weather-icons/39-s.png" },
                    new tblWeatherIconTranslation { ID = 36, WeatherID = 36, LanguageCode = "EN", Text = "Mostly Cloudy w/ Showers", Image = "/attachments/weather-icons/40-s.png" },
                    new tblWeatherIconTranslation { ID = 37, WeatherID = 37, LanguageCode = "EN", Text = "Partly Cloudy w/ T-Storms", Image = "/attachments/weather-icons/41-s.png" },
                    new tblWeatherIconTranslation { ID = 38, WeatherID = 38, LanguageCode = "EN", Text = "Mostly Cloudy w/ T-Storms", Image = "/attachments/weather-icons/42-s.png" },
                    new tblWeatherIconTranslation { ID = 39, WeatherID = 39, LanguageCode = "EN", Text = "Mostly Cloudy w/ Flurries", Image = "/attachments/weather-icons/43-s.png" },
                    new tblWeatherIconTranslation { ID = 40, WeatherID = 40, LanguageCode = "EN", Text = "Mostly Cloudy w/ Snow", Image = "/attachments/weather-icons/44-s.png" },
                    new tblWeatherIconTranslation { ID = 41, WeatherID = 1, LanguageCode = "UR", Text = "دھوپ", Image = "/attachments/weather-icons/01-s.png" },
                    new tblWeatherIconTranslation { ID = 42, WeatherID = 2, LanguageCode = "UR", Text = "زیادہ تر سنی", Image = "/attachments/weather-icons/02-s.png" },
                    new tblWeatherIconTranslation { ID = 43, WeatherID = 3, LanguageCode = "UR", Text = "جزوی طور پر دھوپ", Image = "/attachments/weather-icons/03-s.png" },
                    new tblWeatherIconTranslation { ID = 44, WeatherID = 4, LanguageCode = "UR", Text = "وقفے وقفے سے بادل", Image = "/attachments/weather-icons/04-s.png" },
                    new tblWeatherIconTranslation { ID = 45, WeatherID = 5, LanguageCode = "UR", Text = "دھندلی دھوپ", Image = "/attachments/weather-icons/05-s.png" },
                    new tblWeatherIconTranslation { ID = 46, WeatherID = 6, LanguageCode = "UR", Text = "زیادہ تر ابر آلود", Image = "/attachments/weather-icons/06-s.png" },
                    new tblWeatherIconTranslation { ID = 47, WeatherID = 7, LanguageCode = "UR", Text = "ابر آلود", Image = "/attachments/weather-icons/07-s.png" },
                    new tblWeatherIconTranslation { ID = 48, WeatherID = 8, LanguageCode = "UR", Text = "ڈریری (آب آلود)", Image = "/attachments/weather-icons/08-s.png" },
                    new tblWeatherIconTranslation { ID = 49, WeatherID = 9, LanguageCode = "UR", Text = "دھند", Image = "/attachments/weather-icons/11-s.png" },
                    new tblWeatherIconTranslation { ID = 50, WeatherID = 10, LanguageCode = "UR", Text = "بارش", Image = "/attachments/weather-icons/12-s.png" },
                    new tblWeatherIconTranslation { ID = 51, WeatherID = 11, LanguageCode = "UR", Text = "بارش کے ساتھ زیادہ تر ابر آلود", Image = "/attachments/weather-icons/13-s.png" },
                    new tblWeatherIconTranslation { ID = 52, WeatherID = 12, LanguageCode = "UR", Text = "بارش کے ساتھ جزوی طور پر دھوپ", Image = "/attachments/weather-icons/14-s.png" },
                    new tblWeatherIconTranslation { ID = 53, WeatherID = 13, LanguageCode = "UR", Text = "گرج چمک", Image = "/attachments/weather-icons/15-s.png" },
                    new tblWeatherIconTranslation { ID = 54, WeatherID = 14, LanguageCode = "UR", Text = "T-طوفانوں کے ساتھ زیادہ تر ابر آلود", Image = "/attachments/weather-icons/16-s.png" },
                    new tblWeatherIconTranslation { ID = 55, WeatherID = 15, LanguageCode = "UR", Text = "T-طوفان کے ساتھ جزوی طور پر دھوپ", Image = "/attachments/weather-icons/17-s.png" },
                    new tblWeatherIconTranslation { ID = 56, WeatherID = 16, LanguageCode = "UR", Text = "بارش", Image = "/attachments/weather-icons/18-s.png" },
                    new tblWeatherIconTranslation { ID = 57, WeatherID = 17, LanguageCode = "UR", Text = "جھڑپیں", Image = "/attachments/weather-icons/19-s.png" },
                    new tblWeatherIconTranslation { ID = 58, WeatherID = 18, LanguageCode = "UR", Text = "زیادہ تر بادل چھائے ہوئے ہیں", Image = "/attachments/weather-icons/20-s.png" },
                    new tblWeatherIconTranslation { ID = 59, WeatherID = 19, LanguageCode = "UR", Text = "ہلچل کے ساتھ جزوی طور پر دھوپ", Image = "/attachments/weather-icons/21-s.png" },
                    new tblWeatherIconTranslation { ID = 60, WeatherID = 20, LanguageCode = "UR", Text = "برف", Image = "/attachments/weather-icons/22-s.png" },
                    new tblWeatherIconTranslation { ID = 61, WeatherID = 21, LanguageCode = "UR", Text = "زیادہ تر ابر آلود اور برفباری", Image = "/attachments/weather-icons/23-s.png" },
                    new tblWeatherIconTranslation { ID = 62, WeatherID = 22, LanguageCode = "UR", Text = "برف", Image = "/attachments/weather-icons/24-s.png" },
                    new tblWeatherIconTranslation { ID = 63, WeatherID = 23, LanguageCode = "UR", Text = "برف کے تودے", Image = "/attachments/weather-icons/25-s.png" },
                    new tblWeatherIconTranslation { ID = 64, WeatherID = 24, LanguageCode = "UR", Text = "برفیلی بارش", Image = "/attachments/weather-icons/26-s.png" },
                    new tblWeatherIconTranslation { ID = 65, WeatherID = 25, LanguageCode = "UR", Text = "بارش اور برفباری۔", Image = "/attachments/weather-icons/29-s.png" },
                    new tblWeatherIconTranslation { ID = 66, WeatherID = 26, LanguageCode = "UR", Text = "گرم", Image = "/attachments/weather-icons/30-s.png" },
                    new tblWeatherIconTranslation { ID = 67, WeatherID = 27, LanguageCode = "UR", Text = "ٹھنڈا۔", Image = "/attachments/weather-icons/31-s.png" },
                    new tblWeatherIconTranslation { ID = 68, WeatherID = 28, LanguageCode = "UR", Text = "آندھی", Image = "/attachments/weather-icons/32-s.png" },
                    new tblWeatherIconTranslation { ID = 69, WeatherID = 29, LanguageCode = "UR", Text = "صاف", Image = "/attachments/weather-icons/33-s.png" },
                    new tblWeatherIconTranslation { ID = 70, WeatherID = 30, LanguageCode = "UR", Text = "زیادہ تر صاف", Image = "/attachments/weather-icons/34-s.png" },
                    new tblWeatherIconTranslation { ID = 71, WeatherID = 31, LanguageCode = "UR", Text = "جزوی طور پر ابر آلود", Image = "/attachments/weather-icons/35-s.png" },
                    new tblWeatherIconTranslation { ID = 72, WeatherID = 32, LanguageCode = "UR", Text = "وقفے وقفے سے بادل", Image = "/attachments/weather-icons/36-s.png" },
                    new tblWeatherIconTranslation { ID = 73, WeatherID = 33, LanguageCode = "UR", Text = "دھندلی چاندنی", Image = "/attachments/weather-icons/37-s.png" },
                    new tblWeatherIconTranslation { ID = 74, WeatherID = 34, LanguageCode = "UR", Text = "زیادہ تر ابر آلود (رات)", Image = "/attachments/weather-icons/38-s.png" },
                    new tblWeatherIconTranslation { ID = 75, WeatherID = 35, LanguageCode = "UR", Text = "بارش کے ساتھ جزوی طور پر ابر آلود", Image = "/attachments/weather-icons/39-s.png" },
                    new tblWeatherIconTranslation { ID = 76, WeatherID = 36, LanguageCode = "UR", Text = "بارش کے ساتھ زیادہ تر ابر آلود", Image = "/attachments/weather-icons/40-s.png" },
                    new tblWeatherIconTranslation { ID = 77, WeatherID = 37, LanguageCode = "UR", Text = "ٹی طوفان کے ساتھ جزوی طور پر ابر آلود", Image = "/attachments/weather-icons/41-s.png" },
                    new tblWeatherIconTranslation { ID = 78, WeatherID = 38, LanguageCode = "UR", Text = "T-طوفانوں کے ساتھ زیادہ تر ابر آلود", Image = "/attachments/weather-icons/42-s.png" },
                    new tblWeatherIconTranslation { ID = 79, WeatherID = 39, LanguageCode = "UR", Text = "زیادہ تر بادل چھائے ہوئے ہیں", Image = "/attachments/weather-icons/43-s.png" },
                    new tblWeatherIconTranslation { ID = 80, WeatherID = 40, LanguageCode = "UR", Text = "زیادہ تر ابر آلود اور برفباری", Image = "/attachments/weather-icons/44-s.png" }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WeatherIconTranslations ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WeatherIconTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedSequenceTranslation(AmazonFarmerContext context)
        {
            //if (context.SequenceTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            //{

            //    context.SequenceTranslations.AddRange(
            //        new tblSequenceTranslation { ID = 1,  LanguageCode = "EN", Text = "First" },
            //        new tblSequenceTranslation { ID = 2, SequenceID = 2, LanguageCode = "EN", Text = "Second" },
            //        new tblSequenceTranslation { ID = 3, SequenceID = 3, LanguageCode = "EN", Text = "Third" },
            //        new tblSequenceTranslation { ID = 4, SequenceID = 4, LanguageCode = "EN", Text = "Fourth" },
            //        new tblSequenceTranslation { ID = 5, SequenceID = 5, LanguageCode = "EN", Text = "Fifth" },
            //        new tblSequenceTranslation { ID = 6, SequenceID = 6, LanguageCode = "EN", Text = "Sixth" },
            //        new tblSequenceTranslation { ID = 7, SequenceID = 7, LanguageCode = "EN", Text = "Seventh" },
            //        new tblSequenceTranslation { ID = 8, SequenceID = 8, LanguageCode = "EN", Text = "Eighth" },
            //        new tblSequenceTranslation { ID = 9, SequenceID = 9, LanguageCode = "EN", Text = "Ninth" },
            //        new tblSequenceTranslation { ID = 10, SequenceID = 10, LanguageCode = "EN", Text = "Tenth" },
            //        new tblSequenceTranslation { ID = 11, SequenceID = 11, LanguageCode = "EN", Text = "Eleventh" },
            //        new tblSequenceTranslation { ID = 12, SequenceID = 12, LanguageCode = "EN", Text = "Twelfth" },
            //        new tblSequenceTranslation { ID = 13, SequenceID = 13, LanguageCode = "EN", Text = "Thirteenth" },
            //        new tblSequenceTranslation { ID = 14, SequenceID = 14, LanguageCode = "EN", Text = "Fourteenth" },
            //        new tblSequenceTranslation { ID = 15, SequenceID = 15, LanguageCode = "EN", Text = "Fifteenth" },
            //        new tblSequenceTranslation { ID = 16, SequenceID = 16, LanguageCode = "EN", Text = "Sixteenth" },
            //        new tblSequenceTranslation { ID = 17, SequenceID = 17, LanguageCode = "EN", Text = "Seventeenth" },
            //        new tblSequenceTranslation { ID = 18, SequenceID = 18, LanguageCode = "EN", Text = "Eighteenth" },
            //        new tblSequenceTranslation { ID = 19, SequenceID = 19, LanguageCode = "EN", Text = "Nineteenth" },
            //        new tblSequenceTranslation { ID = 20, SequenceID = 20, LanguageCode = "EN", Text = "Twentieth" },
            //        new tblSequenceTranslation { ID = 21,  LanguageCode = "UR", Text = "پہلا" },
            //        new tblSequenceTranslation { ID = 22, SequenceID = 2, LanguageCode = "UR", Text = "دوسرا" },
            //        new tblSequenceTranslation { ID = 23, SequenceID = 3, LanguageCode = "UR", Text = "تیسرے" },
            //        new tblSequenceTranslation { ID = 24, SequenceID = 4, LanguageCode = "UR", Text = "چوتھا" },
            //        new tblSequenceTranslation { ID = 25, SequenceID = 5, LanguageCode = "UR", Text = "پانچواں" },
            //        new tblSequenceTranslation { ID = 26, SequenceID = 6, LanguageCode = "UR", Text = "چھٹا" },
            //        new tblSequenceTranslation { ID = 27, SequenceID = 7, LanguageCode = "UR", Text = "ساتواں" },
            //        new tblSequenceTranslation { ID = 28, SequenceID = 8, LanguageCode = "UR", Text = "آٹھواں" },
            //        new tblSequenceTranslation { ID = 29, SequenceID = 9, LanguageCode = "UR", Text = "نویں" },
            //        new tblSequenceTranslation { ID = 30, SequenceID = 10, LanguageCode = "UR", Text = "دسویں" },
            //        new tblSequenceTranslation { ID = 31, SequenceID = 11, LanguageCode = "UR", Text = "گیارہویں" },
            //        new tblSequenceTranslation { ID = 32, SequenceID = 12, LanguageCode = "UR", Text = "بارہویں" },
            //        new tblSequenceTranslation { ID = 33, SequenceID = 13, LanguageCode = "UR", Text = "تیرہویں" },
            //        new tblSequenceTranslation { ID = 34, SequenceID = 14, LanguageCode = "UR", Text = "چودہویں" },
            //        new tblSequenceTranslation { ID = 35, SequenceID = 15, LanguageCode = "UR", Text = "پندرہویں" },
            //        new tblSequenceTranslation { ID = 36, SequenceID = 16, LanguageCode = "UR", Text = "سولہویں" },
            //        new tblSequenceTranslation { ID = 37, SequenceID = 17, LanguageCode = "UR", Text = "سترہویں" },
            //        new tblSequenceTranslation { ID = 38, SequenceID = 18, LanguageCode = "UR", Text = "اثناعشرہ" },
            //        new tblSequenceTranslation { ID = 39, SequenceID = 19, LanguageCode = "UR", Text = "تیسرہویں" },
            //        new tblSequenceTranslation { ID = 40, SequenceID = 20, LanguageCode = "UR", Text = "چالیسویں" }
            //    );


            //    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT SequenceTranslations ON");

            //    context.SaveChanges();

            //    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT SequenceTranslations OFF"); transaction.Commit();
            //}
        }
        private static void SeedAuthorityLetter(AmazonFarmerContext context)
        {
            if (context.AuthorityLetters.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.AuthorityLetters.AddRange(
                    new TblAuthorityLetters
                    {
                        AuthorityLetterID = 1,
                        AuthorityLetterNo = "00001",
                        SAPFarmerCode = "00001",
                        OrderID = 1,
                        BearerName = "Syed Talha Salman",
                        BearerNIC = "42401-6487231-1",
                        IsOCRAutomated = false,
                        FieldWHIncharge = "Field Warehouse Incharge",
                        Dated = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture),
                        Status = false,
                        Active = EAuthorityLetterStatus.Active,
                        WareHouseID = 1,
                        CreatedByID = "3ad7592f-12ae-4eca-a96d-278e1155135c"
                    },
                    new TblAuthorityLetters
                    {
                        AuthorityLetterID = 2,
                        AuthorityLetterNo = "00002",
                        SAPFarmerCode = "00001",
                        OrderID = 2,
                        BearerName = "Bakar",
                        BearerNIC = "00000-0000000-0",
                        IsOCRAutomated = false,
                        FieldWHIncharge = "Field Warehouse Incharge 2",
                        Dated = DateTime.ParseExact("22/04/26", "yy/MM/dd", CultureInfo.InvariantCulture),
                        Status = true,
                        Active = EAuthorityLetterStatus.Active,
                        WareHouseID = 1,
                        CreatedByID = "3ad7592f-12ae-4eca-a96d-278e1155135c"
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AuthorityLetters ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AuthorityLetters OFF"); transaction.Commit();
            }
        }
        private static void SeedAuthorityLetterDetails(AmazonFarmerContext context)
        {
            try
            {
                if (context.AuthorityLetterDetails.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
                {

                    context.AuthorityLetterDetails.AddRange(
                        new TblAuthorityLetterDetails
                        {
                            RecID = 1,
                            AuthorityLetterID = 1,
                            ProductID = 1,
                            TruckerNo = "KKR-0181",
                            BagQuantity = 200,
                        },
                        new TblAuthorityLetterDetails
                        {
                            RecID = 2,
                            AuthorityLetterID = 2,
                            ProductID = 1,
                            TruckerNo = "KKR-0181",
                            BagQuantity = 100,
                        }
                    );


                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AuthorityLetterDetails ON");

                    context.SaveChanges();

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AuthorityLetterDetails OFF"); transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void SeedReasons(AmazonFarmerContext context)
        {
            if (context.Reasons.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {

                context.Reasons.AddRange(
                    new tblReasons
                    {
                        ID = 1,
                        Reason = "acha nhi laga, reject krdo",
                        Status = EActivityStatus.Active,
                        ReasonForID = EReasonFor.Reject,
                        ReasonOfID = EReasonOf.Farm
                    },
                    new tblReasons
                    {
                        ID = 2,
                        Reason = "jao sahi kr k ao",
                        Status = EActivityStatus.Active,
                        ReasonForID = EReasonFor.Revert,
                        ReasonOfID = EReasonOf.Farm
                    },
                    new tblReasons
                    {
                        ID = 3,
                        Reason = "direct complete krdo",
                        Status = EActivityStatus.Active,
                        ReasonForID = EReasonFor.Active,
                        ReasonOfID = EReasonOf.Farm
                    },
                    new tblReasons
                    {
                        ID = 4,
                        Reason = "agla laga or approve hoja",
                        Status = EActivityStatus.Active,
                        ReasonForID = EReasonFor.Approve,
                        ReasonOfID = EReasonOf.Farm
                    }
                    //new tblReasons
                    //{
                    //    ID = 5,
                    //    Reason = "Kuch fark nhi prta",
                    //    Status = EActivityStatus.Active,
                    //    ReasonForID = EReasonFor.NoImpactAdvance,
                    //    ReasonOfID = EReasonOf.ChangedPlan_Employee
                    //},
                    //new tblReasons
                    //{
                    //    ID = 6,
                    //    Reason = "Paisa kha jaengy",
                    //    Status = EActivityStatus.Active,
                    //    ReasonForID = EReasonFor.ForfeitAdvance,
                    //    ReasonOfID = EReasonOf.ChangedPlan_Employee
                    //},
                    //new tblReasons
                    //{
                    //    ID = 7,
                    //    Reason = "Paisa wapas kardengy",
                    //    Status = EActivityStatus.Active,
                    //    ReasonForID = EReasonFor.RefundAdvance,
                    //    ReasonOfID = EReasonOf.ChangedPlan_Employee
                    //}
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reasons ON");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reasons OFF"); transaction.Commit();
            }
        }
        private static void SeedReasonTranslations(AmazonFarmerContext context)
        {
            if (context.ReasonTranslations.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {
                context.ReasonTranslations.AddRange(
                    new tblReasonTranslation
                    {
                        ID = 1,
                        ReasonID = 1,
                        LanguageCode = "EN",
                        Text = "acha nhi laga, reject krdo"
                    },
                    new tblReasonTranslation
                    {
                        ID = 2,
                        ReasonID = 2,
                        LanguageCode = "EN",
                        Text = "jao sahi kr k ao"
                    },
                    new tblReasonTranslation
                    {
                        ID = 3,
                        ReasonID = 3,
                        LanguageCode = "EN",
                        Text = "direct complete krdo"
                    },
                    new tblReasonTranslation
                    {
                        ID = 4,
                        ReasonID = 4,
                        LanguageCode = "EN",
                        Text = "agla laga or approve hoja"
                    },
                    new tblReasonTranslation
                    {
                        ID = 5,
                        ReasonID = 1,
                        LanguageCode = "UR",
                        Text = "URDU - acha nhi laga, reject krdo"
                    },
                    new tblReasonTranslation
                    {
                        ID = 6,
                        ReasonID = 2,
                        LanguageCode = "UR",
                        Text = "URDU - jao sahi kr k ao"
                    },
                    new tblReasonTranslation
                    {
                        ID = 7,
                        ReasonID = 3,
                        LanguageCode = "UR",
                        Text = "URDU - direct complete krdo"
                    },
                    new tblReasonTranslation
                    {
                        ID = 8,
                        ReasonID = 4,
                        LanguageCode = "UR",
                        Text = "URDU - agla laga or approve hoja"
                    }
                    //new tblReasonTranslation
                    //{
                    //    ID = 9,
                    //    ReasonID = 6,
                    //    LanguageCode = "EN",
                    //    Text = "English: Kuch fark nhi prta"
                    //},
                    //new tblReasonTranslation
                    //{
                    //    ID = 10,
                    //    ReasonID = 6,
                    //    LanguageCode = "UR",
                    //    Text = "Urdu: Kuch fark nhi prta"
                    //},
                    //new tblReasonTranslation
                    //{
                    //    ID = 11,
                    //    ReasonID = 7,
                    //    LanguageCode = "EN",
                    //    Text = "English: Paisa kha jaengy"
                    //},
                    //new tblReasonTranslation
                    //{
                    //    ID = 12,
                    //    ReasonID = 7,
                    //    LanguageCode = "UR",
                    //    Text = "Urdu: Paisa kha jaengy"
                    //},
                    //new tblReasonTranslation
                    //{
                    //    ID = 13,
                    //    ReasonID = 8,
                    //    LanguageCode = "EN",
                    //    Text = "English: Paisa wapas kardengy"
                    //},
                    //new tblReasonTranslation
                    //{
                    //    ID = 14,
                    //    ReasonID = 8,
                    //    LanguageCode = "UR",
                    //    Text = "Urdu: Paisa wapas kardengy"
                    //}
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ReasonTranslations ON");
                try
                {

                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ReasonTranslations OFF"); transaction.Commit();
            }
        }
        private static void SeedFarmerCNIC(AmazonFarmerContext context)
        {
            #region seed attachment
            if (context.Attachment.Count() > 0) return; using (var transaction = context.Database.BeginTransaction())
            {
                context.Attachment.AddRange(
                    new tblAttachment
                    {
                        ID = 2,
                        tblAttachmentTypeID = (int)EAttachmentType.User_CNIC_Document,
                        Guid = new Guid("95743AE4-F60B-4280-BB1F-3611DC2EA1A1"),
                        SubmittedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture),
                        Name = "Seeded-file-1.jpg",
                        Path = @"\private-documents\NTN\08052024_0629851000000071.jpg",
                        FileType = ".jpg"
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Attachment ON");
                try
                {

                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Attachment OFF"); transaction.Commit();
            }
            #endregion
            #region relation with user attachment
            if (context.UserAttachments.Count() > 1) return; using (var transaction = context.Database.BeginTransaction())
            {
                context.UserAttachments.AddRange(
                    new tblUserAttachments
                    {
                        ID = 2,
                        UserID = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                        tblAttachmentID = 2,
                        Status = EActivityStatus.Active
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserAttachments ON");
                try
                {

                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserAttachments OFF"); transaction.Commit();
            }
            #endregion
        }
        private static void SeedFarmerNTN(AmazonFarmerContext context)
        {
            #region seed attachment
            if (context.Attachment.Count() > 1) return; using (var transaction = context.Database.BeginTransaction())
            {
                context.Attachment.AddRange(
                    new tblAttachment
                    {
                        ID = 1,
                        tblAttachmentTypeID = (int)EAttachmentType.User_NTN_Document,
                        Guid = new Guid("95743AE4-F60B-4280-BB1F-3611DC2EA1A1"),
                        SubmittedDate = DateTime.ParseExact("22/04/25", "yy/MM/dd", CultureInfo.InvariantCulture),
                        Name = "Seeded-file-1.jpg",
                        Path = @"\private-documents\NTN\08052024_0629851000000071.jpg",
                        FileType = ".jpg"
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Attachment ON");
                try
                {

                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Attachment OFF"); transaction.Commit();
            }
            #endregion
            #region relation with user attachment
            if (context.UserAttachments.Count() > 1) return; using (var transaction = context.Database.BeginTransaction())
            {
                context.UserAttachments.AddRange(
                    new tblUserAttachments
                    {
                        ID = 1,
                        UserID= "3ad7592f-12ae-4eca-a96d-278e1155135c",
                        tblAttachmentID=2,
                        Status = EActivityStatus.Active
                    }
                );


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserAttachments ON");
                try
                {

                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserAttachments OFF"); transaction.Commit();
            }
            #endregion
        }
    }
}
