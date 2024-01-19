using DotNetLab12.Areas.Identity;
using DotNetLab12.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetLab12
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
            services.AddRazorPages();
           
            services.AddDbContextPool<ShopDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyDb")));

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = true;
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ShopDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DisableAdminRole", policy =>
                    policy.Requirements.Add(new NonAdminRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, NonAdminRequirementHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            MyIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        }
    }
}
