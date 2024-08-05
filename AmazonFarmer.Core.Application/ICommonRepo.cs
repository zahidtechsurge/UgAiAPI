using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application
{
    public interface ICommonRepo
    {
        Task<string> GetConfigurationValueByConfigType(EConfigType type);
        Task<List<tblConfig>> GetConfigurationValueByConfigType(List<EConfigType> types);
    }
}
