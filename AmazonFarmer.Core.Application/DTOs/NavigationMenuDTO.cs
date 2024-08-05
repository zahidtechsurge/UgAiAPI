namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for navigation menu.
    /// </summary>
    public class NavigationMenuDTO
    {
        // The name of the module
        public string ModuleName { get; set; }

        // List of pages in the module
        public List<PageDTO> Pages { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) for page information.
    /// </summary>
    public class PageDTO
    {
        // The name of the page
        public string PageName { get; set; }

        // The URL of the page
        public string PageUrl { get; set; }

        // The controller associated with the page
        public string Controller { get; set; }

        // The action method associated with the page
        public string ActionMethod { get; set; }

        // Indicates whether the page should be shown on the menu
        public bool ShowOnMenu { get; set; }
    }
}
