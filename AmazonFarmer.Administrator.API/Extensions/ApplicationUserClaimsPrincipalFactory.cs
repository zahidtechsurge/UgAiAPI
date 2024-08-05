using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Security.Claims;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<TblUser>
    {
        private readonly AmazonFarmerContext _context;
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<TblUser> userManager,
            AmazonFarmerContext context,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
            _context = context;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TblUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            List<TblFarmerRole> userRoles = await _context.FarmerRoles
                 .Include(ur => ur.Role)
                 .ThenInclude(r => r.RoleClaims)
                 .ThenInclude(rc => rc.Claim)
                 .ThenInclude(c => c.ClaimActions)
                 .Where(ur => ur.UserId == user.Id).ToListAsync();

            List<int> PageIds = new List<int>();

            foreach (var role in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
                foreach (var roleClaim in role.Role.RoleClaims)
                {
                    _context.Entry(roleClaim.Claim).Reference(b => b.TblPage).Load();
                    PageIds.Add(roleClaim.Claim.PageId);
                    foreach (var claimAction in roleClaim.Claim.ClaimActions)
                    {
                        Claim claim = new Claim("Permission", roleClaim.Claim.TblPage.Controller + "-" + claimAction.ClaimDescription);
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>("Name", claimAction.Claim.ClaimDescription);
                        KeyValuePair<string, string> kv2 = new KeyValuePair<string, string>("Permission", claimAction.ClaimDescription);

                        claim.Properties.Add(kv);
                        claim.Properties.Add(kv2);
                        identity.AddClaim(claim);
                    }
                }
            }
            List<TblNavigationModule> navigationModules = await _context.NavigationModules
                .Include(nm => nm.Pages.Where(p => p.IsActive == EActivityStatus.Active && p.ShowOnMenu == true && PageIds.Contains(p.PageID)).OrderBy(p => p.PageOrder))
                .Where(nm => nm.IsActive == EActivityStatus.Active && nm.ShowInMenu == true && nm.Pages.Where(p => p.IsActive == EActivityStatus.Active && p.ShowOnMenu == true && PageIds.Contains(p.PageID)).Count() > 0)
                .OrderBy(nm => nm.ModuleOrder).ToListAsync();


            List<NavigationMenuDTO> navigationMenus = new List<NavigationMenuDTO>();
            foreach (var navigationModule in navigationModules)
            {
                NavigationMenuDTO navigationMenu = new NavigationMenuDTO();
                navigationMenu.Pages = new List<PageDTO>();
                navigationMenu.ModuleName = navigationModule.ModuleName;
                foreach (var page in navigationModule.Pages)
                {
                    navigationMenu.Pages.Add(new PageDTO { PageName = page.PageName, PageUrl = page.PageUrl, ActionMethod = page.ActionMethod,Controller=page.Controller,ShowOnMenu= page.ShowOnMenu });
                }
                navigationMenus.Add(navigationMenu);
            }
            string jsonString = JsonSerializer.Serialize(navigationMenus);
            identity.AddClaim(new Claim("Modules", jsonString));
            return identity;
        }
    }
}
