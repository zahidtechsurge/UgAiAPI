using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblwarehouse
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string WHCode { get; set; }
        public string Address { get; set; } 
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? InchargeID { get; set; }
        public int? DistrictID { get; set; }
        public string? SalePoint { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        [ForeignKey("InchargeID")]
        public virtual TblUser WarehouseIncharge { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        public virtual List<tblwarehouseTranslation> WarehouseTranslation { get; set; } = null!;
        public virtual List<TblAuthorityLetters> AuthorityLetters { get; set; } = null!;
        public virtual List<tblPlan> Plans { get; set; } = null!;
        public virtual List<TblOrders> Orders { get; set; } = null!;
        public virtual List<tblNotification> Notifications { get; set; } = null!;
    }
}
