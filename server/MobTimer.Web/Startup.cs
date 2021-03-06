using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobTimer.Web.Domain;
using MobTimer.Web.Hubs;

namespace MobTimer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();
            services.AddTransient<IMob, Mob>();
            services.AddTransient<ITimer, Timer>();
            services.AddTransient<IMobMessenger, MobMessenger>();
            services.AddSingleton<IRoom, Room>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => { 
                endpoints.MapRazorPages();
                endpoints.MapHub<TimerHub>("/timer");
            });
        }
    }
}
