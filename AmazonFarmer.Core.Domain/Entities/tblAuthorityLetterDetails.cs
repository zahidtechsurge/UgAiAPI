using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblAuthorityLetterDetails
    {
        [Key]
        public int RecID { get; set; }
        public int AuthorityLetterID { get; set; }
        public int ProductID { get; set; }
        public string TruckerNo { get; set; }
        public string BiltyNo { get; set; }
        //public int BiltyNo { get; set; }
        public int BagQuantity { get; set; }


        [ForeignKey("AuthorityLetterID")]
        public virtual TblAuthorityLetters AuthorityLetters { get; set; }

        [ForeignKey("ProductID")]
        public virtual TblProduct Products { get; set; }


    }
}
