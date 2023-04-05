using IS4MVC.UI.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IS4MVC.UI.Helpers.StartupHelpers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UIServiceCollectionExtensions
    {
        public static IServiceCollection AddIS4UI(this IServiceCollection services, Action<IS4MVCSettings> optionsAction)
        {
            var options = new IS4MVCSettings();
            optionsAction(options);

            services.AddSingleton(options.Admin);

            services.RegisterDbContextsStaging();

            if (options.Security.UseHsts)
            {
                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(365);

                    options.Security.HstsConfigureAction?.Invoke(opt);
                });
            }

            services.AddMvcExceptionFilters();

            services.AddSingleton(options.Testing);
            services.AddSingleton(options.Security);
            services.AddSingleton(options.Http);
            services.AddTransient<IStartupFilter, StartupFilter>();

            return services;
        }
    }
}
