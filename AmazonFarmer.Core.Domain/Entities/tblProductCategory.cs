using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblProductCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public int ProductID { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;

        //[ForeignKey("ProductID")]
        //public virtual TblProduct Product { get; set; }
        public virtual List<tblProductCategoryTranslation> ProductCategoryTranslation { get; set; } = null!;
        public virtual List<TblProduct> Products { get; set; } = null!;
    }
}
