using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Scheduled.Payment.DTOs
{
    public class FileExtractDTO
    {
        public string inputDirectory { get; set; }
        public string companyFileName { get; set; }
        public string extractedMISFileName { get; set; }
        public string archiveDirectory { get; set; }
    }
}
