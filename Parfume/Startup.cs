using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Parfume.DAL;
using Parfume.Models;
using Parfume.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume
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
            services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseDefaultTypeSerializer()
            .UseMemoryStorage());
            services.AddHangfireServer();
            services.AddControllersWithViews();
            services.Configure<VapidKeys>(Configuration.GetSection("VapidKeys"));
            services.AddDbContext<ParfumeContext>(options =>
            {
                options.UseSqlServer(Configuration["AppSettings:ConnectionString:Default"]);
            });
            services.AddControllers();
            services.AddAuthentication(sharedOption =>
            {
                sharedOption.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IGeneralNotifactionService, GeneralNotifactionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICreatePdfService, CreatePdfService>();
            services.AddScoped<ISendNotification, SendNotification>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = System.TimeSpan.FromMinutes(233);
                });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            ISendNotification sendNotification)
        {
             
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
          
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = t =>
                {
                    t.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    t.Context.Response.Headers.Add("Expires", "-1");
                }
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                  pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            app.UseHangfireDashboard();
             
            //recurringJobManager.AddOrUpdate(
            //    "Run every day minute",
            //    ( )=> sendNotification.Print(),
            //     "0 * * * *"  
            //    );
        }
    }
}
