using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing crops in the database.
    /// </summary>
    public class CropRepo : ICropRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the CropRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public CropRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public IQueryable<tblCrop> GetCrops()
        {
            return _context.Crops;
        }
        public async Task<List<tblCropTranslation>> GetCropTranslationByCropID(int CropID)
        {
            return await _context.CropTranslation
                .Include(x => x.Crop)
                .Include(x => x.Language)
                .Where(x => x.CropID == CropID)
                .ToListAsync();
        }
        public async Task<tblCropTranslation?> GetCropTranslationByCropID(int CropID, string LanguageCode)
        {
            return await _context.CropTranslation
                .Include(x => x.Crop)
                .Include(x => x.Language)
                .Where(x => x.CropID == CropID && x.LanguageCode == LanguageCode)
                .FirstOrDefaultAsync();
        }

        public async Task<List<tblCropTimings>> GetCropTimingsByCropID(int CropID)
        {
            return await _context.CropTimings
                .Include(x => x.Crop)
                .Include(x => x.Season)
                .Include(x => x.District)
                .Where(x => x.CropID == CropID)
                .ToListAsync();
        }
        public async Task<tblCropTimings?> GetCropTimingByID(int ID)
        {
            return await _context.CropTimings.Where(x => x.ID == ID).FirstOrDefaultAsync();
        }
        public async Task<tblCropTimings?> GetCropTimingByID(int CropID, int SeasonID, int DistrictID, int fromDate, int toDate)
        {
            return await _context.CropTimings.Where(x =>
                x.CropID == CropID &&
                x.SeasonID == SeasonID &&
                x.DistrictID == DistrictID &&
                x.FromDate.Month == fromDate &&
                x.ToDate.Month == toDate
            ).FirstOrDefaultAsync();
        }
        public void AddCropTranslation(tblCropTranslation ct)
        {
            _context.CropTranslation.Add(ct);
        }
        public void UpdateCropTranslation(tblCropTranslation ct)
        {
            _context.CropTranslation.Update(ct);
        }
        public void AddCropTiming(tblCropTimings ct)
        {
            _context.CropTimings.Add(ct);
        }
        public void UpdateCropTiming(tblCropTimings ct)
        {
            _context.CropTimings.Update(ct);
        }
        /// <summary>
        /// Retrieves a list of crops by season and district ID.
        /// </summary>
        /// <param name="req">The request containing the season ID, district ID, farm ID, and language code.</param>
        /// <returns>A list of Crops_Res objects.</returns>
        public async Task<List<Crops_Res>> getCropsBySeasonAndDistrictID(GetCropDTO_Internal_req req)
        {
            // Retrieve the district ID of the farm
            int districtID = await _context.Farms
                .Where(x => x.FarmID == req.farmID)
                .Select(x => x.DistrictID)
                .FirstOrDefaultAsync();

            // Query crops based on season, district, and language code
            List<Crops_Res> crops = await _context.CropTimings
                .Include(x => x.Crop)
                .ThenInclude(x => x.CropTranslations/*.Where(ct => ct.Status == EActivityStatus.Active)*/)
                .Include(x => x.Crop)
                .ThenInclude(x => x.ProductConsumptionMetrics.Where(pc => pc.TerritoryID == districtID))
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductTranslations)
                .Where(x =>
                    x.Crop.CropTranslations.Any(x => x.LanguageCode == req.languageCode) &&
                    x.DistrictID == districtID &&
                    //x.SeasonID == req.seasonID &&
                    (DateTime.UtcNow.Month >= x.FromDate.Month && DateTime.UtcNow.Month <= x.ToDate.Month) //&&
                    //x.Status == EActivityStatus.Active
                )
                .Select(x => new Crops_Res
                {
                    cropID = x.CropID,
                    cropName = x.Crop.CropTranslations
                        .Where(x => x.CropID == x.CropID && x.LanguageCode == req.languageCode)
                        .Select(x => x.Text)
                        .FirstOrDefault(),
                    filePath = string.Concat(req.basePath, x.Crop.CropTranslations
                        .Where(x => x.CropID == x.CropID && x.LanguageCode == req.languageCode)
                        .Select(x => x.Image.Replace("/", "%2F").Replace(" ", "%20"))
                        .FirstOrDefault()),
                    suggestion = x.Crop.ProductConsumptionMetrics
                        .Select(x => new ConsumptionMatrixDTO
                        {
                            name = x.Product.ProductTranslations
                                .Where(x => x.LanguageCode == req.languageCode)
                                .FirstOrDefault().Text,
                            qty = x.Usage,
                            uom = x.UOM
                        }).ToList()
                }).ToListAsync();

            return crops;
        }
    }
}
