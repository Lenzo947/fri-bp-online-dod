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
using BP_OnlineDOD.Server.Logic;
using BP_OnlineDOD.Server.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

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

            var connectionString = $"server={server}; port={port}; database={database}; user={user}; password={password}; Persist Security Info=False; Connect Timeout=300";

            services.AddDbContextPool<OnlineDODContext>(opt => opt.UseMySql
                (connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(_ => new HtmlSanitizer(new HashSet<string> { "br", "a" }));

            services.AddScoped<IOnlineDOD, SqlOnlineDOD>();
            services.AddScoped<IAccountLogic, AccountLogic>();
            services.AddScoped<ProfanityFilter.Interfaces.IProfanityFilter, ProfanityFilter.ProfanityFilter>();

            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
                    ValidateIssuer = true,
                    ValidAudience = Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TokenSettings").GetValue<string>("Key"))),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });

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
