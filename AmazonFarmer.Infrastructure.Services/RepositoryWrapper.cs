﻿using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly SignInManager<TblUser> _signInManager;
        private readonly UserManager<TblUser> _userManager;
        private readonly RoleManager<TblRole> _roleManager;
        private AmazonFarmerContext _repoContext;
        private IComplaintRepo _complaintRepo;
        private IUserRepo _userRepo;
        private IRoleRepo _roleRepo;
        private IAuthorityLetterRepo _authorityLetterRepo;
        private ICityRepo _cityRepo;
        private ITehsilRepo _tehsilRepo;
        private IDistrictRepo _districtRepo;
        private ILanguageRepo _languageRepo;
        private IOnlinePaymentRepo _onlinePaymentRepo;
        private IAttachmentRepo _attachmentRepo;
        private IFarmRepo _farmRepo;
        private IIntroRepo _introRepo;
        private IBannerRepo _bannerRepo;
        private ISeasonRepo _seasonRepo;
        private ICropRepo _cropRepo;
        private IProductRepo _productRepo;
        private IServiceRepo _serviceRepo;
        private ILocationRepo _locationRepo;
        private IPlanRepo _planRepo;
        private IOrderRepo _orderRepo;
        private INotificationRepo _notificationRepo;
        private ILoggingRepository _loggingRepository;
        private IMonthRepo _monthRepository;
        private IWeatherRepo _weatherRepository;
        private IWarehouseRepo _warehouseRepository;
        private IReasonRepo _reasonRepository;
        public ICommonRepo _commonRepository;
        public IPaymentAcknowledgmentFileRepo _paymentAcknowledgmentFileRepo;
        public IPaymentAcknowledgmentRepo _paymentAcknowledgmentRepo;
        public IRegionRepo _regionRepository;
        public RepositoryWrapper(
            AmazonFarmerContext repositoryContext,
            SignInManager<TblUser> signInManager,
            UserManager<TblUser> userManager,
            RoleManager<TblRole> roleManager)
        {
            _repoContext = repositoryContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public IComplaintRepo ComplaintRepo
        {
            get
            {
                if (_complaintRepo == null)
                {
                    _complaintRepo = new ComplaintRepo(_repoContext);
                }
                return _complaintRepo;
            }
        }
        public IReasonRepo ReasonRepo
        {
            get
            {
                if (_reasonRepository == null)
                {
                    _reasonRepository = new ReasonRepo(_repoContext);
                }
                return _reasonRepository;
            }
        }
        public IWarehouseRepo WarehouseRepo
        {
            get
            {
                if (_warehouseRepository == null)
                {
                    _warehouseRepository = new WarehouseRepo(_repoContext);
                }
                return _warehouseRepository;
            }
        }
        public IUserRepo UserRepo
        {
            get
            {
                if (_userRepo == null)
                {
                    _userRepo = new UserRepo(_repoContext, _signInManager, _roleManager, _userManager);
                }
                return _userRepo;
            }
        }
        public IRoleRepo RoleRepo
        {
            get
            {
                if (_roleRepo == null)
                {
                    _roleRepo = new RoleRepo(_repoContext);
                }
                return _roleRepo;
            }
        }
        public IAuthorityLetterRepo AuthorityLetterRepo
        {
            get
            {
                if (_authorityLetterRepo == null)
                {
                    _authorityLetterRepo = new AuthorityLetterRepo(_repoContext);
                }
                return _authorityLetterRepo;
            }
        }
        public ICityRepo CityRepo
        {
            get
            {
                if (_cityRepo == null)
                {
                    _cityRepo = new CityRepo(_repoContext);
                }
                return _cityRepo;
            }
        }
        public IDistrictRepo DistrictRepo
        {
            get
            {
                if (_districtRepo == null)
                {
                    _districtRepo = new DistrictRepo(_repoContext);
                }
                return _districtRepo;
            }
        }
        public ITehsilRepo TehsilRepo
        {
            get
            {
                if (_tehsilRepo == null)
                {
                    _tehsilRepo = new TehsilRepo(_repoContext);
                }
                return _tehsilRepo;
            }
        }
        public ILanguageRepo LanguageRepo
        {
            get
            {
                if (_languageRepo == null)
                {
                    _languageRepo = new LanguageRepo(_repoContext);
                }
                return _languageRepo;
            }
        }
        public IAttachmentRepo AttachmentRepo
        {
            get
            {
                if (_attachmentRepo == null)
                {
                    _attachmentRepo = new AttachmentRepo(_repoContext);
                }
                return _attachmentRepo;
            }
        }
        public IFarmRepo FarmRepo
        {
            get
            {
                if (_farmRepo == null)
                {
                    _farmRepo = new FarmRepo(_repoContext);
                }
                return _farmRepo;
            }
        }
        public IIntroRepo IntroRepo
        {
            get
            {
                if (_introRepo == null)
                {
                    _introRepo = new IntroRepo(_repoContext);
                }
                return _introRepo;
            }
        }
        public IBannerRepo BannerRepo
        {
            get
            {
                if (_bannerRepo == null)
                {
                    _bannerRepo = new BannerRepo(_repoContext);
                }
                return _bannerRepo;
            }
        }
        public ISeasonRepo SeasonRepo
        {
            get
            {
                if (_seasonRepo == null)
                {
                    _seasonRepo = new SeasonRepo(_repoContext);
                }
                return _seasonRepo;
            }
        }
        public ICropRepo CropRepo
        {
            get
            {
                if (_cropRepo == null)
                {
                    _cropRepo = new CropRepo(_repoContext);
                }
                return _cropRepo;
            }
        }
        public IProductRepo ProductRepo
        {
            get
            {
                if (_productRepo == null)
                {
                    _productRepo = new ProductRepo(_repoContext);
                }
                return _productRepo;
            }
        }
        public IServiceRepo ServiceRepo
        {
            get
            {
                if (_serviceRepo == null)
                {
                    _serviceRepo = new ServiceRepo(_repoContext);
                }
                return _serviceRepo;
            }
        }
        public ILocationRepo LocationRepo
        {
            get
            {
                if (_locationRepo == null)
                {
                    _locationRepo = new LocationRepo(_repoContext);
                }
                return _locationRepo;
            }
        }
        public IPlanRepo PlanRepo
        {
            get
            {
                if (_planRepo == null)
                {
                    _planRepo = new PlanRepo(_repoContext);
                }
                return _planRepo;
            }
        }
        public ILoggingRepository LoggingRepository
        {
            get
            {
                if (_loggingRepository == null)
                {
                    _loggingRepository = new LoggingRepository(_repoContext);
                }
                return _loggingRepository;
            }
        }
        public IOnlinePaymentRepo OnlinePaymentRepo
        {
            get
            {
                if (_onlinePaymentRepo == null)
                {
                    _onlinePaymentRepo = new OnlinePaymentRepo(_repoContext);
                }
                return _onlinePaymentRepo;
            }
        }
        public IOrderRepo OrderRepo
        {
            get
            {
                if (_orderRepo == null)
                {
                    _orderRepo = new OrderRepo(_repoContext);
                }
                return _orderRepo;
            }
        }
        public INotificationRepo NotificationRepo
        {
            get
            {
                if (_notificationRepo == null)
                {
                    _notificationRepo = new NotificationRepo(_repoContext);
                }
                return _notificationRepo;
            }
        }
        public IMonthRepo MonthRepo
        {
            get
            {
                if (_monthRepository == null)
                {
                    _monthRepository = new MonthRepo(_repoContext);
                }
                return _monthRepository;
            }
        }
        public ICommonRepo CommonRepo
        {
            get
            {
                if (_commonRepository == null)
                {
                    _commonRepository = new CommonRepo(_repoContext);
                }
                return _commonRepository;
            }
        }
        public IWeatherRepo WeatherRepo
        {
            get
            {
                if (_weatherRepository == null)
                {
                    _weatherRepository = new WeatherRepo(_repoContext);
                }
                return _weatherRepository;
            }
        }

        public IPaymentAcknowledgmentFileRepo PaymentAcknowledgmentFileRepo
        {
            get
            {
                if (_paymentAcknowledgmentFileRepo == null)
                {
                    _paymentAcknowledgmentFileRepo = new PaymentAcknowledgmentFileRepo(_repoContext);
                }
                return _paymentAcknowledgmentFileRepo;
            }
        } 

        public IPaymentAcknowledgmentRepo PaymentAcknowledgmentRepo
        {
            get
            {
                if (_paymentAcknowledgmentRepo == null)
                {
                    _paymentAcknowledgmentRepo = new PaymentAcknowledgmentRepo(_repoContext);
                }
                return _paymentAcknowledgmentRepo;
            }
        }

        public IRegionRepo RegionRepo
        {
            get
            {
                if (_regionRepository == null)
                {
                    _regionRepository = new RegionRepo(_repoContext);
                }
                return _regionRepository;
            }
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }

        public async Task SaveLogEntries()
        {
            await _repoContext.SaveOnlyWSDLLogsAsync();
        }
        public async Task DisposeAsync()
        {
            await _repoContext.DisposeAsync();
        }

    }
}