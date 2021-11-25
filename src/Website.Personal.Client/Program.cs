using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Website.Personal.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddHttpClient("coinbase", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.coinbase.com/v2/");
});

builder.Services.AddHttpClient("binance", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.binance.com/api/v3/");
});
builder.Services.AddMudServices();

await builder.Build().RunAsync();
