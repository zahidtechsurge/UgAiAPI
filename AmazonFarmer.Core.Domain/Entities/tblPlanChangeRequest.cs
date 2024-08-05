using System.ComponentModel.DataAnnotations;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblPlanChangeRequest
    {
        [Key]
        public int ID { get; set; }
        public int PlanID { get; set; }
        public string UserID { get; set; }
        public int FarmID { get; set; }
        public int SeasonID { get; set; }
        public int Reason { get; set; }
        public int Status { get; set; }
    }
}
