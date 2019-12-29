using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Monito.Database.EFCore;
using Monito.Domain.Service;
using Monito.Domain.Service.Interface;
using Monito.Repository.EFCore;
using Monito.Repository.Interface;
using Monito.Web.Services;
using Monito.Web.Services.Interface;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Monito.Web
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
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // TODO: Move into a separated "Integration" project
            #region IOC Integration

                #region Database
                services.AddDbContext<DbContext, MonitoContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseMySql(Configuration.GetSection("ConnectionString").Value, mysqlOptions =>
                            mysqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))));
                #endregion

                #region Repositories
                services
                    .AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
                services
                    .AddScoped(typeof(IRepository<>), typeof(Repository<>));
                #endregion

                #region Domain services
                services
                    .AddScoped<IUserService, UserService>();
                services
                    .AddScoped<IRequestService, RequestService>();
                #endregion

                #region Application services
                services
                    .AddScoped<IHttpRequestService, HttpRequestService>();
                services
                    .AddScoped<IJobService, JobService>();
                #endregion
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions(){
                RequestPath = new PathString("/static"),
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"wwwroot/dist/static"))
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
