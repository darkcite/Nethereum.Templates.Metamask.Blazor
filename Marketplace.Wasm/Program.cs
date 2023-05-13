using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.UI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nethereum.Metamask.Blazor;
using Nethereum.Metamask;
using Microsoft.AspNetCore.Components.Authorization;
using Nethereum.Blazor;
using Marketplace.Wasm.Services;
using BlazorBootstrap;

namespace Marketplace.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddAuthorizationCore();

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IMetamaskInterop, MetamaskBlazorInterop>();
            builder.Services.AddSingleton<MetamaskHostProvider>();

            builder.Services.AddBlazorBootstrap();

            //Add metamask as the selected ethereum host provider
            builder.Services.AddSingleton(services =>
            {
                var metamaskHostProvider = services.GetService<MetamaskHostProvider>();
                var selectedHostProvider = new SelectedEthereumHostProviderService();
                selectedHostProvider.SetSelectedEthereumHostProvider(metamaskHostProvider);
                return selectedHostProvider;
            });

            builder.Services.AddSingleton<EthereumClientService>();

            builder.Services.AddScoped<NFTService>();

            builder.Services.AddScoped<MetaMaskService>();

            builder.Services.AddSingleton<AuthenticationStateProvider, EthereumAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
