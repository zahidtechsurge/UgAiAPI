using System.ComponentModel.DataAnnotations;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblReasons
    {
        [Key]
        public int ID { get; set; }
        public string Reason { get; set; }
        public EReasonFor ReasonForID { get; set; }
        public EReasonOf ReasonOfID { get; set; }
        public EActivityStatus Status { get; set; }
        public virtual List<tblReasonTranslation> ReasonTranslation { get; set; } = null!;
        public virtual List<tblfarm> Farms { get; set; } = null!;
        public virtual List<tblNotification> Notifications { get; set; } = null!;
    }
}
