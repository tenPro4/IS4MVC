using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS4MVC.UI.Configuration
{
    public class AdminConfiguration
    {
        public string PageTitle { get; set; }
        public string HomePageLogoUri { get; set; }
        public string FaviconUri { get; set; }
        public string IdentityAdminBaseUrl { get; set; }
        public string AdministrationRole { get; set; }

        public string Theme { get; set; }

        public string CustomThemeCss { get; set; }
    }
}
