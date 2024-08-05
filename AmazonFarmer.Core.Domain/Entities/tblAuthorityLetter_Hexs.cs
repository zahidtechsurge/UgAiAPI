using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblAuthorityLetter_Hexs
    {
        [Key]
        public int ID { get; set; }
        public string HexaNo { get; set; }
        public int Number { get; set; }
    }
}
