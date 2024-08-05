using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class NavigationMenuDTO
    {
        public string ModuleName { get; set; }
        public List<PageDTO> Pages { get; set; }
    }
    public class PageDTO
    {
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public string Controller { get; set; }
        public string ActionMethod { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
