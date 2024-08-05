using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IOrderRepo
    {
        Task CreateOrder(int planID, string createdBy, EOrderType orderType, int parentOrderID);
    }
}
