using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IWeatherRepo
    {
        Task<tblWeatherIcon> getWeatherByWeatherType(long weatherType);
    }
}
