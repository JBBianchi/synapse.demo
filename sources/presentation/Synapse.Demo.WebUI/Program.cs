using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Synapse.Demo.WebUI;
using Synapse.Demo.Common.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var baseAddress = builder.HostEnvironment.BaseAddress;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(provider =>
    new HubConnectionBuilder()
        .WithUrl($"{baseAddress}api/ws")
        .WithAutomaticReconnect()
        .AddNewtonsoftJsonProtocol(options => {
            options.PayloadSerializerSettings = new JsonSerializerSettings().ConfigureSerializerSettings();
        })
        .Build()
);

await builder.Build().RunAsync();
