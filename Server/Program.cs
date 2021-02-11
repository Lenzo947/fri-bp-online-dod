using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BP_OnlineDOD.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace BP_OnlineDOD.Server
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var server = Environment.GetEnvironmentVariable("DB_ADDRESS") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "SA";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "FRIUniza1990";
            var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "OnlineDOD_DB";

            var sinkOpts = new MSSqlServerSinkOptions();
            sinkOpts.TableName = "Logs";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.MSSqlServer(
                    connectionString: $"Data Source={server},{port};Initial Catalog={database};User Id={user}; Password={password};",
                    sinkOptions: sinkOpts)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();


            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<OnlineDODContext>();
                if (dbContext.Database.IsSqlServer())
                {
                    dbContext.Database.Migrate();
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
                }).UseSerilog();
    }
}
