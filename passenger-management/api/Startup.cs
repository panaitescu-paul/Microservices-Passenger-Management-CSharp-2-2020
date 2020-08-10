using api.Models;
using api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<PassengersDatabaseSettings>(
                Configuration.GetSection(nameof(PassengersDatabaseSettings)));
            services.AddSingleton<IPassengersDatabaseSettings>(provider =>
                provider.GetRequiredService<IOptions<PassengersDatabaseSettings>>().Value);

            services.Configure<KafkaConfig>(
                Configuration.GetSection(nameof(KafkaConfig)));
            services.AddSingleton<IKafkaConfig>(provider => provider.GetRequiredService<IOptions<KafkaConfig>>().Value);

            services.AddSingleton<PassengerService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "api/{controller=Passenger}/{id?}");
            });
        }
    }
}