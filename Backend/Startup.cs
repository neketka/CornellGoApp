using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using Backend.Hub;
using Backend.Admin;
using System.IO;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System;

namespace Backend
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
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));

            string conString = Environment.GetEnvironmentVariable("JDBC_DATABASE_URL") ?? 
                Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BackendModel.CornellGoDb>(o => 
                o.UseLazyLoadingProxies().UseNpgsql(conString, 
                e => e.UseNetTopologySuite()), ServiceLifetime.Scoped);
            services.AddSignalR();
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "client/build");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CornellGoHub>("/hub");
                endpoints.MapHub<AdminHub>("/adminhub");
            });

            app.UseSpa(spa =>
            {
<<<<<<< HEAD
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "client/build");
=======
                //spa.Options.SourcePath = Path.Join(env.ContentRootPath, "client/build");
>>>>>>> MoreProduction

                /*if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }*/
            });
        }
    }
}
