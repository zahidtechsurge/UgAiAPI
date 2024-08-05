using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonFarmer.Data.Models
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);
    }
}
