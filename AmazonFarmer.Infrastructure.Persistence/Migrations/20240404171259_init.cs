using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FarmApplication",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmApplication", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HomeSliders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSliders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Intros",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intros", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblFarmers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isOTPApproved = table.Column<bool>(type: "bit", nullable: true),
                    LastPasswordChange = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignupAgreementDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFarmers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblNavigationModule",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    ModuleOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblNavigationModule", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "tblRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tblAttachmentTypeID = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Attachment_AttachmentTypes_tblAttachmentTypeID",
                        column: x => x.tblAttachmentTypeID,
                        principalTable: "AttachmentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannerLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BannerLanguages_Banners_BannerID",
                        column: x => x.BannerID,
                        principalTable: "Banners",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropTranslation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTranslation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CropTranslation_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropTranslation_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistrictLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DistrictLanguages_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistrictLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeSliderLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeSliderID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSliderLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HomeSliderLanguages_HomeSliders_HomeSliderID",
                        column: x => x.HomeSliderID,
                        principalTable: "HomeSliders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeSliderLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntroLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IntroLanguages_Intros_IntroID",
                        column: x => x.IntroID,
                        principalTable: "Intros",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntroLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MonthTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthTranslations_Months_MonthID",
                        column: x => x.MonthID,
                        principalTable: "Months",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryTranslation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryTranslation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductCategoryTranslation_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryTranslation_ProductCategory_ProductCategoryID",
                        column: x => x.ProductCategoryID,
                        principalTable: "ProductCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropTimings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    SeasonID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTimings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CropTimings_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropTimings_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropTimings_Season_SeasonID",
                        column: x => x.SeasonID,
                        principalTable: "Season",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SeasonTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTranslations_Season_SeasonID",
                        column: x => x.SeasonID,
                        principalTable: "Season",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "ProductCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Service_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFarmerClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFarmerClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFarmerClaim_tblFarmers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFarmerLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFarmerLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_tblFarmerLogin_tblFarmers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFarmerToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFarmerToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_tblFarmerToken_tblFarmers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPage",
                columns: table => new
                {
                    PageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleID = table.Column<int>(type: "int", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    PageIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowOnMenu = table.Column<bool>(type: "bit", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectModule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPage", x => x.PageID);
                    table.ForeignKey(
                        name: "FK_tblPage_tblNavigationModule_ModuleID",
                        column: x => x.ModuleID,
                        principalTable: "tblNavigationModule",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFarmerRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFarmerRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_tblFarmerRole_tblFarmers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblFarmerRole_tblRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTranslation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTranslation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WarehouseTranslation_Warehouse_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAttachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tblAttachmentID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserAttachments_Attachment_tblAttachmentID",
                        column: x => x.tblAttachmentID,
                        principalTable: "Attachment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAttachments_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityLanguages_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tehsils",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tehsils", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tehsils_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorityLetters",
                columns: table => new
                {
                    AuthorityLetterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorityLetterNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    BearerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BearerNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOCRAutomated = table.Column<bool>(type: "bit", nullable: false),
                    INVNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldWHIncharge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorityLetters", x => x.AuthorityLetterID);
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    RecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.RecID);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTranslation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTranslation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServiceTranslation_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTranslation_Service_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Service",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblClaim",
                columns: table => new
                {
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClaim", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_tblClaim_tblPage_PageId",
                        column: x => x.PageId,
                        principalTable: "tblPage",
                        principalColumn: "PageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmerProfile",
                columns: table => new
                {
                    ProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNICNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTNNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnedLand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeasedLand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalArea = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    SAPFarmerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedByID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelectedLangCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tblTehsilID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerProfile", x => x.ProfileID);
                    table.ForeignKey(
                        name: "FK_FarmerProfile_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmerProfile_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmerProfile_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmerProfile_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    FarmID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FarmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationID = table.Column<int>(type: "int", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    TehsilID = table.Column<int>(type: "int", nullable: false),
                    Acreage = table.Column<double>(type: "float", nullable: false),
                    isLeased = table.Column<bool>(type: "bit", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    isApproved = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledgedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SAPFarmCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tblDistrictID = table.Column<int>(type: "int", nullable: true),
                    tblTehsilID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.FarmID);
                    table.ForeignKey(
                        name: "FK_Farms_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Farms_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Farms_District_tblDistrictID",
                        column: x => x.tblDistrictID,
                        principalTable: "District",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Farms_FarmApplication_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "FarmApplication",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Farms_tblFarmers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Farms_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Farms_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Farms_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileChangeRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNICNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTNNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnedLand = table.Column<double>(type: "float", nullable: false),
                    LeasedLand = table.Column<double>(type: "float", nullable: false),
                    TotalArea = table.Column<double>(type: "float", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    TehsilID = table.Column<int>(type: "int", nullable: false),
                    SAPFarmerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    tblTehsilID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileChangeRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_tblFarmers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TehsilLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TehsilID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TehsilLanguages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TehsilLanguages_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TehsilLanguages_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorityLetterDetails",
                columns: table => new
                {
                    RecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorityLetterID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    WareHouseID = table.Column<int>(type: "int", nullable: false),
                    TruckerNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BiltyNo = table.Column<int>(type: "int", nullable: false),
                    BagQuantity = table.Column<int>(type: "int", nullable: false),
                    TblProductID = table.Column<int>(type: "int", nullable: true),
                    tblServiceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorityLetterDetails", x => x.RecID);
                    table.ForeignKey(
                        name: "FK_AuthorityLetterDetails_AuthorityLetters_AuthorityLetterID",
                        column: x => x.AuthorityLetterID,
                        principalTable: "AuthorityLetters",
                        principalColumn: "AuthorityLetterID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorityLetterDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorityLetterDetails_Products_TblProductID",
                        column: x => x.TblProductID,
                        principalTable: "Products",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuthorityLetterDetails_Service_tblServiceID",
                        column: x => x.tblServiceID,
                        principalTable: "Service",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tblClaimAction",
                columns: table => new
                {
                    ClaimActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClaimAction", x => x.ClaimActionId);
                    table.ForeignKey(
                        name: "FK_tblClaimAction_tblClaim_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "tblClaim",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRoleClaim_tblClaim_ClaimValue",
                        column: x => x.ClaimValue,
                        principalTable: "tblClaim",
                        principalColumn: "ClaimId");
                    table.ForeignKey(
                        name: "FK_tblRoleClaim_tblRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmAttachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmID = table.Column<int>(type: "int", nullable: false),
                    AttachmentID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmAttachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FarmAttachments_Attachment_AttachmentID",
                        column: x => x.AttachmentID,
                        principalTable: "Attachment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmAttachments_Farms_FarmID",
                        column: x => x.FarmID,
                        principalTable: "Farms",
                        principalColumn: "FarmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmChangeRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FarmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    TehsilID = table.Column<int>(type: "int", nullable: false),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    Acreage = table.Column<double>(type: "float", nullable: false),
                    isLeased = table.Column<bool>(type: "bit", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    isApproved = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledgedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SAPFarmCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tblDistrictID = table.Column<int>(type: "int", nullable: true),
                    tblTehsilID = table.Column<int>(type: "int", nullable: true),
                    tblfarmFarmID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmChangeRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_District_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_District_tblDistrictID",
                        column: x => x.tblDistrictID,
                        principalTable: "District",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_FarmApplication_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "FarmApplication",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Farms_FarmID",
                        column: x => x.FarmID,
                        principalTable: "Farms",
                        principalColumn: "FarmID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Farms_tblfarmFarmID",
                        column: x => x.tblfarmFarmID,
                        principalTable: "Farms",
                        principalColumn: "FarmID");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_tblFarmers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FarmID = table.Column<int>(type: "int", nullable: false),
                    SeasonID = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Plan_Farms_FarmID",
                        column: x => x.FarmID,
                        principalTable: "Farms",
                        principalColumn: "FarmID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_Season_SeasonID",
                        column: x => x.SeasonID,
                        principalTable: "Season",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plan_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanCrop",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    Acre = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanCrop", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanCrop_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanCrop_Plan_PlanID",
                        column: x => x.PlanID,
                        principalTable: "Plan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanProduct",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCropID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanProduct_PlanCrop_PlanCropID",
                        column: x => x.PlanCropID,
                        principalTable: "PlanCrop",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProduct_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AttachmentTypes",
                columns: new[] { "ID", "TypeName" },
                values: new object[,]
                {
                    { 1, "User Attachment" },
                    { 2, "Farm Attachment" }
                });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "ID", "Name", "Status" },
                values: new object[] { 1, "Banner 1", 1 });

            migrationBuilder.InsertData(
                table: "Crops",
                columns: new[] { "ID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Wheat", 1 },
                    { 2, "Sunflower", 1 },
                    { 3, "Cotton", 1 },
                    { 4, "Sugarcane", 1 },
                    { 5, "Onion", 1 },
                    { 6, "Tomato", 1 },
                    { 7, "Potato", 1 },
                    { 8, "Rice", 1 },
                    { 9, "Chillies", 1 },
                    { 10, "Soyabean", 1 },
                    { 11, "Bajra", 1 },
                    { 12, "Jowar", 1 },
                    { 13, "Garlic", 1 },
                    { 14, "Coriander", 1 },
                    { 15, "Barley", 1 },
                    { 16, "Maize", 1 }
                });

            migrationBuilder.InsertData(
                table: "District",
                columns: new[] { "ID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Sindh", 1 },
                    { 2, "Punjab", 1 },
                    { 3, "Khyber Pakhtunkhwa (KP)", 1 },
                    { 4, "Balochistan", 1 },
                    { 5, "Gilgit-Baltistan (GB)", 1 },
                    { 6, "Azad Jammu and Kashmir (AJK)", 1 }
                });

            migrationBuilder.InsertData(
                table: "Intros",
                columns: new[] { "ID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "First", 1 },
                    { 2, "Second", 1 }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageCode", "LanguageName", "Status" },
                values: new object[,]
                {
                    { "BH", "بروہی (Brahui)", 0 },
                    { "BL", "بلوچی (Balochi)", 0 },
                    { "EN", "English", 1 },
                    { "KS", "کٲشُر (Kashmiri)", 0 },
                    { "PA", "پنجابی (Punjabi)", 0 },
                    { "PS", "پښتو (Pashto)", 0 },
                    { "SD", "سنڌي (Sindhi)", 0 },
                    { "SK", "سرائیکی (Saraiki)", 0 },
                    { "SN", "شینا (Shina)", 0 },
                    { "UR", "اردو (Urdu)", 1 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "ID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "UREA", 1 },
                    { 2, "DAP", 1 },
                    { 3, "Micronutrient", 1 },
                    { 4, "Potash", 1 }
                });

            migrationBuilder.InsertData(
                table: "Season",
                columns: new[] { "ID", "EndDate", "Name", "StartDate", "Status" },
                values: new object[] { 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6624), "Rabbi", new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6610), 1 });

            migrationBuilder.InsertData(
                table: "Season",
                columns: new[] { "ID", "EndDate", "Name", "StartDate", "Status" },
                values: new object[] { 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6634), "Kharif", new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6633), 1 });

            migrationBuilder.InsertData(
                table: "Warehouse",
                columns: new[] { "ID", "Name", "Status", "latitude", "longitude" },
                values: new object[,]
                {
                    { 1, "WH-Eng-1", 1, 0.0, 0.0 },
                    { 2, "WH-Eng-2", 1, 0.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "tblNavigationModule",
                columns: new[] { "ModuleId", "IsActive", "ModuleName", "ModuleOrder", "ShowInMenu" },
                values: new object[,]
                {
                    { 1, 1, "Dashboard", 1, true },
                    { 2, 1, "Farmer Management", 2, true },
                    { 3, 1, "Role Management", 2, true }
                });

            migrationBuilder.InsertData(
                table: "BannerLanguages",
                columns: new[] { "ID", "BannerID", "Image", "LanguageCode" },
                values: new object[,]
                {
                    { 1, 1, "/attachments/banner-01.JPG", "EN" },
                    { 2, 1, "/attachments/banner-01.JPG", "UR" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "DistrictID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Karachi", 1 },
                    { 2, 1, "Hyderabad", 1 },
                    { 3, 1, "Sukkur", 1 },
                    { 4, 1, "Larkana", 1 },
                    { 5, 1, "Nawabshah", 1 },
                    { 6, 1, "Mirpur Khas", 1 },
                    { 7, 2, "Lahore", 1 },
                    { 8, 2, "Faisalabad", 1 },
                    { 9, 2, "Rawalpindi", 1 },
                    { 10, 2, "Multan", 1 },
                    { 11, 2, "Gujranwala", 1 },
                    { 12, 2, "Bahawalpur", 1 }
                });

            migrationBuilder.InsertData(
                table: "CropTimings",
                columns: new[] { "ID", "CropID", "DistrictID", "FromDate", "SeasonID", "ToDate" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6833), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6835) },
                    { 2, 2, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6839), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6840) },
                    { 3, 3, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6843), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6844) },
                    { 4, 4, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6846), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6847) },
                    { 5, 5, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6850), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6851) },
                    { 6, 6, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6853), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6854) },
                    { 7, 7, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6857), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6858) },
                    { 8, 8, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6860), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6861) },
                    { 9, 9, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6864), 1, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6865) },
                    { 10, 10, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6867), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6868) },
                    { 11, 11, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6871), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6872) },
                    { 12, 12, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6882), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6884) },
                    { 13, 13, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6886), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6903) },
                    { 14, 14, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6922), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6923) },
                    { 15, 15, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6925), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6926) },
                    { 16, 16, 1, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7002), 2, new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7004) }
                });

            migrationBuilder.InsertData(
                table: "CropTranslation",
                columns: new[] { "ID", "CropID", "Image", "LanguageCode", "Text" },
                values: new object[,]
                {
                    { 1, 1, "", "EN", "Wheat" },
                    { 2, 2, "", "EN", "Sunflower" },
                    { 3, 3, "", "EN", "Cotton" },
                    { 4, 4, "", "EN", "Sugarcane" },
                    { 5, 5, "", "EN", "Onion" },
                    { 6, 6, "", "EN", "Tomato" },
                    { 7, 7, "", "EN", "Potato" },
                    { 8, 8, "", "EN", "Rice" },
                    { 9, 9, "", "EN", "Chillies" },
                    { 10, 10, "", "EN", "Soyabean" },
                    { 11, 11, "", "EN", "Bajra" },
                    { 12, 12, "", "EN", "Jowar" }
                });

            migrationBuilder.InsertData(
                table: "CropTranslation",
                columns: new[] { "ID", "CropID", "Image", "LanguageCode", "Text" },
                values: new object[,]
                {
                    { 13, 13, "", "EN", "Garlic" },
                    { 14, 14, "", "EN", "Coriander" },
                    { 15, 15, "", "EN", "Barley" },
                    { 16, 16, "", "EN", "Maize" },
                    { 17, 1, "", "UR", "گندم" },
                    { 18, 2, "", "UR", "سورج مکھی" },
                    { 19, 3, "", "UR", "کپاس" },
                    { 20, 4, "", "UR", "گنا" },
                    { 21, 5, "", "UR", "پیاز" },
                    { 22, 6, "", "UR", "ٹماٹر" },
                    { 23, 7, "", "UR", "آلو" },
                    { 24, 8, "", "UR", "چاول" },
                    { 25, 9, "", "UR", "مرچیں" },
                    { 26, 10, "", "UR", "سویا بین" },
                    { 27, 11, "", "UR", "باجرہ" },
                    { 28, 12, "", "UR", "جوار" },
                    { 29, 13, "", "UR", "Garlic" },
                    { 30, 14, "", "UR", "Coriander" },
                    { 31, 15, "", "UR", "Barley" },
                    { 32, 16, "", "UR", "Maize" }
                });

            migrationBuilder.InsertData(
                table: "DistrictLanguages",
                columns: new[] { "ID", "DistrictID", "LanguageCode", "Translation" },
                values: new object[,]
                {
                    { 1, 1, "EN", "Sindh" },
                    { 2, 2, "EN", "Punjab" },
                    { 3, 3, "EN", "Khyber Pakhtunkhwa" },
                    { 4, 4, "EN", "Balochistan" },
                    { 5, 5, "EN", "Gilgit-Baltistan" },
                    { 6, 6, "EN", "Azad Jammu and Kashmir" },
                    { 7, 1, "UR", "سندھ" },
                    { 8, 2, "UR", "پنجاب" },
                    { 9, 3, "UR", "خیبر پختونخواہ" },
                    { 10, 4, "UR", "بلوچستان" },
                    { 11, 5, "UR", "گلگت بلتستان" },
                    { 12, 6, "UR", "آزاد جموں و کشمیر" }
                });

            migrationBuilder.InsertData(
                table: "IntroLanguages",
                columns: new[] { "ID", "Image", "IntroID", "LanguageCode", "Text" },
                values: new object[,]
                {
                    { 1, "/attachments/banner-01.JPG", 1, "EN", "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable." },
                    { 2, "/attachments/banner-01.JPG", 1, "UR", "Lorem Ipsum کے اقتباسات کے بہت سے تغیرات دستیاب ہیں، لیکن اکثریت کو کسی نہ کسی شکل میں، انجکشن شدہ مزاح، یا بے ترتیب الفاظ کے ذریعے تبدیلی کا سامنا کرنا پڑا ہے جو قدرے قابل اعتبار بھی نہیں لگتے ہیں۔" },
                    { 3, "/attachments/banner-02.JPG", 2, "EN", "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable." },
                    { 4, "/attachments/banner-02.JPG", 2, "UR", "Lorem Ipsum کے اقتباسات کے بہت سے تغیرات دستیاب ہیں، لیکن اکثریت کو کسی نہ کسی شکل میں، انجکشن شدہ مزاح، یا بے ترتیب الفاظ کے ذریعے تبدیلی کا سامنا کرنا پڑا ہے جو قدرے قابل اعتبار بھی نہیں لگتے ہیں۔" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategoryTranslation",
                columns: new[] { "ID", "Image", "LanguageCode", "ProductCategoryID", "Text" },
                values: new object[,]
                {
                    { 1, "", "EN", 1, "UREA" },
                    { 2, "", "UR", 1, "یوریا" },
                    { 3, "", "EN", 2, "DAP" },
                    { 4, "", "UR", 2, "ڈی اے پی" },
                    { 5, "", "EN", 3, "Micronutrient" },
                    { 6, "", "UR", 3, "مائیکرو نیوٹرینٹ" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategoryTranslation",
                columns: new[] { "ID", "Image", "LanguageCode", "ProductCategoryID", "Text" },
                values: new object[,]
                {
                    { 7, "", "EN", 4, "Potash" },
                    { 8, "", "UR", 4, "پوٹاش" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Active", "CategoryID", "CreatedByID", "CreatedDate", "Name", "ProductCode" },
                values: new object[,]
                {
                    { 1, 1, 1, null, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7129), "UREA", "00001" },
                    { 2, 1, 1, null, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7133), "Ammonium Sulphate", "00002" },
                    { 3, 1, 1, null, new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7136), "Zabardast Urea", "00003" }
                });

            migrationBuilder.InsertData(
                table: "SeasonTranslations",
                columns: new[] { "ID", "Image", "LanguageCode", "SeasonID", "Translation" },
                values: new object[,]
                {
                    { 1, "", "EN", 1, "Rabbi" },
                    { 2, "", "EN", 2, "Kharif" },
                    { 3, "", "UR", 1, "ربی" },
                    { 4, "", "UR", 2, "خزاں" }
                });

            migrationBuilder.InsertData(
                table: "WarehouseTranslation",
                columns: new[] { "ID", "LanguageCode", "Text", "WarehouseID" },
                values: new object[,]
                {
                    { 1, "EN", "WH-Eng-1", 1 },
                    { 2, "EN", "WH-Eng-2", 2 },
                    { 3, "UR", "WH-Ur-1", 1 },
                    { 4, "UR", "WH-Ur-2", 2 }
                });

            migrationBuilder.InsertData(
                table: "tblPage",
                columns: new[] { "PageID", "ActionMethod", "Controller", "IsActive", "ModuleID", "PageIcon", "PageName", "PageOrder", "PageUrl", "ProjectModule", "ShowOnMenu" },
                values: new object[,]
                {
                    { 1, "Index", "Dashboard", 1, 1, "", "Dashboard", 1, "/Dashboard", "DMS", true },
                    { 2, "Index", "Dashboard", 1, 1, "", "Dashboard", 2, "/Dashboard", "DMS", true },
                    { 3, "Create", "Farmer", 1, 2, "", "Create Farmer", 1, "/Employee/Farmer/Create", "DMS", true },
                    { 4, "Farmer", "Farmer", 1, 2, "", "Farmers", 2, "/Employee/Farmer", "DMS", true },
                    { 5, "Index", "RoleManager", 1, 3, "", "Role Manager", 1, "/RoleManager/", "DMS", true },
                    { 6, "getRoles", "RoleManager", 1, 3, "", "get Roles", 2, "/RoleManager/getRoles", "DMS", false },
                    { 7, "PermissionManager", "RoleManager", 1, 3, "", "Permission Manager", 3, "/RoleManager/PermissionManager", "DMS", false },
                    { 8, "getPermissionsByRoleID", "RoleManager", 1, 3, "", "get Permissions By RoleID", 4, "/RoleManager/getPermissionsByRoleID", "DMS", false },
                    { 9, "updatePermissions", "RoleManager", 1, 3, "", "update Permissions", 5, "/RoleManager/updatePermissions", "DMS", false }
                });

            migrationBuilder.InsertData(
                table: "CityLanguages",
                columns: new[] { "ID", "CityID", "LanguageCode", "Translation" },
                values: new object[,]
                {
                    { 1, 1, "EN", "Karachi" },
                    { 2, 2, "EN", "Hyderabad" },
                    { 3, 3, "EN", "Sukkur" },
                    { 4, 4, "EN", "Larkana" },
                    { 5, 5, "EN", "Nawabshah" },
                    { 6, 6, "EN", "Mirpur Khas" },
                    { 7, 1, "UR", "کراچی" },
                    { 8, 2, "UR", "حیدرآباد" },
                    { 9, 3, "UR", "شکر" },
                    { 10, 4, "UR", "کانپنا" },
                    { 11, 5, "UR", "نواب شاہ" },
                    { 12, 6, "UR", "میرپور خاص" },
                    { 13, 7, "EN", "Lahore" },
                    { 14, 8, "EN", "Faisalabad" },
                    { 15, 9, "EN", "Rawalpindi" },
                    { 16, 10, "EN", "Multan" },
                    { 17, 11, "EN", "Gujranwala" },
                    { 18, 12, "EN", "Bahawalpur" },
                    { 19, 7, "UR", "لاہور" },
                    { 20, 8, "UR", "فیصل آباد" },
                    { 21, 9, "UR", "راولپنڈی" },
                    { 22, 10, "UR", "ملتان" },
                    { 23, 11, "UR", "گرینولا" },
                    { 24, 12, "UR", "بہاولپور" }
                });

            migrationBuilder.InsertData(
                table: "ProductTranslations",
                columns: new[] { "ID", "Image", "LanguageCode", "ProductID", "Text" },
                values: new object[,]
                {
                    { 1, "", "EN", 1, "UREA" },
                    { 2, "", "UR", 1, "یوریا" },
                    { 3, "", "EN", 2, "Ammonium Sulphate" },
                    { 4, "", "UR", 2, "امونیم سلفیٹ" },
                    { 5, "", "EN", 3, "Zabardast Urea" },
                    { 6, "", "UR", 3, "بھاری یوریا" }
                });

            migrationBuilder.InsertData(
                table: "Tehsils",
                columns: new[] { "ID", "CityID", "Name", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Karachi Central", 1 },
                    { 2, 1, "Karachi East", 1 },
                    { 3, 1, "Karachi West", 1 },
                    { 4, 1, "Karachi South", 1 },
                    { 5, 1, "Malir", 1 },
                    { 6, 1, "Korangi", 1 },
                    { 7, 2, "Hyderabad", 1 }
                });

            migrationBuilder.InsertData(
                table: "tblClaim",
                columns: new[] { "ClaimId", "ClaimDescription", "PageId" },
                values: new object[,]
                {
                    { "0099a508-30eb-4d7c-9a43-49b7fa87c9f7", "update Permissions", 9 },
                    { "0f845fc3-0fbb-4939-8f5f-cb720d9e9517", "Permission Manager", 7 },
                    { "63d97de3-54ad-4d81-b6e5-3f94c4e22515", "Employee Dashboard", 1 },
                    { "674b8bea-c21b-4a2b-a16b-8ac2fc382c32", "Role Manager", 5 },
                    { "6dea7f8b-a4af-436c-bdbd-bfe3dd8215d1", "get Roles", 6 }
                });

            migrationBuilder.InsertData(
                table: "tblClaim",
                columns: new[] { "ClaimId", "ClaimDescription", "PageId" },
                values: new object[,]
                {
                    { "6ffc4e4b-c2b6-4fb1-b0f4-65d9472cba17", "Farmer Listing", 4 },
                    { "e28bf186-99f1-49c2-aef1-1c88d020cc69", "Farmer Dashboard", 2 },
                    { "e7b4543e-f884-4da2-a8a3-75189981bccf", "Create Farmer", 3 },
                    { "f00c42ba-d2ae-4cee-bb55-ef9678bcfcb4", "get Permissions By RoleID", 8 }
                });

            migrationBuilder.InsertData(
                table: "TehsilLanguages",
                columns: new[] { "ID", "LanguageCode", "TehsilID", "Translation" },
                values: new object[,]
                {
                    { 1, "EN", 1, "Karachi Central" },
                    { 2, "UR", 1, "کراچی سینٹرل" },
                    { 3, "EN", 2, "Karachi East" },
                    { 4, "UR", 2, "کراچی ایسٹ" },
                    { 5, "EN", 3, "Karachi West" },
                    { 6, "UR", 3, "کراچی ویسٹ" },
                    { 7, "EN", 4, "Karachi South" },
                    { 8, "UR", 4, "کراچی جنوبی" },
                    { 9, "EN", 5, "Malir" },
                    { 10, "UR", 5, "ملیر" },
                    { 11, "EN", 6, "Hyderabad" },
                    { 12, "UR", 6, "حیدرآباد" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_tblAttachmentTypeID",
                table: "Attachment",
                column: "tblAttachmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetterDetails_AuthorityLetterID",
                table: "AuthorityLetterDetails",
                column: "AuthorityLetterID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetterDetails_ProductID",
                table: "AuthorityLetterDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetterDetails_TblProductID",
                table: "AuthorityLetterDetails",
                column: "TblProductID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetterDetails_tblServiceID",
                table: "AuthorityLetterDetails",
                column: "tblServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_CreatedByID",
                table: "AuthorityLetters",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_OrderID",
                table: "AuthorityLetters",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerLanguages_BannerID",
                table: "BannerLanguages",
                column: "BannerID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerLanguages_LanguageCode",
                table: "BannerLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_DistrictID",
                table: "Cities",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_CityLanguages_CityID",
                table: "CityLanguages",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityLanguages_LanguageCode",
                table: "CityLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_CropTimings_CropID",
                table: "CropTimings",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_CropTimings_DistrictID",
                table: "CropTimings",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_CropTimings_SeasonID",
                table: "CropTimings",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_CropTranslation_CropID",
                table: "CropTranslation",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_CropTranslation_LanguageCode",
                table: "CropTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictLanguages_DistrictID",
                table: "DistrictLanguages",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictLanguages_LanguageCode",
                table: "DistrictLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_FarmAttachments_AttachmentID",
                table: "FarmAttachments",
                column: "AttachmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmAttachments_FarmID",
                table: "FarmAttachments",
                column: "FarmID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_ApplicationID",
                table: "FarmChangeRequests",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_ApprovedBy",
                table: "FarmChangeRequests",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_CityID",
                table: "FarmChangeRequests",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_DistrictID",
                table: "FarmChangeRequests",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_FarmID",
                table: "FarmChangeRequests",
                column: "FarmID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblDistrictID",
                table: "FarmChangeRequests",
                column: "tblDistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblfarmFarmID",
                table: "FarmChangeRequests",
                column: "tblfarmFarmID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblTehsilID",
                table: "FarmChangeRequests",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_TehsilID",
                table: "FarmChangeRequests",
                column: "TehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_UserID",
                table: "FarmChangeRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProfile_CityID",
                table: "FarmerProfile",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProfile_DistrictID",
                table: "FarmerProfile",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProfile_tblTehsilID",
                table: "FarmerProfile",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProfile_UserID",
                table: "FarmerProfile",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ApplicationID",
                table: "Farms",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ApprovedBy",
                table: "Farms",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_CityID",
                table: "Farms",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_DistrictID",
                table: "Farms",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_tblDistrictID",
                table: "Farms",
                column: "tblDistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_tblTehsilID",
                table: "Farms",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_TehsilID",
                table: "Farms",
                column: "TehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_UserID",
                table: "Farms",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSliderLanguages_HomeSliderID",
                table: "HomeSliderLanguages",
                column: "HomeSliderID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSliderLanguages_LanguageCode",
                table: "HomeSliderLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_IntroLanguages_IntroID",
                table: "IntroLanguages",
                column: "IntroID");

            migrationBuilder.CreateIndex(
                name: "IX_IntroLanguages_LanguageCode",
                table: "IntroLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_MonthTranslations_LanguageCode",
                table: "MonthTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_MonthTranslations_MonthID",
                table: "MonthTranslations",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderID",
                table: "OrderProducts",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductID",
                table: "OrderProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedByID",
                table: "Orders",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_FarmID",
                table: "Plan",
                column: "FarmID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_SeasonID",
                table: "Plan",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_UserID",
                table: "Plan",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCrop_CropID",
                table: "PlanCrop",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCrop_PlanID",
                table: "PlanCrop",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProduct_PlanCropID",
                table: "PlanProduct",
                column: "PlanCropID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProduct_ProductID",
                table: "PlanProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTranslation_LanguageCode",
                table: "ProductCategoryTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTranslation_ProductCategoryID",
                table: "ProductCategoryTranslation",
                column: "ProductCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByID",
                table: "Products",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_LanguageCode",
                table: "ProductTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_ProductID",
                table: "ProductTranslations",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_ApprovedBy",
                table: "ProfileChangeRequests",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_CityID",
                table: "ProfileChangeRequests",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_DistrictID",
                table: "ProfileChangeRequests",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_tblTehsilID",
                table: "ProfileChangeRequests",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_TehsilID",
                table: "ProfileChangeRequests",
                column: "TehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_UserID",
                table: "ProfileChangeRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTranslations_LanguageCode",
                table: "SeasonTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTranslations_SeasonID",
                table: "SeasonTranslations",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CreatedByID",
                table: "Service",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTranslation_LanguageCode",
                table: "ServiceTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTranslation_ServiceID",
                table: "ServiceTranslation",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_tblClaim_PageId",
                table: "tblClaim",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_tblClaimAction_ClaimId",
                table: "tblClaimAction",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFarmerClaim_UserId",
                table: "tblFarmerClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFarmerLogin_UserId",
                table: "tblFarmerLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFarmerRole_RoleId",
                table: "tblFarmerRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "tblFarmers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "tblFarmers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblPage_ModuleID",
                table: "tblPage",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "tblRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoleClaim_ClaimValue",
                table: "tblRoleClaim",
                column: "ClaimValue");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoleClaim_RoleId",
                table: "tblRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilLanguages_LanguageCode",
                table: "TehsilLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilLanguages_TehsilID",
                table: "TehsilLanguages",
                column: "TehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_Tehsils_CityID",
                table: "Tehsils",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_tblAttachmentID",
                table: "UserAttachments",
                column: "tblAttachmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_UserID",
                table: "UserAttachments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTranslation_WarehouseID",
                table: "WarehouseTranslation",
                column: "WarehouseID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorityLetterDetails");

            migrationBuilder.DropTable(
                name: "BannerLanguages");

            migrationBuilder.DropTable(
                name: "CityLanguages");

            migrationBuilder.DropTable(
                name: "CropTimings");

            migrationBuilder.DropTable(
                name: "CropTranslation");

            migrationBuilder.DropTable(
                name: "DistrictLanguages");

            migrationBuilder.DropTable(
                name: "FarmAttachments");

            migrationBuilder.DropTable(
                name: "FarmChangeRequests");

            migrationBuilder.DropTable(
                name: "FarmerProfile");

            migrationBuilder.DropTable(
                name: "HomeSliderLanguages");

            migrationBuilder.DropTable(
                name: "IntroLanguages");

            migrationBuilder.DropTable(
                name: "MonthTranslations");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "PlanProduct");

            migrationBuilder.DropTable(
                name: "ProductCategoryTranslation");

            migrationBuilder.DropTable(
                name: "ProductTranslations");

            migrationBuilder.DropTable(
                name: "ProfileChangeRequests");

            migrationBuilder.DropTable(
                name: "SeasonTranslations");

            migrationBuilder.DropTable(
                name: "ServiceTranslation");

            migrationBuilder.DropTable(
                name: "tblClaimAction");

            migrationBuilder.DropTable(
                name: "tblFarmerClaim");

            migrationBuilder.DropTable(
                name: "tblFarmerLogin");

            migrationBuilder.DropTable(
                name: "tblFarmerRole");

            migrationBuilder.DropTable(
                name: "tblFarmerToken");

            migrationBuilder.DropTable(
                name: "tblRoleClaim");

            migrationBuilder.DropTable(
                name: "TehsilLanguages");

            migrationBuilder.DropTable(
                name: "UserAttachments");

            migrationBuilder.DropTable(
                name: "WarehouseTranslation");

            migrationBuilder.DropTable(
                name: "AuthorityLetters");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "HomeSliders");

            migrationBuilder.DropTable(
                name: "Intros");

            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropTable(
                name: "PlanCrop");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "tblClaim");

            migrationBuilder.DropTable(
                name: "tblRole");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "tblPage");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "tblNavigationModule");

            migrationBuilder.DropTable(
                name: "FarmApplication");

            migrationBuilder.DropTable(
                name: "tblFarmers");

            migrationBuilder.DropTable(
                name: "Tehsils");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
