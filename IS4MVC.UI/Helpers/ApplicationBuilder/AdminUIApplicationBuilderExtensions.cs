using IS4MVC.UI.Configuration;
using IS4MVC.UI.Configuration.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS4MVC.UI.Helpers.ApplicationBuilder
{
    public static class AdminUIApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIS4AdminUI(this IApplicationBuilder app)
        {
            app.UseRoutingDependentMiddleware(app.ApplicationServices.GetRequiredService<TestingConfiguration>());

            return app;
        }

        public static IEndpointConventionBuilder MapAdminUI(this IEndpointRouteBuilder endpoint, string patternPrefix = "/")
        {
            return endpoint.MapAreaControllerRoute(CommonConsts.AdminUIArea, CommonConsts.AdminUIArea, patternPrefix + "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
