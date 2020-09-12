using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleCrm.SqlDbServices;

namespace SimpleCrm.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IGreeter, ConfigurationGreeter>();
            services.AddScoped<ICustomerData, SqlCustomerData>();

            services.AddDbContext<SimpleCrmDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SimpleCrmConnection"));
            });
            services.AddDbContext<CrmIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SimpleCrmConnection"));
            });

            services.AddIdentity<CrmUser, IdentityRole>()
                .AddEntityFrameworkStores<CrmIdentityDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Oops!")
                });
            }

            app.UseStaticFiles();
            app.UseWelcomePage(new WelcomePageOptions
            {
                Path = "/welcome"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default_route",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(ctx => ctx.Response.WriteAsync("Not found"));
        }
    }
}
