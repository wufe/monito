using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Monito.Web.Extensions;
using Monito.Web.Hubs;
using Monito.Web.Integration;

namespace Monito.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string _allowDomainCorsPolicy = "_allowDomainCorsPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(conf => conf.EnableDetailedErrors = true);
            services.AddControllersWithViews();
            services.AddHttpUtils();
            services.AddDatabase(Configuration.GetSection("ConnectionString").Value);
            services.AddRepositories();
            services.AddDomainServices();
            services.AddPresentationServices();
            services.AddAutomapperConfigurations();
            services.AddCors(options => {
                options.AddPolicy(_allowDomainCorsPolicy, builder =>
                    builder.WithOrigins("http://localhost:8008", "https://monito.bembi.dev")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Index");
            }

            var cachePeriod = env.IsDevelopment() ? "0" : "604800";

            app.UseStaticFiles(new StaticFileOptions(){
                RequestPath = new PathString("/static"),
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"wwwroot/dist/static")),
                OnPrepareResponse = ctx => {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(_allowDomainCorsPolicy);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<JobHub>("/jobhub");
            });
        }
    }
}
