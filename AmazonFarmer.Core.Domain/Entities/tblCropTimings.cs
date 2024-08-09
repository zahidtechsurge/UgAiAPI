using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblCropTimings
    {
        [Key]
        public int ID { get; set; }
        public int CropID { get; set; }
        public int SeasonID { get; set; }
        public int DistrictID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        //public EActivityStatus Status { get; set; }

        [ForeignKey("CropID")]
        public virtual tblCrop Crop { get; set; }
        [ForeignKey("SeasonID")]
        public virtual tblSeason Season { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
    }

}
