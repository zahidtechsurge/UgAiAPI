using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class CropRepo : ICropRepo
    {
        private AmazonFarmerContext _context;

        public CropRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<Crops_Res>> getCropsBySeasonAndDistrictID(GetCropDTO_Internal_req req)
        {
            int distictID = await _context.Farms.Where(x=>x.FarmID == req.farmID).Select(x=>x.DistrictID).FirstOrDefaultAsync();

            //List<ConsumptionMatrixDTO> dummySuggestions = new List<ConsumptionMatrixDTO>()
            //{
            //    new ConsumptionMatrixDTO()
            //    {
            //        name = "DAP",
            //        qty = (100).ToString("N2"),
            //        uom = "Bags"
            //    },
            //    new ConsumptionMatrixDTO()
            //    {
            //        name = "Urea",
            //        qty = (300).ToString("N2"),
            //        uom = "Bags"
            //    }
            //};

            List<Crops_Res> crops =  await _context.CropTimings
                .Include(x => x.Crop)
                .ThenInclude(x => x.CropTranslations)
                .Include(x=>x.Crop)
                .ThenInclude(x=>x.ProductConsumptionMetrics)
                .ThenInclude(x=>x.Product)
                .ThenInclude(x=>x.ProductTranslations)
                .Where(x =>
                    x.Crop.CropTranslations.Any(x => x.LanguageCode == req.languageCode) && x.DistrictID == distictID && x.SeasonID == req.seasonID
                )
                .Select(x => new Crops_Res
                {
                    cropID= x.CropID,
                    cropName = x.Crop.CropTranslations.Where(x=>x.CropID == x.CropID && x.LanguageCode == req.languageCode).Select(x=>x.Text).FirstOrDefault(),
                    filePath = x.Crop.CropTranslations.Where(x => x.CropID == x.CropID && x.LanguageCode == req.languageCode).Select(x => x.Image).FirstOrDefault(),
                    suggestion = x.Crop.ProductConsumptionMetrics.Select(x=> new ConsumptionMatrixDTO
                    {
                        name = x.Product.ProductTranslations.Where(x=>x.LanguageCode == req.languageCode).FirstOrDefault().Text,
                        qty = x.Usage.ToString("N2"),
                        uom = x.UOM
                    }).ToList()
                }).ToListAsync();
            return crops;


            //dummySuggestions = await _context.ProductConsumptionMetric.Where(x => x.CropID == req.)
        }
    }
}
