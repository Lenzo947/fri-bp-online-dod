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
using BP_OnlineDOD.Client.Services;
using Tewr.Blazor.FileReader;
using BP_OnlineDOD.Client.HttpRepository;

namespace BP_OnlineDOD.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<TokenAuthenticationStateProvider, TokenAuthenticationStateProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
            {
                return provider.GetRequiredService<TokenAuthenticationStateProvider>();
            });

            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IHttpRepo, HttpRepo>();
            builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);

            await builder.Build().RunAsync();
        }
    }
}
