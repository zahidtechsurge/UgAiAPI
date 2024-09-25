using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ICropRepo // Defining the interface for handling crops
    {
        IQueryable<tblCrop> GetCrops();
        Task<List<tblCropTranslation>> GetCropTranslationByCropID(int CropID);
        Task<List<tblCropTimings>> GetCropTimingsByCropID(int CropID);
        IQueryable<tblCropTimings> GetCropTimings();
        Task<tblCropTranslation?> GetCropTranslationByCropID(int CropID, string LanguageCode);
        Task<tblCropTimings?> GetCropTimingByID(int ID);
        Task<tblCropTimings?> GetCropTimingByID(int CropID, int SeasonID, int DistrictID, int fromDate, int toDate);
        void AddCropTranslation(tblCropTranslation ct);
        void UpdateCropTranslation(tblCropTranslation ct);
        void AddCropTiming(tblCropTimings ct);
        void AddCropTimings(List<tblCropTimings> ct);
        void UpdateCropTiming(tblCropTimings ct);
        Task<List<Crops_Res>> getCropsBySeasonAndDistrictID(GetCropDTO_Internal_req req); // Method signature for getting crops by season and district ID
        Task<tblCrop?> GetCropByID(int ID);
        Task<tblCrop?> GetCropByName(string Name);
        void AddCrop(tblCrop crop);
        void UpdateCrop(tblCrop crop);

        Task<List<tblCrop>> getCropsProductConsumptionMetrics( );
    }
}
