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
using Monito.Persistence.Configuration.DependencyInjection;
using Monito.Web.Configuration.DependencyInjection;
using Monito.Web.Extensions;
using Monito.Web.Hubs;
using Monito.Domain.Configuration.DependencyInjection;
using Monito.Web.Configuration.Mapping;
using Monito.Application.Configuration.Mapping;
using Monito.Application.Model.Command;
using MediatR;
using AutoMapper;
using Monito.Application.Services;
using Monito.Application.Services.Command;
using System.Text.Json.Serialization;
using GraphQL.Types;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.DataLoader;
using Monito.Application.Services.Graph;

namespace Monito.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        private string _allowDomainCorsPolicy = "_allowDomainCorsPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(conf => conf.EnableDetailedErrors = true);
            services.AddControllersWithViews()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddHttpUtils();

            services.AddAutoMapper(
                typeof(PresentationApplicationMappingProfile),
                typeof(PersistenceApplicationMappingProfile),
                typeof(DomainPersistenceMappingProfile)
            );

            #region Application
            services.AddMediatR(typeof(SaveJobCommand), typeof(SaveJobCommandHandler));
            #endregion

            #region Persistence 
            services.AddDatabase(Configuration.GetSection("ConnectionString").Value);
            services.AddMonitoDbContext();
            services.AddGenericRepositories();
            #endregion

            #region Domain
            services.AddDomainServices();
            #endregion

            #region Presentation
            services.AddPresentationServices();
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

            #region GraphQL
            services.AddScoped<MonitoQuery>()
                .AddScoped<MonitoSchema>()
                .AddGraphQL(o => {})
                .AddGraphTypes(
                    typeof(MonitoSchema),
                    ServiceLifetime.Scoped
                )
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = Environment.IsDevelopment())
                .AddDataLoader();
            #endregion

            #endregion

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

            #region GraphQL
            app.UseGraphQL<MonitoSchema>();
            app.UseGraphiQLServer();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions() {});
            app.UseGraphQLAltair();
            #endregion

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