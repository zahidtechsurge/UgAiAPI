using AmazonFarmer.Administrator.API.Middlewares;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services;
using AmazonFarmer.NotificationServices.Helpers;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


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

builder.Services.AddCors(policy => policy.AddPolicy("corsPolicy", build =>
{
    build.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.Configure<WsdlConfig>(builder.Configuration.GetSection("WsdlConfig"));

// Register configurations
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

builder.Services.Configure<FcmConfiguration>(builder.Configuration.GetSection("Fcm"));

builder.Services.Configure<SMSConfiguration>(builder.Configuration.GetSection("SMS"));
builder.Services.AddTransient<NotificationService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Amazon Farmer Admin API Project", Version = "v1" });
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
//enabling the cors for single domain 
//multiple domain
//any domain

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true, // To serve files with unknown MIME types
    //DefaultContentType = "application/xml", // Default MIME type if not recognized
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".plist"))
        {
            ctx.Context.Response.ContentType = "application/x-plist";
        }
        else if (ctx.File.Name.EndsWith(".ipa"))
        {
            ctx.Context.Response.ContentType = "application/octet-stream";
        }
    },
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "attachments")),
    RequestPath = "/attachments"
});

app.UseCors("corsPolicy");

app.UseAuthorization();


app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();

app.Run();
