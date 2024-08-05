using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblIntro
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblIntroLanguages> IntroLanguages { get; set; } = null!;
    }

}
