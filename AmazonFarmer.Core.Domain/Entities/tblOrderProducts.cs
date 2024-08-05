using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblOrderProducts
    {
        [Key]
        public int OrderProductID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal Amount { get; set; }
        public int QTY { get; set; }
        [ForeignKey("ProductID")]
        public virtual TblProduct Product { get; set; }
        [ForeignKey("OrderID")]
        public virtual TblOrders Order { get; set; }
    }
}
