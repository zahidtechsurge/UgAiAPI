using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentType = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuthorityLetterHexs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HexaNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorityLetterHexs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerType = table.Column<int>(type: "int", nullable: false),
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
                    DistrictCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmailNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotifications", x => x.ID);
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
                name: "NotificationLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLog", x => x.Id);
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
                name: "Reasons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonForID = table.Column<int>(type: "int", nullable: false),
                    ReasonOfID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reasons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.RequestId);
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
                    OTPExpiredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WrongPasswordAttempt = table.Column<int>(type: "int", nullable: false),
                    isAccountLocked = table.Column<bool>(type: "bit", nullable: false),
                    isOTPApproved = table.Column<bool>(type: "bit", nullable: true),
                    LastPasswordChange = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignupAgreementDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "tblUnitOfMeasures",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUnitOfMeasures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeatherIcon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeatherType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherIcon", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WSDLLogs",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WSDLLogs", x => x.RequestId);
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Attachment_AttachmentType_tblAttachmentTypeID",
                        column: x => x.tblAttachmentTypeID,
                        principalTable: "AttachmentType",
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
                name: "ReasonTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReasonTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReasonTranslations_Reasons_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseLogs",
                columns: table => new
                {
                    ResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLogs", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_ResponseLogs_RequestLogs_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestLogs",
                        principalColumn: "RequestId",
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
                name: "Months",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    orderBy = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Months_Season_SeasonID",
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
                name: "Service",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        principalColumn: "Id");
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
                name: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    InchargeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Warehouse_tblFarmers_InchargeID",
                        column: x => x.InchargeID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
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
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesOrg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Products_tblUnitOfMeasures_UOMID",
                        column: x => x.UOMID,
                        principalTable: "tblUnitOfMeasures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasureTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasureTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasureTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasureTranslations_tblUnitOfMeasures_UOMID",
                        column: x => x.UOMID,
                        principalTable: "tblUnitOfMeasures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherIconTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeatherID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherIconTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WeatherIconTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherIconTranslations_WeatherIcon_WeatherID",
                        column: x => x.WeatherID,
                        principalTable: "WeatherIcon",
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
                name: "MonthTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthID = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "ProductConsumptionMetric",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    TerritoryID = table.Column<int>(type: "int", nullable: true),
                    Usage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConsumptionMetric", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductConsumptionMetric_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductConsumptionMetric_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                name: "FarmerProfile",
                columns: table => new
                {
                    ProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNICNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTNNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STRNNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CellNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnedLand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeasedLand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalArea = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    SAPFarmerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isApproved = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_FarmerProfile_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FarmerProfile_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    ApplicationID = table.Column<int>(type: "int", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    TehsilID = table.Column<int>(type: "int", nullable: false),
                    Acreage = table.Column<int>(type: "int", nullable: false),
                    isLeased = table.Column<bool>(type: "bit", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    isApproved = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    isFarmApprovalAcknowledgedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SAPFarmCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevertedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonID = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Farms_Reasons_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reasons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Farms_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Farms_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Farms_tblFarmers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Farms_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_ProfileChangeRequests_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileChangeRequests_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
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
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        name: "FK_FarmChangeRequests_Tehsils_TehsilID",
                        column: x => x.TehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_Tehsils_tblTehsilID",
                        column: x => x.tblTehsilID,
                        principalTable: "Tehsils",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_tblFarmers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "tblFarmers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FarmChangeRequests_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FarmID = table.Column<int>(type: "int", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    SeasonID = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_Plan_Warehouse_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouse",
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
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: true),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SAPOrderID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvancePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalDatePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDatePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoicedDatePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoicedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentOrderID = table.Column<int>(type: "int", nullable: true),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuePaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    tblPlanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Orders_Plan_PlanID",
                        column: x => x.PlanID,
                        principalTable: "Plan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Plan_tblPlanID",
                        column: x => x.tblPlanID,
                        principalTable: "Plan",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Orders_Warehouse_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
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
                name: "AuthorityLetters",
                columns: table => new
                {
                    AuthorityLetterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorityLetterNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPFarmerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    BearerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BearerNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentID = table.Column<int>(type: "int", nullable: true),
                    IsOCRAutomated = table.Column<bool>(type: "bit", nullable: false),
                    INVNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldWHIncharge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WareHouseID = table.Column<int>(type: "int", nullable: false),
                    tblwarehouseID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorityLetters", x => x.AuthorityLetterID);
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_Attachment_AttachmentID",
                        column: x => x.AttachmentID,
                        principalTable: "Attachment",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_Warehouse_WareHouseID",
                        column: x => x.WareHouseID,
                        principalTable: "Warehouse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_Warehouse_tblwarehouseID",
                        column: x => x.tblwarehouseID,
                        principalTable: "Warehouse",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuthorityLetters_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillingIquiryRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    BankMnemonic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reserved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingIquiryRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingIquiryRequest_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingPaymentRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    consumer_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankMemonic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tran_Auth_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reserved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPaymentRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingPaymentRequest_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QTY = table.Column<int>(type: "int", nullable: false),
                    ClosingQTY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.OrderProductID);
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
                name: "PlanProduct",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCropID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PlanService",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCropID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanService", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanService_PlanCrop_PlanCropID",
                        column: x => x.PlanCropID,
                        principalTable: "PlanCrop",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanService_Service_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Service",
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
                    TruckerNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "BillingIquiryResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillingInquiryRequestID = table.Column<int>(type: "int", nullable: false),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountWithInDueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountAfterDueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingMonth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tran_auth_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reserved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingIquiryResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingIquiryResponse_BillingIquiryRequest_BillingInquiryRequestID",
                        column: x => x.BillingInquiryRequestID,
                        principalTable: "BillingIquiryRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingPaymentResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillingPaymentRequestID = table.Column<int>(type: "int", nullable: false),
                    Response_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identification_parameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reserved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TblOrdersOrderID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPaymentResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingPaymentResponse_BillingPaymentRequest_BillingPaymentRequestID",
                        column: x => x.BillingPaymentRequestID,
                        principalTable: "BillingPaymentRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillingPaymentResponse_Orders_TblOrdersOrderID",
                        column: x => x.TblOrdersOrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tran_Auth_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SAPInvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPOrderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    BillPaymentRequestID = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_BillingPaymentRequest_BillPaymentRequestID",
                        column: x => x.BillPaymentRequestID,
                        principalTable: "BillingPaymentRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AuthorityLetters_AttachmentID",
                table: "AuthorityLetters",
                column: "AttachmentID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_CreatedByID",
                table: "AuthorityLetters",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_OrderID",
                table: "AuthorityLetters",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_tblwarehouseID",
                table: "AuthorityLetters",
                column: "tblwarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_WareHouseID",
                table: "AuthorityLetters",
                column: "WareHouseID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerLanguages_BannerID",
                table: "BannerLanguages",
                column: "BannerID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerLanguages_LanguageCode",
                table: "BannerLanguages",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_BillingIquiryRequest_OrderID",
                table: "BillingIquiryRequest",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingIquiryResponse_BillingInquiryRequestID",
                table: "BillingIquiryResponse",
                column: "BillingInquiryRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentRequest_OrderID",
                table: "BillingPaymentRequest",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentResponse_BillingPaymentRequestID",
                table: "BillingPaymentResponse",
                column: "BillingPaymentRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentResponse_TblOrdersOrderID",
                table: "BillingPaymentResponse",
                column: "TblOrdersOrderID");

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
                name: "IX_FarmChangeRequests_UpdatedBy",
                table: "FarmChangeRequests",
                column: "UpdatedBy");

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
                name: "IX_Farms_CityID",
                table: "Farms",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_DistrictID",
                table: "Farms",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ReasonID",
                table: "Farms",
                column: "ReasonID");

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
                name: "IX_Farms_UpdatedBy",
                table: "Farms",
                column: "UpdatedBy");

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
                name: "IX_Months_SeasonID",
                table: "Months",
                column: "SeasonID");

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
                name: "IX_Orders_CropID",
                table: "Orders",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlanID",
                table: "Orders",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_tblPlanID",
                table: "Orders",
                column: "tblPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WarehouseID",
                table: "Orders",
                column: "WarehouseID");

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
                name: "IX_Plan_WarehouseID",
                table: "Plan",
                column: "WarehouseID");

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
                name: "IX_PlanService_PlanCropID",
                table: "PlanService",
                column: "PlanCropID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanService_ServiceID",
                table: "PlanService",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTranslation_LanguageCode",
                table: "ProductCategoryTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTranslation_ProductCategoryID",
                table: "ProductCategoryTranslation",
                column: "ProductCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptionMetric_CropID",
                table: "ProductConsumptionMetric",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptionMetric_ProductID",
                table: "ProductConsumptionMetric",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByID",
                table: "Products",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UOMID",
                table: "Products",
                column: "UOMID");

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
                name: "IX_ReasonTranslations_LanguageCode",
                table: "ReasonTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_ReasonTranslations_ReasonID",
                table: "ReasonTranslations",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseLogs_RequestId",
                table: "ResponseLogs",
                column: "RequestId");

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
                name: "IX_Transactions_BillPaymentRequestID",
                table: "Transactions",
                column: "BillPaymentRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasureTranslations_LanguageCode",
                table: "UnitOfMeasureTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasureTranslations_UOMID",
                table: "UnitOfMeasureTranslations",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_tblAttachmentID",
                table: "UserAttachments",
                column: "tblAttachmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_UserID",
                table: "UserAttachments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_InchargeID",
                table: "Warehouse",
                column: "InchargeID");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTranslation_WarehouseID",
                table: "WarehouseTranslation",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherIconTranslations_LanguageCode",
                table: "WeatherIconTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherIconTranslations_WeatherID",
                table: "WeatherIconTranslations",
                column: "WeatherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorityLetterDetails");

            migrationBuilder.DropTable(
                name: "AuthorityLetterHexs");

            migrationBuilder.DropTable(
                name: "BannerLanguages");

            migrationBuilder.DropTable(
                name: "BillingIquiryResponse");

            migrationBuilder.DropTable(
                name: "BillingPaymentResponse");

            migrationBuilder.DropTable(
                name: "CityLanguages");

            migrationBuilder.DropTable(
                name: "CropTimings");

            migrationBuilder.DropTable(
                name: "CropTranslation");

            migrationBuilder.DropTable(
                name: "DistrictLanguages");

            migrationBuilder.DropTable(
                name: "EmailNotifications");

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
                name: "NotificationLog");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "PlanProduct");

            migrationBuilder.DropTable(
                name: "PlanService");

            migrationBuilder.DropTable(
                name: "ProductCategoryTranslation");

            migrationBuilder.DropTable(
                name: "ProductConsumptionMetric");

            migrationBuilder.DropTable(
                name: "ProductTranslations");

            migrationBuilder.DropTable(
                name: "ProfileChangeRequests");

            migrationBuilder.DropTable(
                name: "ReasonTranslations");

            migrationBuilder.DropTable(
                name: "ResponseLogs");

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
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UnitOfMeasureTranslations");

            migrationBuilder.DropTable(
                name: "UserAttachments");

            migrationBuilder.DropTable(
                name: "WarehouseTranslation");

            migrationBuilder.DropTable(
                name: "WeatherIconTranslations");

            migrationBuilder.DropTable(
                name: "WSDLLogs");

            migrationBuilder.DropTable(
                name: "AuthorityLetters");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "BillingIquiryRequest");

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
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "tblClaim");

            migrationBuilder.DropTable(
                name: "tblRole");

            migrationBuilder.DropTable(
                name: "BillingPaymentRequest");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "WeatherIcon");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "tblUnitOfMeasures");

            migrationBuilder.DropTable(
                name: "tblPage");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AttachmentType");

            migrationBuilder.DropTable(
                name: "tblNavigationModule");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "FarmApplication");

            migrationBuilder.DropTable(
                name: "Reasons");

            migrationBuilder.DropTable(
                name: "Tehsils");

            migrationBuilder.DropTable(
                name: "tblFarmers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
