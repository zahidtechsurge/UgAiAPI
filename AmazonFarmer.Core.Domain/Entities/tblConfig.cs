using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblConfig
    {
        [Key]
        public int Id { get; set; }

        public EConfigType Type { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public string Value { get; set; } 
        public EConfigStatus Status { get; set; }
    }
    public enum EConfigStatus
    {
        Active = 1,
        Deactive = 0
    }

    public enum EConfigType
    {
        AdvancePaymentPercent = 1,
        AdvancePaymentBufferTime = 2,
        OrderBufferTime = 3,
        OrderPaymentBufferTime= 4,
        PartialPayment = 5,
        FullPayment = 6
    }
}
