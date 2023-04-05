using IS4MVC.Helpers;
using IS4MVC.UI.Helpers;
using IS4MVC.UI.Helpers.ApplicationBuilder;
using Microsoft.Extensions.Options;

namespace IS4MVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIS4UI(ConfigureUIOptions);
            services.AddAdminUIRazorRuntimeCompilation(HostingEnvironment);
            services.AddEmailSenders(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();
            app.UseIS4AdminUI();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapAdminUI();
            });
        }

        public virtual void ConfigureUIOptions(IS4MVCSettings options)
        {
            // Applies configuration from appsettings.
            options.BindConfiguration(Configuration);
            if (HostingEnvironment.IsDevelopment())
            {
                options.Security.UseDeveloperExceptionPage = true;
            }
            else
            {
                options.Security.UseHsts = true;
            }

            options.Testing.IsStaging = true;
        }
    }
}
