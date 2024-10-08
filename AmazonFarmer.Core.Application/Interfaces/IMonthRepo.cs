﻿using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IMonthRepo
    {
        Task<List<getMonths>> getMonthsByLanguageCode(string languageCode);
        Task<List<getMonths>> getMonthsByLanguageCodeAndSeasonID(string languageCode, int seasonID);
    }
}
