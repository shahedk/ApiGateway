using System.Collections.Generic;
using System.Globalization;
using ApiGateway.Client;
using ApiGateway.Core;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using ApiGateway.Data.EFCore;
using ApiGateway.Data.EFCore.DataAccess;
using ApiGateway.InternalClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace ApiGateway.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            var db = Configuration.GetValue<string>("Database").ToLower();
           
            services.AddDbContext<ApiGatewayContext>(o =>
            {
                o.EnableSensitiveDataLogging();
                if (db == "mysql")
                {
                    o.UseMySql(Configuration.GetConnectionString("DefaultConnection"));    
                }
                else if (db == "mssql")
                {
                    o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));    
                }
                else
                {
                    o.UseSqlite(new SqliteConnection(Configuration.GetConnectionString("DefaultConnection")));    
                }
                
                
            });

            services.AddMvc();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<HttpClient, HttpClient>();
            
            services.AddTransient<IAppEnvironment, AppEnvironment>();
            services.AddTransient<IClientLoginService, InternalClientLoginService>();
            services.AddTransient<IApiRequestHelper, ApiRequestHelper>();
            services.AddTransient<KeySecretValidator, KeySecretValidator>();
            services.AddTransient<IApiKeyValidator, ApiKeyValidator>();
            
            
            // Core services
            services.AddTransient<IAccessRuleManager, AccessRuleManager>();
            services.AddTransient<IApiManager, ApiManager>();
            services.AddTransient<IKeyManager, KeyManager>();
            services.AddTransient<IRoleManager, RoleManager>();
            services.AddTransient<IServiceManager, ServiceManager>();
            
            // Data
            services.AddTransient<IKeyData, KeyData>();
            services.AddTransient<IServiceData, ServiceData>();
            services.AddTransient<IRoleData, RoleData>();
            services.AddTransient<IApiData, ApiData>();
            services.AddTransient<IAccessRuleData, AccessRuleData>();
            
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.UseMiddleware(typeof(InternalClientApiKeyValidationMiddleware));
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute("SystemApi", "sys/{controller}/{action}");
                routes.MapRoute("AppServices", "api/{*url}", new { controller = "AppService", action = "Spa" });
            });

        }
    }
}
