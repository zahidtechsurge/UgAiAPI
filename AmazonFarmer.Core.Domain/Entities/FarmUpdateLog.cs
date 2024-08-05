using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class FarmUpdateLog
    {
        [Key]
        public int Id { get; set; }
        public int FarmId { get; set; }
        public string UserId { get; set;}
        public EFarmStatus Status{ get; set;} 
        public DateTime UpdatedDate { get; set;} = DateTime.UtcNow; 

    }
}
