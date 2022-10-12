using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Synapse.Demo.WebUI;

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
            options.PayloadSerializerSettings = new()
            {
                ContractResolver = new NonPublicSetterContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = false,
                        OverrideSpecifiedNames = false,
                        ProcessExtensionDataNames = false
                    }
                },
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };
            options.PayloadSerializerSettings.Converters.Add(new AbstractClassConverterFactory());
        })
        .Build()
);

await builder.Build().RunAsync();
