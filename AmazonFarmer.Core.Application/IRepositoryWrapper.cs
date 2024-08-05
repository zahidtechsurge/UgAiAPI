using AmazonFarmer.Core.Application.Interfaces; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application
{
    public interface IRepositoryWrapper // Defining the interface for Repository Wrapper
    {
        public IUserRepo UserRepo { get; } // Property for User repository
        public IRoleRepo RoleRepo { get; } // Property for Role repository
        public IAuthorityLetterRepo AuthorityLetterRepo { get; } // Property for Authority Letter repository
        public ICityRepo CityRepo { get; } // Property for City repository
        public ITehsilRepo TehsilRepo { get; } // Property for Tehsil repository
        public IDistrictRepo DistrictRepo { get; } // Property for District repository
        public ILanguageRepo LanguageRepo { get; } // Property for Language repository
        public IAttachmentRepo AttachmentRepo { get; } // Property for Attachment repository
        public IFarmRepo FarmRepo { get; } // Property for Farm repository
        public IIntroRepo IntroRepo { get; } // Property for Intro repository
        public IBannerRepo BannerRepo { get; } // Property for Banner repository
        public ISeasonRepo SeasonRepo { get; } // Property for Season repository
        public ICropRepo CropRepo { get; } // Property for Crop repository
        public IProductRepo ProductRepo { get; } // Property for Product repository
        public IServiceRepo ServiceRepo { get; } // Property for Service repository
        public ILocationRepo LocationRepo { get; } // Property for Location repository
        public IPlanRepo PlanRepo { get; } // Property for Plan repository
        public ILoggingRepository LoggingRepository { get; } // Property for Logging repository
        public IOnlinePaymentRepo OnlinePaymentRepo { get; } // Property for Online Payment repository
        public IOrderRepo OrderRepo { get; } // Property for Order repository
        public INotificationRepo NotificationRepo { get; } // Property for Order repository
        public IMonthRepo MonthRepo { get; } // Property for Month repository
        public IWeatherRepo WeatherRepo { get; } // Property for Weather repository
        public IWarehouseRepo WarehouseRepo { get; } // Property for Warehouse repository
        public IReasonRepo ReasonRepo { get; } // Property for Reason repository
        public ICommonRepo CommonRepo { get; } // Property for Common repository
        IPaymentAcknowledgmentFileRepo PaymentAcknowledgmentFileRepo { get; }
        IPaymentAcknowledgmentRepo PaymentAcknowledgmentRepo { get; }
        public void Save(); // Method signature for saving changes synchronously
        public Task SaveAsync(); // Method signature for saving changes asynchronously
        public Task SaveLogEntries();
        public Task DisposeAsync(); // Method signature for Disposing changes asynchronously 
    }
}
