using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class CropDTO_req
    {
        public int seasonID { get; set; }
        public int farmID { get; set; }
    }
    public class GetCropDTO_Internal_req
    {
        public int seasonID { get; set; }
        public int farmID { get; set; }
        public string languageCode { get; set; }
    }
    public class Crops_Res
    {
        public int cropID { get; set; }
        public string cropName { get; set; }
        public string filePath { get; set; }
        public List<ConsumptionMatrixDTO> suggestion { get; set; }
    }
    public class PlanCrop_Req
    {
        public int cropID { get; set; }
        public double crop_acreage { get; set; }
        public List<addCropPlan_Req> products { get; set; }
        public List<Server_Req> services { get; set; }
    }
}
