using Amazon_Farmer.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infastructure.Persistence;
using AmazonFarmer.Infastructure.Persistence.Seeding;
using AmazonFarmer.Infastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var initialScopes = config.GetValue<string>("Authentication:GraphAPI:Scopes")?.Split(' ');

builder.Services.AddDbContext<AmazonFarmerContext>(options =>
options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DB_Env")
                    ));

builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.AddIdentity<TblUser, TblRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AmazonFarmerContext>()
.AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
//.AddRoles<TblRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();

//builder.Services.AddScoped<RoleManager<TblRole>>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(Convert.ToInt32(builder.Configuration["SessionHandler:TimeOut_Seconds"]));
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");
    try
    {
        var userManager = services.GetRequiredService<UserManager<TblUser>>();
        var roleManager = services.GetRequiredService<RoleManager<TblRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);
        logger.LogInformation("Finished Seeding Default Data");
        logger.LogInformation("Application Starting");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "An error occurred seeding the DB");
    }
}
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");
    endpoints.MapRazorPages();

});

app.MapRazorPages();

app.Run();
