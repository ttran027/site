using Blazored.LocalStorage;
using Client;
using Client.CryptoPrices;
using Client.Sudoku;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMediatR(typeof(Program), typeof(CryptoQuerySettings));
builder.Services.AddFluxor(options => 
    {
        options.ScanAssemblies(typeof(Program).Assembly, typeof(CryptoPriceQuery).Assembly, typeof(SudokuCache).Assembly);
        options.UseRouting();
    });

builder.Services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
{
    ResumeUrl = builder.HostEnvironment.BaseAddress + "resume.json",
});

builder.Services.AddBlazorUILibrary();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<ICryptoPriceCache, CryptoPriceCache>();
builder.Services.AddTransient<ISudokuCache, SudokuCache>();
await builder.Build().RunAsync();