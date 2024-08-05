using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBAGradsAdmin.Data.Models
{
    public class RandomGenerator
    {
        private static Random _random = new Random();

        public static string GenerateRandomNo()
        {
            return _random.Next(1000, 9999).ToString("D4");
        }
    }
}
