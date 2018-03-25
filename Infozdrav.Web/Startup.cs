using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data;
using Infozdrav.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace Infozdrav.Web
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
            services.AddMvc();
            services.AddAutoMapper();
            services.AddDbContext<AppDbContext>(o => o.UseMySQL(Configuration["ConnectionString"]));

            foreach (var dependecy in Assembly.GetEntryAssembly().GetAllTypesWithBase<IDependency>())
            {
                var interfaces = dependecy.GetInterfaces();
                //Debug.WriteLine(dependecy);

                if (interfaces.Contains(typeof(ISingletonDependency)))
                    services.AddSingleton(dependecy);
                else if (interfaces.Contains(typeof(IDependency)))
                    services.AddTransient(dependecy);
            }

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<DbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
