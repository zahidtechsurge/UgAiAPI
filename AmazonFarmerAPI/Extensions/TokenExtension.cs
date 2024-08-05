using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AmazonFarmerAPI.Extensions
{

    public class JWTService
    {
        private readonly HttpContext _httpContext;

        public JWTService(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public static TokenResponse GenerateJwt(UserDTO user, string secret, string issuer, int expirationInDays, IEnumerable<Claim> pClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.userID),
                new Claim(ClaimTypes.GivenName, user.firstName is null ? "" : user.firstName),
                new Claim("userName", user.username is null ? "" : user.username),
                new Claim("languageCode", user.languageCode is null ? "EN" : user.languageCode),
                new Claim( "isOTPVerified", user.isOTPVerified ? "true" : "false"),
                new Claim( "designationID", user.designationID.ToString()), 
                new Claim(ClaimTypes.Email, user.email is null ? "" : user.email),
                new Claim("expOn", ((long)DateTime.UtcNow.AddDays(expirationInDays).ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString())
            };

            claims.AddRange(pClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiresIn = DateTime.UtcNow.AddDays(expirationInDays);

            var token = new JwtSecurityToken(
               issuer: issuer,
              audience: issuer,
              expires: expiresIn,
              signingCredentials: creds,
              claims: claims
              );

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = (long)expiresIn.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds
            };
        }

        public object GetUserIDByJWT()
        {
            // Get the JWT token from the request
            var token = _httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Parse the token and read claims
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            // Access a specific claim (value) from the payload
            var claimValue = tokenS.Claims.First(claim => claim.Type == "claim_key").Value;

            // Use the claimValue in your code
            // ...

            return "";
        }
    }

    public class JwtAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the controller or action has AllowAnonymousJwtAttribute
            var allowAnonymousJwtAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .FirstOrDefault();

            if (allowAnonymousJwtAttribute != null)
            {
                return; // Allow anonymous access
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Customize your error response
                var result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "Authentication failed. Please provide a valid JWT token.",
                    ContentType = "application/json; charset=utf-8"
                };

                context.Result = result;
            }

            if (Convert.ToInt32(context.HttpContext.User.FindFirst("expOn")?.Value) < Convert.ToInt32((long)DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds))
            {
                // Customize your error response
                var result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "Authentication failed. Login Timeout",
                    ContentType = "application/json; charset=utf-8"
                };

                context.Result = result;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAnonymousJwtAttribute : AllowAnonymousAttribute
    {
    }
}
