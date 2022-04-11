using Blazored.LocalStorage;
using Client;
using Client.Business;
using Client.Contract.Interfaces;
using Client.Contract.Stores;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMediatR(typeof(Program), typeof(CacheService));
builder.Services.AddFluxor(options => 
    {
        options.ScanAssemblies(typeof(Program).Assembly, typeof(SudokuState).Assembly);
        options.UseRouting();
    });

builder.Services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
{
    ResumeUrl = builder.HostEnvironment.BaseAddress + "resume.json",
});

builder.Services.AddBlazorUILibrary();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<ICacheService, CacheService>();
await builder.Build().RunAsync();