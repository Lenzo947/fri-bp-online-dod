using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BP_OnlineDOD.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BP_OnlineDOD.Server
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            //var server = Environment.GetEnvironmentVariable("DB_ADDRESS") ?? "localhost";
            //var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            //var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "root";
            //var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "FRIUniza1990";
            //var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "OnlineDOD_DB";

            
            //var connectionString = $"server = {server}; uid = {user}; pwd = {password}; database = {database};";

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
            //    .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
            //    .WriteTo.MySQL(connectionString, storeTimestampInUtc: true, batchSize: 1)
            //    .CreateLogger();

            //Log.Warning("Dolezity vypis");

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<OnlineDODContext>();
                if (dbContext.Database.IsMySql())
                {
                    dbContext.Database.Migrate();
                }

                var dataSeeder = services.GetService<SampleData>();

                var loginEnv = Environment.GetEnvironmentVariable("WEB_LOGIN");
                var passwordEnv = Environment.GetEnvironmentVariable("WEB_PASSWORD");

                if (loginEnv != null && passwordEnv != null)
                {
                    await dataSeeder.SeedAdminUser(login: loginEnv, password: passwordEnv);
                }
                else
                {
                    await dataSeeder.SeedAdminUser();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }); // .UseSerilog()
    }
}
