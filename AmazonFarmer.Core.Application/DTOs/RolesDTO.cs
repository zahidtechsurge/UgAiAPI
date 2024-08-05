using System;
using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for permissions associated with a role.
    /// </summary>
    public class PermissionDTO
    {
        // The ID of the role
        public string RoleId { get; set; }

        // List of permissions for the role
        public IList<PermissionWiseClaimsDTO> Permissions { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) for permissions associated with a role.
    /// </summary>
    public class PermissionWiseClaimsDTO
    {
        // The ID of the module
        public int ModuleID { get; set; }

        // The name of the module
        public string ModuleName { get; set; }

        // The ID of the role
        public string RoleId { get; set; }

        // List of claims for the module
        public IList<RoleClaimDTO> ModuleClaims { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) for a role claim.
    /// </summary>
    public class RoleClaimDTO
    {
        // The type of claim
        public string Type { get; set; }

        // The value of the claim
        public string Value { get; set; }

        // The description of the claim
        public string Description { get; set; }

        // Indicates whether the claim is selected
        public bool Selected { get; set; } = false;
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for getting roles.
    /// </summary>
    public class getRoles
    {
        // Search query
        public string search { get; set; }

        // Number of records to skip
        public int skip { get; set; }

        // Number of records to take
        public int take { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for getting roles.
    /// </summary>
    public class getRoles_Resp
    {
        // The ID of the role
        public string roleID { get; set; }

        // The name of the role
        public string roleName { get; set; }

        // Indicates the status of the role
        public bool status { get; set; }
    }

    /// <summary>
    /// Request Data Transfer Object (DTO) for getting permissions.
    /// </summary>
    public class getPermission
    {
        // The ID of the role
        public string roleID { get; set; }
    }

    /// <summary>
    /// Response Data Transfer Object (DTO) for getting permissions.
    /// </summary>
    public class getPermission_Resp
    {
        // The ID of the role
        public string roleID { get; set; }

        // The name of the role
        public string roleName { get; set; }

        // List of pages and associated permissions for the role
        public List<pagesInRole> pages { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) for pages associated with a role.
    /// </summary>
    public class pagesInRole
    {
        // The ID of the page
        public int pageID { get; set; }

        // The name of the page
        public string pageName { get; set; }

        // List of permissions for the page
        public List<permissionInRole> permission { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) for permissions associated with a role on a page.
    /// </summary>
    public class permissionInRole
    {
        // The ID of the permission
        public string permissionID { get; set; }

        // The name of the permission
        public string permissionName { get; set; }
    }
}
