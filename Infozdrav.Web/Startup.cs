  using System.Linq;
using System.Reflection;
using AutoMapper;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data;
using Infozdrav.Web.Helpers;
using Infozdrav.Web.Models;
  using Infozdrav.Web.Settings;
  using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Infrastructure;
  using Microsoft.AspNetCore.Mvc.Routing;
  using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddAutoMapper(o => o.AddProfile(new MappingProfile()));
            services.AddDbContext<AppDbContext>(o => o.UseMySQL(Configuration["ConnectionString"]));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            foreach (var dependecy in Assembly.GetEntryAssembly().GetAllTypesWithBase<IDependency>())
            {
                var interfaces = dependecy.GetInterfaces();
                //Debug.WriteLine(dependecy);

                if (interfaces.Contains(typeof(ISingletonDependency)))
                    services.AddSingleton(dependecy);
                else if (interfaces.Contains(typeof(IDependency)))
                    services.AddTransient(dependecy);
            }

            // Settings
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));

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
