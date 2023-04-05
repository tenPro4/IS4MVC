using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.HttpOverrides;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using IS4MVC.UI.Configuration;
using IS4MVC.UI.ExceptionHandling;
using IS4MVC.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using IS4MVC.UI.Configuration.Email;

namespace IS4MVC.UI.Helpers
{
    public static class StartupHelpers
    {
        /// <summary>
        /// A helper to inject common middleware into the application pipeline, without having to invoke Use*() methods.
        /// </summary>
        internal class StartupFilter : IStartupFilter
        {
            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                return builder =>
                {
                    // Adds our required middlewares to the beginning of the app pipeline.
                    // This does not include the middleware that is required to go between UseRouting and UseEndpoints.
                    builder.UseCommonMiddleware(
                        builder.ApplicationServices.GetRequiredService<SecurityConfiguration>(),
                        builder.ApplicationServices.GetRequiredService<HttpConfiguration>());

                    next(builder);
                };
            }
        }

        public static void UseCommonMiddleware(this IApplicationBuilder app, SecurityConfiguration securityConfiguration, HttpConfiguration httpConfiguration)
        {
            app.UseCookiePolicy();

            if (securityConfiguration.UseDeveloperExceptionPage)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (securityConfiguration.UseHsts)
            {
                app.UseHsts();
            }

            app.UsePathBase(httpConfiguration.BasePath);

            // Add custom security headers
            app.UseSecurityHeaders(securityConfiguration.CspTrustedDomains);

            app.UseStaticFiles();
        }

        public static void UseSecurityHeaders(this IApplicationBuilder app, List<string> cspTrustedDomains)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(options => options.NoReferrer());

            // CSP Configuration to be able to use external resources
            if (cspTrustedDomains != null && cspTrustedDomains.Any())
            {
                app.UseCsp(csp =>
                {
                    var imagesCustomSources = new List<string>();
                    imagesCustomSources.AddRange(cspTrustedDomains);
                    imagesCustomSources.Add("data:");

                    csp.ImageSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = imagesCustomSources;
                        options.Enabled = true;
                    });
                    csp.FontSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                    csp.ScriptSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                        options.UnsafeEvalSrc = true;
                    });
                    csp.StyleSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                    });
                    csp.DefaultSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                });
            }
        }

        public static void AddMvcExceptionFilters(this IServiceCollection services)
        {
            //Exception handling
            services.AddScoped<ControllerExceptionFilterAttribute>();
        }

        public static void RegisterDbContextsStaging(this IServiceCollection services)
        {
            var databaseName = Guid.NewGuid().ToString();

            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(databaseName));
        }

        public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            var sendGridConfiguration = configuration.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

            if (sendGridConfiguration != null && !string.IsNullOrWhiteSpace(sendGridConfiguration.ApiKey))
            {
                services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridConfiguration.ApiKey));
                services.AddSingleton(sendGridConfiguration);
                services.AddTransient<IEmailSender, SendGridEmailSender>();
            }
            else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
            {
                services.AddSingleton(smtpConfiguration);
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddSingleton<IEmailSender, LogEmailSender>();
            }
        }

        public static void UseRoutingDependentMiddleware(this IApplicationBuilder app, TestingConfiguration testingConfiguration)
        {
            //app.UseAuthentication();
            //if (testingConfiguration.IsStaging)
            //{
            //    app.UseMiddleware<AuthenticatedTestRequestMiddleware>();
            //}

            //app.UseAuthorization();
        }

    }
}
