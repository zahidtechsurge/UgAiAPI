using AmazonFarmer.Core.Application.Interfaces;

namespace AmazonFarmer.Core.Application
{
    public interface IRepositoryWrapper
    {
        public IUserRepo UserRepo { get; }
        public IRoleRepo RoleRepo { get; }
        public IAuthorityLetterRepo AuthorityLetterRepo { get; }
        public ICityRepo CityRepo { get; }
        public ITehsilRepo TehsilRepo { get; }
        public IDistrictRepo DistrictRepo { get; }
        public ILanguageRepo LanguageRepo { get; }
        public IAttachmentRepo AttachmentRepo { get; }
        public IFarmRepo FarmRepo { get; }
        public IIntroRepo IntroRepo { get; }
        public IBannerRepo BannerRepo { get; }
        public ISeasonRepo SeasonRepo { get; }
        public ICropRepo CropRepo { get; }
        public IProductRepo ProductRepo { get; }
        public IServiceRepo ServiceRepo { get; }
        public ILocationRepo LocationRepo { get; }
        public IPlanRepo PlanRepo { get; }
        public ILoggingRepository LoggingRepository { get; }



        public void Save();
        public Task SaveAsync();
    }
}