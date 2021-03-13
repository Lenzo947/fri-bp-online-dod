using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BP_OnlineDOD.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Ganss.XSS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using BP_OnlineDOD.Server.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BP_OnlineDOD.Server
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddEnvironmentVariables();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var server = Environment.GetEnvironmentVariable("DB_ADDRESS") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "FRIUniza1990";
            var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "OnlineDOD_DB";

            var profanities = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "profanities.txt"));
            var allowedHTMLTags = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "allowedHTMLTags.txt"));

            var connectionString = $"server={server}; port={port}; database={database}; user={user}; password={password}; Persist Security Info=False; Connect Timeout=300";

            services.AddDbContext<OnlineDODContext>(opt => opt.UseMySql
                (connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<OnlineDODContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser, OnlineDODContext>()
                .AddProfileService<IdentityProfileService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(_ => new HtmlSanitizer(new HashSet<string>(allowedHTMLTags)));

            services.AddScoped<IOnlineDOD, SqlOnlineDOD>();

            services.AddSingleton<ProfanityFilter.Interfaces.IProfanityFilter>(provider =>
            {
                var filter = new ProfanityFilter.ProfanityFilter();
                filter.AddProfanity(profanities);
                return filter;
            });

            services.AddTransient<SampleData>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"files")),
                RequestPath = new PathString("/files")
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
