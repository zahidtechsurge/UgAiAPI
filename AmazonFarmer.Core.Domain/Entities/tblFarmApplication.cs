using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblFarmApplication
    {
        [Key]
        public int ID { get; set; }
        public EApplicationType ApplicationTypeID { get; set; }
        public virtual ICollection<tblFarmChangeRequest> FarmChangeRequest { get; set; } = new List<tblFarmChangeRequest>();
        public virtual ICollection<tblfarm> Farm { get; set; } = new List<tblfarm>();
    }
}
