using System;
using System.IO;
using System.Threading.Tasks;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services;
using AmazonFarmer.NotificationServices.Helpers;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.Scheduled.Payment.Services;
using AmazonFarmer.WSDL.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AmazonFarmerContext>(options =>
            options.UseSqlServer(
                    context.Configuration.GetConnectionString("DB_Env")
                    ));


        services.AddIdentity<TblUser, TblRole>(options =>
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
        .AddEntityFrameworkStores<AmazonFarmerContext>();

        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>(); // Ensure RepositoryWrapper is implemented
        services.Configure<WsdlConfig>(context.Configuration.GetSection("WsdlConfig"));
        services.AddScoped<PaymentService>();
        services.AddSingleton<FileProcessorService>();


        // Register configurations
        services.Configure<EmailConfiguration>(context.Configuration.GetSection("EmailConfiguration"));

        services.Configure<FcmConfiguration>(context.Configuration.GetSection("Fcm"));

        services.Configure<SMSConfiguration>(context.Configuration.GetSection("SMS")); 
        services.Configure<GoogleAPIConfiguration>(context.Configuration.GetSection("GoogleMaps")); 
        
        // Register NotificationService
        services.AddTransient<NotificationService>();
    })
    .Build();
//await host.RunAsync();


var fileProcessor = host.Services.GetRequiredService<FileProcessorService>();
await fileProcessor.ProcessFilesAsync();
