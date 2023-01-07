using Blazored.LocalStorage;
using Client;
using Client.Contract.Interfaces;
using Client.Services;
using Client.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var baseAddress = builder.HostEnvironment.BaseAddress;
builder.Services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
{
    ResumeUrl = baseAddress + "resume.json",
    SoccerUrl = baseAddress + "soccer.json"
});

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddTransient<ISoccerService, SoccerService>();
await builder.Build().RunAsync();