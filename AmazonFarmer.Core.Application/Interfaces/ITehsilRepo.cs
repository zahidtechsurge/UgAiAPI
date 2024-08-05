using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface ITehsilRepo
    {
        Task<List<TehsilDTO>> getTehsils(getTehsil_Req req);
    }
}
