using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
            var logDB = "Data Source=localhost,1433;Initial Catalog=OnlineDOD_DB;User Id=SA; Password=FRIUniza1990;";
            var sinkOpts = new MSSqlServerSinkOptions();
            sinkOpts.TableName = "Logs";
            //sinkOpts.AutoCreateSqlTable = true;
            //var columnOpts = new ColumnOptions();
            //columnOpts.Store.Remove(StandardColumn.Properties);
            //columnOpts.Store.Add(StandardColumn.LogEvent);
            //columnOpts.LogEvent.DataLength = 2048;
            //columnOpts.PrimaryKey = options.TimeStamp;
            //columnOpts.TimeStamp.NonClusteredIndex = true;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
                .WriteTo.MSSqlServer(
                    connectionString: logDB,
                    sinkOptions: sinkOpts)
                .CreateLogger();

            //Log.Warning("Dolezity vypis");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
