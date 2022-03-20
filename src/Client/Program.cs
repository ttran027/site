using Blazored.LocalStorage;
using Business.Queries;
using Business.Queries.Crypto;
using Client;
using Client.Cache;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMediatR(typeof(Program), typeof(QuerySettings));
builder.Services.AddFluxor(options => 
    {
        options.ScanAssemblies(typeof(Program).Assembly);
        options.UseRouting();
    });

var querySettings = new QuerySettings()
{
    CryptoSettings = new()
    {
        BinanceAddress = "https://api.binance.com/api/v3/",
    }
};
builder.Services.AddSingleton<CryptoQuerySettings>(querySettings.CryptoSettings);
builder.Services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
{
    ResumeUrl = builder.HostEnvironment.BaseAddress + "resume.json",
});

builder.Services.AddBlazorUILibrary();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<ICryptoPriceCache, CryptoPriceCache>();
await builder.Build().RunAsync();