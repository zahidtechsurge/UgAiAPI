using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblSeason
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual List<tblCropTimings> CropTimings { get; set; } = null!;
        public virtual List<tblSeasonTranslation> SeasonTranslations { get; set; } = null!;
        public virtual List<tblPlan> plans { get; set; } = null;
        public virtual List<tblMonth> Months { get; set; } = new List<tblMonth>();
    }

}
