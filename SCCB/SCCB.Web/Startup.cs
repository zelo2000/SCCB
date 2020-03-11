using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCCB.Core.Settings;
using SCCB.DAL;
using SCCB.Repos;
using SCCB.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SCCB.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            configurationBuilder.AddJsonFile("appsettings.json", false, true);

            Configuration = configurationBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var authSettingsSection = Configuration.GetSection("AuthSetting");
            services.Configure<AuthSetting>(authSettingsSection);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SCCBDbContext>(options => options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

            services.AddControllersWithViews();

            services.AddOptions();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCCB API", Version = "v1" });
            });

            services.AddAutoMapper(
                typeof(ServiceMapProfile).Assembly,
                typeof(WebApiMapProfile).Assembly);

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule<ReposDependencyModule>();
            builder.RegisterModule<ServiceDependencyModule>();

            //builder.RegisterType<TokenServiceMiddleware>().InstancePerDependency();
            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SCCB API v1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
