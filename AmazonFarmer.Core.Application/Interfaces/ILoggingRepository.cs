using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface ILoggingRepository
    {
        WSDLLog AddLogEntry(WSDLLog logEntry);
        void UpdateLogEntry(WSDLLog logEntry);
    }
}
