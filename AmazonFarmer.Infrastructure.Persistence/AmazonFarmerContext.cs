using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AmazonFarmer.Infrastructure.Persistence
{
    public partial class AmazonFarmerContext : IdentityDbContext<TblUser, TblRole, string,
                    TblFarmerClaim, TblFarmerRole, TblFarmerLogin,
                    TblRoleClaim, TblFarmerToken>
    {
        public AmazonFarmerContext()
        {

        }
        public AmazonFarmerContext(DbContextOptions<AmazonFarmerContext> options)
            : base(options)
        {
        }
        public DbSet<TblUser> Users { get; set; }
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
        public DbSet<TblAuthorityLetters> AuthorityLetters { get; set; }
        public DbSet<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; }
        public DbSet<tblAttachment> Attachment { get; set; }
        public DbSet<tblAttachmentTypes> AttachmentTypes { get; set; }
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

        //Request Response Logging
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<ResponseLog> ResponseLogs { get; set; }
        public DbSet<WSDLLog> WSDLLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);

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

            //// Define the relationship between FarmerProfile and Tehsil
            //modelBuilder.Entity<tblFarmerProfile>()
            //    .HasOne(f => f.Tehsil)  // Specify navigation property
            //    .WithMany()             // Specify inverse navigation property
            //    .HasForeignKey(f => f.TehsilID)  // Specify foreign key property
            //    .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior
            //                                        // Define the relationship between Farms and Tehsils
            modelBuilder.Entity<tblfarm>()
                .HasOne(f => f.Tehsil)
                .WithMany()
                .HasForeignKey(f => f.TehsilID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            // Define the relationship between ProfileChangeRequests and Tehsils
            modelBuilder.Entity<tblProfileChangeRequest>()
                .HasOne(pcr => pcr.Tehsil)
                .WithMany()
                .HasForeignKey(pcr => pcr.TehsilID)
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            // Define the relationship between FarmChangeRequests and Tehsils
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.Tehsil)
                .WithMany()
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
                .WithMany()
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
                .WithMany()
                .HasForeignKey(fcr => fcr.DistrictID)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your requirements
            modelBuilder.Entity<tblFarmChangeRequest>()
                .HasOne(fcr => fcr.Farm)
                .WithMany()
                .HasForeignKey(fcr => fcr.FarmID)
                .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION or ON UPDATE NO ACTION based on your requirement
            modelBuilder.Entity<TblAuthorityLetterDetails>()
                .HasOne(ald => ald.Products)
                .WithMany()
                .HasForeignKey(ald => ald.ProductID)
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
    }
}