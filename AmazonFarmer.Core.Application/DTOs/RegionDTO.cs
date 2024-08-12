using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class AddRegionRequest
    {
        public string regionName { get; set; }
        public string regionCode { get; set; }
    }
    public class UpdateRegionRequest : AddRegionRequest
    {
        public int regionID { get; set; }
        /// <summary>
        /// DeActive = 0,
        /// Active = 1,
        /// </summary>
        public int status { get; set; }
    }

}
