using Infrastructure.Application;
using Infrastructure.Application.Implementation;
using Infrastructure.Helper;
using Infrastructure.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CheckList.Web.UI
{
    public class Startup
    {
        string _connString = "";
        private readonly IConfigurationRoot configuration;
        public IHostingEnvironment Environment { get; }


        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile(env.ContentRootPath + "/appsettings.json").Build();
            _connString = configuration.GetConnectionString("DefaultConnection");
            Environment = env;
            AppSettings.ContentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            using (var uow = new UnitOfWork(_connString))
            {
                uow.DbContext.InitDb();
            }

            services.AddScoped<UnitOfWork>(provider =>
            {
                return new UnitOfWork(_connString);
            });

            services.AddSingleton<IConfiguration>(configuration);
            services.AddMvc();
            RegisterInfraDependencies(services);

            services.AddCors();



            // configure jwt authentication
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSpaStaticFiles();

            app.UseCors(policy =>
            {
                policy.WithOrigins("*").AllowAnyHeader();
                policy.AllowAnyHeader();
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowCredentials();

            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static void RegisterInfraDependencies(IServiceCollection services)
        {
            services.AddTransient<AuthenticationService>();
            services.AddTransient<EncryptionService>();
            services.AddTransient<EventService>();

        }
    }
  
}
