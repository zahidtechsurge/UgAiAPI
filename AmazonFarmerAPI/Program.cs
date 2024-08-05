using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Persistence.Seeding;
using AmazonFarmer.Infrastructure.Services;
using AmazonFarmer.NotificationServices.Helpers;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL.Helpers; 
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using AmazonFarmerAPI.Middlewares; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var initialScopes = config.GetValue<string>("Authentication:GraphAPI:Scopes")?.Split(' ');

builder.Services.AddDbContext<AmazonFarmerContext>(options =>
options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DB_Env")
                    ));

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
//.AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
.AddDefaultTokenProviders();


builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>(); 
builder.Services.AddHttpClient();
// Bind configuration from appsettings.json
builder.Services.Configure<WsdlConfig>(builder.Configuration.GetSection("WsdlConfig"));

// Register HttpClient and configure it
//builder.Services.AddHttpClient("WebServiceClient", client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration["WsdlConfig:BaseUrl"]);
//    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
//        "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(
//            $"{builder.Configuration["WsdlConfig:UserName"]}:{builder.Configuration["WsdlConfig:Password"]}"))); 
//}).AddHttpMessageHandler<LoggingHandler>();

builder.Services.AddScoped<GoogleLocationExtension>(); // Register GoogleLocationExtension


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Amazon Farmer", Version = "v1" });
    // Define the JWT security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Here Enter JWT Token with bearer format like bearer[spcae] token"
    });
    // Require the JWT token in the request header
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Register configurations
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.Configure<FcmConfiguration>(builder.Configuration.GetSection("Fcm"));

// Register NotificationService
builder.Services.AddTransient<NotificationService>();


// Configure Email settings from appsettings.json

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateLifetime = false,
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = builder.Configuration["Jwt:Issuer"],
                      ValidAudience = builder.Configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                  };
              });
var app = builder.Build();
// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");
    try
    {
        var dbContext = scope.ServiceProvider
            .GetRequiredService<AmazonFarmerContext>();

        // Here is the migration executed
        dbContext.Database.Migrate();

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
app.UseMiddleware<RequestResponseLoggingMiddleware>();
 

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
