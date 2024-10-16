﻿using System.Security.Claims;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSpecificClaim(this ClaimsIdentity principal, string claimName)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == claimName).Value;
        }
    }
}
