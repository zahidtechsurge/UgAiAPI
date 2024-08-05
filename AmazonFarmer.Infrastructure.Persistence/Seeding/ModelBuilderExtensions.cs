using AmazonFarmer.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedModules(modelBuilder);
            SeedPages(modelBuilder);
            SeedClaimHeaders(modelBuilder);
            SeedLangauges(modelBuilder);
            SeedAttachmentTypes(modelBuilder);
            SeedWarehouse(modelBuilder);
            SeedWarehouseLanguages(modelBuilder);
            SeedDistrict(modelBuilder);
            SeedDistrictLanguages(modelBuilder);
            SeedCities(modelBuilder);
            SeedCityLangugaes(modelBuilder);
            SeedBanner(modelBuilder);
            SeedBannerLanguages(modelBuilder);
            SeedTehsil(modelBuilder);
            SeedTehsilLanguages(modelBuilder);
            SeedIntro(modelBuilder);
            SeedIntroLanguges(modelBuilder);
            SeedSeasons(modelBuilder);
            SeedSeasonTranslation(modelBuilder);
            SeedCrops(modelBuilder);
            SeedCropTranslation(modelBuilder);
            SeedCropTimings(modelBuilder);
            SeedCategory(modelBuilder);
            SeedCategoryTranslation(modelBuilder);
            SeedProduct(modelBuilder);
            SeedProductTranslation(modelBuilder);
            SeedServices(modelBuilder);
            SeedServiceTranslation(modelBuilder);
            SeedProductConsumptionMatrix(modelBuilder);
            //SeedProductClaimsPermissions(modelBuilder);
        }
        private static void SeedModules(ModelBuilder modelBuilder)
        {
            //Seesing Languages
            modelBuilder.Entity<TblNavigationModule>().HasData(
                new TblNavigationModule { ModuleId = 1, IsActive = EActivityStatus.Active, ModuleName = "Dashboard", ShowInMenu = true, ModuleOrder = 1 },
                new TblNavigationModule { ModuleId = 2, IsActive = EActivityStatus.Active, ModuleName = "Farmer Management", ShowInMenu = true, ModuleOrder = 2 },
                new TblNavigationModule { ModuleId = 3, IsActive = EActivityStatus.Active, ModuleName = "Role Management", ShowInMenu = true, ModuleOrder = 2 }
           );
        }
        private static void SeedPages(ModelBuilder modelBuilder)
        {
            //Seesing Permissions for different Modules
            modelBuilder.Entity<TblPage>().HasData(
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
        }
        private static void SeedClaimHeaders(ModelBuilder modelBuilder)
        {
            //Seesing Permission groups of claim for product controller/page
            modelBuilder.Entity<TblClaim>().HasData(
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
        }
        private static void SeedProductClaimsPermissions(ModelBuilder modelBuilder)
        {
            //Seesing Permission groups of claim for product controller/page
            modelBuilder.Entity<TblClaimAction>().HasData(
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
        }
        private static void SeedLangauges(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblLanguages>().HasData(
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
        }
        private static void SeedAttachmentTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblAttachmentTypes>().HasData(
                new tblAttachmentTypes { ID = 1, TypeName = "User Attachment" },
                new tblAttachmentTypes { ID = 2, TypeName = "Farm Attachment" }
            );
        }
        private static void SeedCities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCity>().HasData(
                new tblCity { ID = 1, DistrictID = 1, Name = "Karachi", Status = EActivityStatus.Active },
                new tblCity { ID = 2, DistrictID = 1, Name = "Hyderabad", Status = EActivityStatus.Active },
                new tblCity { ID = 3, DistrictID = 1, Name = "Sukkur", Status = EActivityStatus.Active },
                new tblCity { ID = 4, DistrictID = 1, Name = "Larkana", Status = EActivityStatus.Active },
                new tblCity { ID = 5, DistrictID = 1, Name = "Nawabshah", Status = EActivityStatus.Active },
                new tblCity { ID = 6, DistrictID = 1, Name = "Mirpur Khas", Status = EActivityStatus.Active },
                new tblCity { ID = 7, DistrictID = 2, Name = "Lahore", Status = EActivityStatus.Active },
                new tblCity { ID = 8, DistrictID = 2, Name = "Faisalabad", Status = EActivityStatus.Active },
                new tblCity { ID = 9, DistrictID = 2, Name = "Rawalpindi", Status = EActivityStatus.Active },
                new tblCity { ID = 10, DistrictID = 2, Name = "Multan", Status = EActivityStatus.Active },
                new tblCity { ID = 11, DistrictID = 2, Name = "Gujranwala", Status = EActivityStatus.Active },
                new tblCity { ID = 12, DistrictID = 2, Name = "Bahawalpur", Status = EActivityStatus.Active }
            );
        }
        private static void SeedCityLangugaes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCityLanguages>().HasData(
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
        }
        private static void SeedDistrict(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblDistrict>().HasData(
                new tblDistrict { ID = 1, Name = "Sindh", Status = EActivityStatus.Active },
                new tblDistrict { ID = 2, Name = "Punjab", Status = EActivityStatus.Active },
                new tblDistrict { ID = 3, Name = "Khyber Pakhtunkhwa (KP)", Status = EActivityStatus.Active },
                new tblDistrict { ID = 4, Name = "Balochistan", Status = EActivityStatus.Active },
                new tblDistrict { ID = 5, Name = "Gilgit-Baltistan (GB)", Status = EActivityStatus.Active },
                new tblDistrict { ID = 6, Name = "Azad Jammu and Kashmir (AJK)", Status = EActivityStatus.Active }
            );
        }
        private static void SeedDistrictLanguages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblDistrictLanguages>().HasData(
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
        }
        private static void SeedTehsil(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblTehsil>().HasData(
                new tblTehsil { ID = 1, CityID = 1, Name = "Karachi Central", Status = EActivityStatus.Active },
                new tblTehsil { ID = 2, CityID = 1, Name = "Karachi East", Status = EActivityStatus.Active },
                new tblTehsil { ID = 3, CityID = 1, Name = "Karachi West", Status = EActivityStatus.Active },
                new tblTehsil { ID = 4, CityID = 1, Name = "Karachi South", Status = EActivityStatus.Active },
                new tblTehsil { ID = 5, CityID = 1, Name = "Malir", Status = EActivityStatus.Active },
                new tblTehsil { ID = 6, CityID = 1, Name = "Korangi", Status = EActivityStatus.Active },
                new tblTehsil { ID = 7, CityID = 2, Name = "Hyderabad", Status = EActivityStatus.Active }
            );
        }
        private static void SeedTehsilLanguages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblTehsilLanguages>().HasData(
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
        }
        private static void SeedWarehouse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblwarehouse>().HasData(
                new tblwarehouse { ID = 1, Name = "WH-Eng-1", Status = EActivityStatus.Active },
                new tblwarehouse { ID = 2, Name = "WH-Eng-2", Status = EActivityStatus.Active }
            );
        }
        private static void SeedWarehouseLanguages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblwarehouseTranslation>().HasData(
                new tblwarehouseTranslation { ID = 1, WarehouseID = 1, LanguageCode = "EN", Text = "WH-Eng-1" },
                new tblwarehouseTranslation { ID = 2, WarehouseID = 2, LanguageCode = "EN", Text = "WH-Eng-2" },
                new tblwarehouseTranslation { ID = 3, WarehouseID = 1, LanguageCode = "UR", Text = "WH-Ur-1" },
                new tblwarehouseTranslation { ID = 4, WarehouseID = 2, LanguageCode = "UR", Text = "WH-Ur-2" }
            );
        }
        private static void SeedBanner(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblBanner>().HasData(
                new tblBanner { ID = 1, Name = "Banner 1", Status = EActivityStatus.Active }
            );
        }
        private static void SeedBannerLanguages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblBannerLanguages>().HasData(
                new tblBannerLanguages { ID = 1, BannerID = 1, LanguageCode = "EN", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU" },
                new tblBannerLanguages { ID = 2, BannerID = 1, LanguageCode = "UR", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU" }
            );
        }
        private static void SeedIntro(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblIntro>().HasData(
                new tblIntro { ID = 1, Name = "First", Status = EActivityStatus.Active },
                new tblIntro { ID = 2, Name = "Second", Status = EActivityStatus.Active }
            );
        }
        private static void SeedIntroLanguges(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblIntroLanguages>().HasData(
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
        }
        private static void SeedCrops(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCrop>().HasData(
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
        }
        private static void SeedCropTranslation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCropTranslation>().HasData(
                new tblCropTranslation { ID = 1, CropID = 1, LanguageCode = "EN", Text = "Wheat", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 2, CropID = 2, LanguageCode = "EN", Text = "Sunflower", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 3, CropID = 3, LanguageCode = "EN", Text = "Cotton", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 4, CropID = 4, LanguageCode = "EN", Text = "Sugarcane", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 5, CropID = 5, LanguageCode = "EN", Text = "Onion", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 6, CropID = 6, LanguageCode = "EN", Text = "Tomato", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 7, CropID = 7, LanguageCode = "EN", Text = "Potato", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 8, CropID = 8, LanguageCode = "EN", Text = "Rice", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 9, CropID = 9, LanguageCode = "EN", Text = "Chillies", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 10, CropID = 10, LanguageCode = "EN", Text = "Soyabean", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 11, CropID = 11, LanguageCode = "EN", Text = "Bajra", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 12, CropID = 12, LanguageCode = "EN", Text = "Jowar", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 13, CropID = 13, LanguageCode = "EN", Text = "Garlic", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 14, CropID = 14, LanguageCode = "EN", Text = "Coriander", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 15, CropID = 15, LanguageCode = "EN", Text = "Barley", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 16, CropID = 16, LanguageCode = "EN", Text = "Maize", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 17, CropID = 1, LanguageCode = "UR", Text = "گندم", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 18, CropID = 2, LanguageCode = "UR", Text = "سورج مکھی", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 19, CropID = 3, LanguageCode = "UR", Text = "کپاس", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 20, CropID = 4, LanguageCode = "UR", Text = "گنا", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 21, CropID = 5, LanguageCode = "UR", Text = "پیاز", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 22, CropID = 6, LanguageCode = "UR", Text = "ٹماٹر", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 23, CropID = 7, LanguageCode = "UR", Text = "آلو", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 24, CropID = 8, LanguageCode = "UR", Text = "چاول", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 25, CropID = 9, LanguageCode = "UR", Text = "مرچیں", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 26, CropID = 10, LanguageCode = "UR", Text = "سویا بین", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 27, CropID = 11, LanguageCode = "UR", Text = "باجرہ", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 28, CropID = 12, LanguageCode = "UR", Text = "جوار", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 29, CropID = 13, LanguageCode = "UR", Text = "Garlic", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 30, CropID = 14, LanguageCode = "UR", Text = "Coriander", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 31, CropID = 15, LanguageCode = "UR", Text = "Barley", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblCropTranslation { ID = 32, CropID = 16, LanguageCode = "UR", Text = "Maize", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" }
            );
        }
        private static void SeedSeasons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblSeason>().HasData(
                new tblSeason { ID = 1, Name = "Rabbi", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = EActivityStatus.Active },
                new tblSeason { ID = 2, Name = "Kharif", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = EActivityStatus.Active }
            );
        }
        private static void SeedSeasonTranslation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblSeasonTranslation>().HasData(
                new tblSeasonTranslation { ID = 1, SeasonID = 1, LanguageCode = "EN", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Translation = "Rabbi" },
                new tblSeasonTranslation { ID = 2, SeasonID = 2, LanguageCode = "EN", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Translation = "Kharif" },
                new tblSeasonTranslation { ID = 3, SeasonID = 1, LanguageCode = "UR", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Translation = "ربی" },
                new tblSeasonTranslation { ID = 4, SeasonID = 2, LanguageCode = "UR", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Translation = "خزاں" }
            );
        }
        private static void SeedCropTimings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCropTimings>().HasData(
                new tblCropTimings { ID = 1, CropID = 1, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 2, CropID = 2, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 3, CropID = 3, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 4, CropID = 4, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 5, CropID = 5, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 6, CropID = 6, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 7, CropID = 7, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 8, CropID = 8, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 9, CropID = 9, DistrictID = 1, SeasonID = 1, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 10, CropID = 10, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 11, CropID = 11, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 12, CropID = 12, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 13, CropID = 13, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 14, CropID = 14, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 15, CropID = 15, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) },
                new tblCropTimings { ID = 16, CropID = 16, DistrictID = 1, SeasonID = 2, FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) }
            );
        }
        private static void SeedCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblProductCategory>().HasData(
                new tblProductCategory { ID = 1, Name = "UREA", Status = EActivityStatus.Active },
                new tblProductCategory { ID = 2, Name = "DAP", Status = EActivityStatus.Active },
                new tblProductCategory { ID = 3, Name = "Micronutrient", Status = EActivityStatus.Active },
                new tblProductCategory { ID = 4, Name = "Potash", Status = EActivityStatus.Active }
            );
        }
        private static void SeedCategoryTranslation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblProductCategoryTranslation>().HasData(
                new tblProductCategoryTranslation { ID = 1, ProductCategoryID = 1, LanguageCode = "EN", Image = "", Text = "UREA" },
                new tblProductCategoryTranslation { ID = 2, ProductCategoryID = 1, LanguageCode = "UR", Image = "", Text = "یوریا" },
                new tblProductCategoryTranslation { ID = 3, ProductCategoryID = 2, LanguageCode = "EN", Image = "", Text = "DAP" },
                new tblProductCategoryTranslation { ID = 4, ProductCategoryID = 2, LanguageCode = "UR", Image = "", Text = "ڈی اے پی" },
                new tblProductCategoryTranslation { ID = 5, ProductCategoryID = 3, LanguageCode = "EN", Image = "", Text = "Micronutrient" },
                new tblProductCategoryTranslation { ID = 6, ProductCategoryID = 3, LanguageCode = "UR", Image = "", Text = "مائیکرو نیوٹرینٹ" },
                new tblProductCategoryTranslation { ID = 7, ProductCategoryID = 4, LanguageCode = "EN", Image = "", Text = "Potash" },
                new tblProductCategoryTranslation { ID = 8, ProductCategoryID = 4, LanguageCode = "UR", Image = "", Text = "پوٹاش" }
            );
        }
        private static void SeedProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblProduct>().HasData(
                new TblProduct { ID = 1, CategoryID = 1/*, CreatedByID = "a35b9178-2859-4826-9583-430c71887b341"*/, Name = "UREA", ProductCode = "00001", CreatedDate = DateTime.Now, Active = EActivityStatus.Active },
                new TblProduct { ID = 2, CategoryID = 1/*, CreatedByID = "a35b9178-2859-4826-9583-430c71887b341"*/, Name = "Ammonium Sulphate", ProductCode = "00002", CreatedDate = DateTime.Now, Active = EActivityStatus.Active },
                new TblProduct { ID = 3, CategoryID = 2/*, CreatedByID = "a35b9178-2859-4826-9583-430c71887b341"*/, Name = "Zabardast Urea", ProductCode = "00003", CreatedDate = DateTime.Now, Active = EActivityStatus.Active }
            );
        }
        private static void SeedProductTranslation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblProductTranslation>().HasData(
                new tblProductTranslation { ID = 1, ProductID = 1, LanguageCode = "EN", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "UREA" },
                new tblProductTranslation { ID = 2, ProductID = 1, LanguageCode = "UR", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "یوریا" },
                new tblProductTranslation { ID = 3, ProductID = 2, LanguageCode = "EN", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "Ammonium Sulphate" },
                new tblProductTranslation { ID = 4, ProductID = 2, LanguageCode = "UR", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "امونیم سلفیٹ" },
                new tblProductTranslation { ID = 5, ProductID = 3, LanguageCode = "EN", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "Zabardast Urea" },
                new tblProductTranslation { ID = 6, ProductID = 3, LanguageCode = "UR", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", Text = "بھاری یوریا" }
            );
        }
        private static void SeedServices(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblService>().HasData(
                new tblService { ID = 1, Name = "Soil Sampling", Active = EActivityStatus.Active },
                new tblService { ID = 2, Name = "Geofencing", Active = EActivityStatus.Active },
                new tblService { ID = 3, Name = "Drone Footage", Active = EActivityStatus.Active }
            );
        }
        private static void SeedServiceTranslation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblServiceTranslation>().HasData(
                new tblServiceTranslation { ID = 1, ServiceID = 1, LanguageCode = "EN", Text = "Soil Sampling", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblServiceTranslation { ID = 2, ServiceID = 1, LanguageCode = "UR", Text = "مٹی کے نمونے لینے", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblServiceTranslation { ID = 3, ServiceID = 2, LanguageCode = "EN", Text = "Geofencing", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblServiceTranslation { ID = 4, ServiceID = 2, LanguageCode = "UR", Text = "جیوفینسنگ", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblServiceTranslation { ID = 5, ServiceID = 3, LanguageCode = "EN", Text = "Drone Footage", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" },
                new tblServiceTranslation { ID = 6, ServiceID = 3, LanguageCode = "UR", Text = "ڈرون فوٹیج", Image = "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png" }
            );
        }
        private static void SeedProductConsumptionMatrix(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblProductConsumptionMetrics>().HasData(
                new tblProductConsumptionMetrics { ID = 1, CropID = 1, ProductID = 1, TerritoryID = 1, UOM = "Bags", Usage = Convert.ToDecimal(1.2) },
                new tblProductConsumptionMetrics { ID = 2, CropID = 1, ProductID = 2, TerritoryID = 1, UOM = "Bags", Usage = Convert.ToDecimal(2) }
            );
        }





    }
}
