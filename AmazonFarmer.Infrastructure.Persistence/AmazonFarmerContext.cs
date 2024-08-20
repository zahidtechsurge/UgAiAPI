using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace AmazonFarmer.Infrastructure.Persistence
{
    public partial class AmazonFarmerContext : IdentityDbContext<TblUser, TblRole, string,
                    TblFarmerClaim, TblFarmerRole, TblFarmerLogin,
                    TblRoleClaim, TblFarmerToken>
    {
        //private readonly UserManager<TblUser> _userManager;
        //private readonly RoleManager<TblRole> _roleManager;
        private readonly IServiceProvider _serviceProvider;
        //private readonly DbContextOptions<DbContext> _context;

        public AmazonFarmerContext(DbContextOptions<AmazonFarmerContext> options)
            : base(options)
        {
            //_userManager = userManager;
            //_roleManager = roleManager;
            //_serviceProvider = serviceProvider;
            //_context = options;
        }
        public AmazonFarmerContext()
        {

        }
        public DbSet<TblUser> Users { get; set; }
        public DbSet<ActiveToken> ActiveTokens { get; set; }
        public DbSet<TblFarmerRole> FarmerRoles { get; set; }
        public DbSet<TblClaim> Claims { get; set; }
        public DbSet<TblClaimAction> ClaimActions { get; set; }
        public DbSet<TblNavigationModule> NavigationModules { get; set; }
        public DbSet<TblPage> Pages { get; set; }
        public DbSet<TblRole> Roles { get; set; }
        public DbSet<TblRoleClaim> RoleClaims { get; set; }
        public DbSet<TblProduct> Products { get; set; }
        public DbSet<TblOrders> Orders { get; set; }
        public DbSet<TblOrderProducts> OrderProducts { get; set; }
        public DbSet<TblOrderService> OrderServices { get; set; }
        public DbSet<tblAuthorityLetter_Hexs> AuthorityLetterHexs { get; set; }
        public DbSet<TblAuthorityLetters> AuthorityLetters { get; set; }
        public DbSet<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; }
        public DbSet<tblAttachment> Attachment { get; set; }
        public DbSet<tblAttachmentTypes> AttachmentType { get; set; }
        public DbSet<tblBanner> Banners { get; set; }
        public DbSet<tblBannerLanguages> BannerLanguages { get; set; }
        public DbSet<tblCity> Cities { get; set; }
        public DbSet<tblCityLanguages> CityLanguages { get; set; }
        public DbSet<tblCrop> Crops { get; set; }
        public DbSet<tblCropTimings> CropTimings { get; set; }
        public DbSet<tblSeason> Season { get; set; }
        public DbSet<tblDistrict> District { get; set; }
        public DbSet<tblCropTranslation> CropTranslation { get; set; }
        public DbSet<tblDistrictLanguages> DistrictLanguages { get; set; }
        public DbSet<tblfarm> Farms { get; set; }
        public DbSet<tblFarmAttachments> FarmAttachments { get; set; }
        public DbSet<tblFarmChangeRequest> FarmChangeRequests { get; set; }
        public DbSet<tblHomeSlider> HomeSliders { get; set; }
        public DbSet<tblHomeSliderLanguages> HomeSliderLanguages { get; set; }
        public DbSet<tblIntro> Intros { get; set; }
        public DbSet<tblIntroLanguages> IntroLanguages { get; set; }
        public DbSet<tblLanguages> Languages { get; set; }
        public DbSet<tblMonth> Months { get; set; }
        public DbSet<tblMonthTranslation> MonthTranslations { get; set; }
        public DbSet<tblProductTranslation> ProductTranslations { get; set; }
        public DbSet<tblProfileChangeRequest> ProfileChangeRequests { get; set; }
        public DbSet<tblSeasonTranslation> SeasonTranslations { get; set; }
        public DbSet<tblTehsil> Tehsils { get; set; }
        public DbSet<tblTehsilLanguages> TehsilLanguages { get; set; }
        public DbSet<tblUserAttachments> UserAttachments { get; set; }
        public DbSet<tblFarmerProfile> FarmerProfile { get; set; }
        public DbSet<tblService> Service { get; set; }
        public DbSet<tblServiceTranslation> ServiceTranslation { get; set; }
        public DbSet<tblProductCategory> ProductCategory { get; set; }
        public DbSet<tblProductCategoryTranslation> ProductCategoryTranslation { get; set; }
        public DbSet<tblFarmApplication> FarmApplication { get; set; }
        public DbSet<tblwarehouse> Warehouse { get; set; }
        public DbSet<tblwarehouseTranslation> WarehouseTranslation { get; set; }
        public DbSet<tblPlan> Plan { get; set; }
        public DbSet<tblPlanCrops> PlanCrop { get; set; }
        public DbSet<tblPlanProduct> PlanProduct { get; set; }
        public DbSet<tblPlanService> PlanService { get; set; }
        public DbSet<tblProductConsumptionMetrics> ProductConsumptionMetric { get; set; }
        public DbSet<tblBillingIquiryRequest> BillingIquiryRequest { get; set; }
        public DbSet<tblBillingIquiryResponse> BillingIquiryResponse { get; set; }
        public DbSet<tblBillingPaymentRequest> BillingPaymentRequest { get; set; }
        public DbSet<tblBillingPaymentResponse> BillingPaymentResponse { get; set; }
        public DbSet<tblTransaction> Transactions { get; set; }
        public DbSet<tblEmailNotifications> EmailNotifications { get; set; }
        public DbSet<tblEmailNotificationTranslations> EmailNotificationTranslations { get; set; }
        public DbSet<tblUnitOfMeasure> tblUnitOfMeasures { get; set; }
        public DbSet<tblUnitOfMeasureTranslation> UnitOfMeasureTranslations { get; set; }
        public DbSet<tblWeatherIcon> WeatherIcon { get; set; }
        public DbSet<tblWeatherIconTranslation> WeatherIconTranslations { get; set; }
        public DbSet<tblReasons> Reasons { get; set; }
        public DbSet<tblReasonTranslation> ReasonTranslations { get; set; }
        public DbSet<tblRegion> Regions { get; set; }
        public DbSet<tblRegionLanguages> RegionLanguages { get; set; }
        public DbSet<TblEmployeeDistrictAssignment> EmployeeDistrictAssignments { get; set; }
        public DbSet<TblEmployeeRegionAssignment> EmployeeRegionAssignments { get; set; }
        public DbSet<tblTransactionLog> TransactionLogs { get; set; }
        public DbSet<tblConfig> ConfigurationTable { get; set; }
        public DbSet<TblOrderLog> OrderLogs { get; set; }
        public DbSet<tblNotification> Notifications { get; set; }
        public DbSet<tblDeviceNotifications> DeviceNotification { get; set; }
        public DbSet<tblDeviceNotificationTranslation> DeviceNotificationTranslations { get; set; }

        //public DbSet<tblSequenceTranslation> SequenceTranslations { get; set; }

        //Request Response Logging
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<ResponseLog> ResponseLogs { get; set; }
        public DbSet<WSDLLog> WSDLLogs { get; set; }
        public DbSet<NotificationLog> NotificationLog { get; set; }
        public DbSet<FarmUpdateLog> FarmUpdateLog { get; set; }
        public DbSet<PaymentAcknowledgment> PaymentAcknowledgments { get; set; }
        public DbSet<PaymentAcknowledgmentFile> PaymentAcknowledgmentFiles { get; set; }
        public DbSet<tblCropGroup> CropGroup { get; set; }
        public DbSet<tblCropGroupCrops> CropGroupCrops { get; set; }
        public DbSet<PlanStatusResult> SP_PlanStatusResult { get; set; }
        public DbSet<PlanSeasonCropResult> SP_PlanSeasonCropResult { get; set; }
        public DbSet<SP_FarmerDetailsResult> SP_FarmerDetailsResult { get; set; }
        public DbSet<SP_LogEntryResult> SP_LogEntryResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SeedData(modelBuilder).GetAwaiter().GetResult();
            //modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlanStatusResult>().ToView("View_PlanStatusResult");
            modelBuilder.Entity<PlanSeasonCropResult>().ToView("View_PlanSeasonCropResult");
            modelBuilder.Entity<SP_FarmerDetailsResult>().ToView("View_FarmerDetailsResult");
            modelBuilder.Entity<SP_LogEntryResult>().ToView("View_LogEntryResult");

            modelBuilder.Entity<TblUser>(b =>
            {
                b.ToTable("tblFarmers");

                // Each Farmer can have many FarmerClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each Farmer can have many FarmerLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each Farmer can have many FarmerTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each Farmer can have many entries in the FarmerRole join table
                b.HasMany(e => e.FarmerRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            });
            modelBuilder.Entity<TblOrders>()
            .HasOne(o => o.Plan)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.PlanID)
            .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION

            modelBuilder.Entity<tblTransaction>()
            .HasOne(t => t.Order)
            .WithMany(o => o.Transactions)
            .HasForeignKey(t => t.OrderID)
            .OnDelete(DeleteBehavior.Restrict);  // Specify ON DELETE NO ACTION

            //// Define the relationship between FarmerProfile and Tehsil
            //modelBuilder.Entity<tblFarmerProfile>()
            //    .HasOne(f => f.Tehsil)  // Specify navigation property
            //    .WithMany()             // Specify inverse navigation property
            //    .HasForeignKey(f => f.TehsilID)  // Specify foreign key property
            //    .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior
            //                                        // Define the relationship between Farms and Tehsils
            modelBuilder.Entity<tblfarm>()
                .HasOne(f => f.Tehsil)
                .WithMany(t => t.farms)
                .HasForeignKey(f => f.TehsilID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            // Define the relationship between ProfileChangeRequests and Tehsils
            modelBuilder.Entity<tblProfileChangeRequest>()
                .HasOne(pcr => pcr.Tehsil)
                .WithMany(t => t.ProfileChangeRequest)
                .HasForeignKey(pcr => pcr.TehsilID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            // Define the relationship between FarmChangeRequests and Tehsils
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.Tehsil)
                .WithMany(t => t.FarmChangeRequests)
                .HasForeignKey(fcr => fcr.TehsilID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior
                                                    // Define the relationship between Plan and Farmer
            modelBuilder.Entity<tblPlan>()
                .HasOne(p => p.Farm)
                .WithMany(f => f.plans)
                .HasForeignKey(p => p.FarmID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            modelBuilder.Entity<tblPlan>()
            .HasOne(p => p.Farm)
            .WithMany(f => f.plans)
            .HasForeignKey(p => p.FarmID); // Assuming UserID matches FarmID

            modelBuilder.Entity<tblPlanCrops>()
           .HasOne(p => p.CropGroup)
           .WithMany(f => f.PlanCrops)
           .HasForeignKey(p => p.CropGroupID); // Assuming UserID matches FarmID

            modelBuilder.Entity<TblOrderProducts>()
            .HasOne(p => p.PlanProduct)
            .WithOne(f => f.OrderProduct)
            .HasForeignKey<TblOrderProducts>(p => p.PlanProductID); // Assuming UserID matches FarmID


            modelBuilder.Entity<tblPlanProduct>()
            .HasOne(pp => pp.Product)
            .WithMany(p => p.PlanProducts)
            .HasForeignKey(pp => pp.ProductID)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.SetNull, depending on your requirements

            modelBuilder.Entity<tblProfileChangeRequest>()
                .HasOne(pcr => pcr.User)
                .WithMany(u => u.ProfileChangeRequest)
                .HasForeignKey(pcr => pcr.UserID);

            // Configure the foreign key relationship
            modelBuilder.Entity<tblfarm>()
                .HasOne(f => f.District)
                .WithMany(d => d.farms)
                .HasForeignKey(f => f.DistrictID)
                .OnDelete(DeleteBehavior.Restrict); // Specify the desired cascade delete behavior
            modelBuilder.Entity<tblProfileChangeRequest>()
                .HasOne(p => p.District)
                .WithMany(d => d.ProfileChangeRequest) // Assuming this is the navigation property in tblDistrict pointing back to tblProfileChangeRequest
                .HasForeignKey(p => p.DistrictID)
                .OnDelete(DeleteBehavior.Restrict); // Change to DeleteBehavior.Cascade if appropriate
            modelBuilder.Entity<tblFarmerProfile>()
                .HasOne(p => p.District)
                .WithMany(d => d.FarmerProfiles) // Assuming this is the navigation property in tblDistrict pointing back to tblProfileChangeRequest
                .HasForeignKey(p => p.DistrictID)
                .OnDelete(DeleteBehavior.Restrict); // Change to DeleteBehavior.Cascade if appropriate

            modelBuilder.Entity<TblUser>()
                .HasMany(f => f.AuthorityLetters)
                .WithOne(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your requirements

            modelBuilder.Entity<TblOrderProducts>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductID)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your requirements
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.District)
                .WithMany(d => d.tblFarmChangeRequest)
                .HasForeignKey(fcr => fcr.DistrictID)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your requirements
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.Farm)
                .WithMany(f => f.FarmChangeRequests)
                .HasForeignKey(fcr => fcr.FarmID)
                .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION or ON UPDATE NO ACTION based on your requirement
            modelBuilder.Entity<TblAuthorityLetterDetails>()
                .HasOne(ald => ald.Products)
                .WithMany(p => p.AuthorityLetterDetails)
                .HasForeignKey(ald => ald.ProductID)
                .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION or ON UPDATE NO ACTION based on your requirement
            modelBuilder.Entity<TblAuthorityLetters>()
                .HasOne(ald => ald.Warehouse)
                .WithMany(w => w.AuthorityLetters)
                .HasForeignKey(ald => ald.WareHouseID)
                .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION or ON UPDATE NO ACTION based on your requirement
            modelBuilder.Entity<tblDistrict>()
                .HasMany(d => d.ProfileChangeRequest)
                .WithOne(p => p.District)
                .HasForeignKey(p => p.DistrictID)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<tblTehsil>()
            //    .HasOne(t => t.District)
            //    .WithMany(d => d.Tehsils)
            //    .HasForeignKey(t => t.DistrictID)
            //    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<TblUser>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<tblfarm>()
                .HasOne(f => f.Users)
                .WithMany(u => u.farms)
                .HasForeignKey(f => f.UserID);
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.User)
                .WithMany(u => u.tblFarmChangeRequest)
                .HasForeignKey(fcr => fcr.UserID);
            //Id as auto generated GUID
            modelBuilder.Entity<TblRole>(b =>
            {

                b.ToTable("tblRole");

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.FarmerRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            });

            modelBuilder.Entity<TblClaim>(b =>
            {
                b.ToTable("tblClaim");

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.ClaimActions)
                    .WithOne(e => e.Claim)
                    .HasForeignKey(ur => ur.ClaimId)
                    .IsRequired();
            });

            modelBuilder.Entity<TblUser>()
            .HasOne(u => u.Token)
            .WithOne(up => up.User)
            .HasForeignKey<ActiveToken>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TblNavigationModule>(b =>
            {
                b.ToTable("tblNavigationModule");
            });
            modelBuilder.Entity<TblRoleClaim>(b =>
            {
                b.ToTable("tblRoleClaim");
            });
            modelBuilder.Entity<TblFarmerClaim>(b =>
            {
                b.ToTable("tblFarmerClaim");
            });
            modelBuilder.Entity<TblFarmerLogin>(b =>
            {
                b.ToTable("tblFarmerLogin");
            });
            modelBuilder.Entity<TblFarmerRole>(b =>
            {
                b.ToTable("tblFarmerRole");
            });
            modelBuilder.Entity<TblFarmerToken>(b =>
            {
                b.ToTable("tblFarmerToken");
            });
            modelBuilder.Entity<TblClaimAction>(b =>
            {
                b.ToTable("tblClaimAction");
            });
            modelBuilder.Entity<TblPage>(b =>
            {
                b.ToTable("tblPage");
            });

        }

        private async Task SeedData(ModelBuilder modelBuilder)

        {

            using (var serviceScope = _serviceProvider.CreateScope())

            {

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<TblUser>>();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<TblRole>>();
                await SeedRoles(roleManager);
                await SeedUsers(userManager);


            }
        }

        private async Task SeedRoles(RoleManager<TblRole> _roleManager)
        {
            // Seed roles using RoleManager
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new TblRole("Admin", ERoles.Admin));
            }
            // Seed roles using RoleManager
            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                await _roleManager.CreateAsync(new TblRole("Employee", ERoles.Employee));
            }
            // Seed roles using RoleManager
            if (!await _roleManager.RoleExistsAsync("Farmer"))
            {
                await _roleManager.CreateAsync(new TblRole("Farmer", ERoles.Farmer));
            }
            // Seed roles using RoleManager
            if (!await _roleManager.RoleExistsAsync("OneLink"))
            {
                await _roleManager.CreateAsync(new TblRole("OneLink", ERoles.OneLink));
            }
            // Add more roles if needed
        }

        private async Task SeedUsers(UserManager<TblUser> _userManager)
        {
            if (await _userManager.FindByNameAsync("syedtalha") == null)
            {
                var user = new TblUser
                {
                    Id = "a35b9178-2859-4826-9583-430c71887b341",
                    FirstName = "Syed Talha",
                    LastName = "Admin",
                    Email = "syed.talha@techsurgeinc.com",
                    UserName = "syedtalha",
                    NormalizedUserName = "syedtalha"
                };

                IdentityResult temp = await _userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            if (await _userManager.FindByNameAsync("bakar") == null)
            {
                var user = new TblUser
                {
                    Id = "3ad7592f-12ae-4eca-a96d-278e1155135c",
                    FirstName = "Bakar",
                    LastName = "Farmer",
                    Email = "noreply@engro.com",
                    UserName = "bakar",
                    NormalizedUserName = "bakar"
                };

                IdentityResult temp = await _userManager.CreateAsync(user, "Bakar123@");
                if (temp.Succeeded == true)
                {
                    try
                    {


                        await _userManager.AddToRoleAsync(user, "Farmer");
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            if (await _userManager.FindByNameAsync("Am@zonEngro") == null)
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

                IdentityResult temp = await _userManager.CreateAsync(user, "@Am@z0n$engr0+=");
                if (temp.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(user, "OneLink");
                }
            }

        }


        public async Task<int> SaveOnlyWSDLLogsAsync()
        {
            // Get all entries
            var allEntries = this.ChangeTracker.Entries().ToList();

            // Get WSDLLogs entries that are Added, Modified, or Deleted
            var wsdlLogEntries = allEntries
                .Where(e => e.Entity is WSDLLog &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                .ToList();

            // Store the original states of non-WSDLLog entries
            var originalStates = new Dictionary<EntityEntry, EntityState>();
            foreach (var entry in allEntries)
            {
                if (!(entry.Entity is WSDLLog))
                {
                    originalStates[entry] = entry.State;
                    entry.State = EntityState.Unchanged;
                }
            }

            // Save changes for WSDLLogs entries
            var result = await base.SaveChangesAsync();

            // Restore the original states of non-WSDLLog entries
            foreach (var entry in originalStates)
            {
                entry.Key.State = entry.Value;
            }

            return result;
        }
    }
}