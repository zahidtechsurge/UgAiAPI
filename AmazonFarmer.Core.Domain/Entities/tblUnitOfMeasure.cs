using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblUnitOfMeasure
    {
        [Key]
        public int ID { get; set; }
        public string UOM { get; set; }
        public decimal? UnitofMeasureReporting { get; set; } = decimal.Zero;
        public decimal? UnitOfMeasureSales { get; set; } = decimal.Zero;
        public EActivityStatus Status { get; set; }
        public virtual List<TblProduct> Products { get; set; } = null!;
        public virtual List<tblUnitOfMeasureTranslation> UnitOfMeasureTranslation { get; set; } = null!;
    }
}
