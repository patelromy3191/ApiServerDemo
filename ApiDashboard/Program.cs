using ApiDashboard;
using ApiDashboard.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped<HttpClient>(s =>
{
    var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7065/") };
    // Additional configuration for HttpClient can be done here
    return httpClient;
});
builder.Services.AddScoped<IApiServices, ApiServices>();
builder.Services.AddScoped<IKeyInterceptorFactory, KeyInterceptorFactory>();
builder.Services.AddScoped<IDialogService, DialogService>();


await builder.Build().RunAsync();
