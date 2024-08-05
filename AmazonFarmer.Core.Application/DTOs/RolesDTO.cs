using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class PermissionDTO
    {
        public string RoleId { get; set; }
        public IList<PermissionWiseClaimsDTO> Permissions { get; set; }
    }
    public class PermissionWiseClaimsDTO
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string RoleId { get; set; }
        public IList<RoleClaimDTO> ModuleClaims { get; set; }
    }
    public class RoleClaimDTO
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; } = false;
    }

    public class getRoles
    {
        public string search { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
    }
    public class getRoles_Resp
    {
        public string roleID { get; set; }
        public string roleName { get; set; }
        public bool status { get; set; }
    }
    public class getPermission
    {
        public string roleID { get; set; }
    }
    public class getPermission_Resp
    {
        public string roleID { get; set; }
        public string roleName { get; set; }
        public List<pagesInRole> pages { get; set; }
    }
    public class pagesInRole
    {
        public int pageID { get; set; }
        public string pageName { get; set; }
        public List<permissionInRole> permission { get; set; }
    }
    public class permissionInRole
    {
        public string permissionID { get; set; }
        public string permissionName { get; set; }
    }
}
