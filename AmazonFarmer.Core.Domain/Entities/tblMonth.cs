using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblMonth
    {
        [Key]
        public int ID { get; set; }
        public int SeasonID { get; set; }
        public string Name { get; set; }
        public int orderBy { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        [ForeignKey("SeasonID")]
        public virtual tblSeason Season { get; set; }
        public virtual List<tblMonthTranslation> MonthTranslations { get; set; } = null!;
    }

}
