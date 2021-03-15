using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.SessionStorage;
using Blazored.Modal;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Tewr.Blazor.FileReader;
using BP_OnlineDOD.Client.Repository;
using BP_OnlineDOD.Client.Helpers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BP_OnlineDOD.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient<HttpClientWithToken>(
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<HttpClientWithoutToken>(
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHttpRepo, HttpRepo>();
            services.AddFileReaderService(o => {
                o.UseWasmSharedBuffer = true;
                o.InitializeOnFirstCall = true;
            });

            services.AddBlazoredSessionStorage();
            services.AddBlazoredModal();
            services.AddBlazoredLocalStorage();

            services.AddOptions();

            services.AddApiAuthorization();

        }
    }
}
