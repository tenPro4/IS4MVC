using IS4MVC.UI.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class IS4MVCSettings
    {
        public AdminConfiguration Admin { get; set; } = new AdminConfiguration();
        public TestingConfiguration Testing { get; set; } = new TestingConfiguration();
        public HttpConfiguration Http { get; set; } = new HttpConfiguration();
        public SecurityConfiguration Security { get; set; } = new SecurityConfiguration();
        public void BindConfiguration(IConfiguration configuration)
        {
            configuration.GetSection(nameof(AdminConfiguration)).Bind(Admin);
            configuration.GetSection(nameof(TestingConfiguration)).Bind(Testing);
            configuration.GetSection(nameof(SecurityConfiguration)).Bind(Security);
            configuration.GetSection(nameof(HttpConfiguration)).Bind(Http);
        }
    }
}
