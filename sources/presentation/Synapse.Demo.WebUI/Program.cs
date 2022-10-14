var builder = WebAssemblyHostBuilder.CreateDefault(args);
var baseAddress = builder.HostEnvironment.BaseAddress;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddRestApiClient(http => http.BaseAddress = new Uri(baseAddress));
builder.Services.AddNewtonsoftJsonSerializer(options => options.ConfigureSerializerSettings());
builder.Services.AddSingleton(provider =>
    new HubConnectionBuilder()
        .WithUrl($"{baseAddress}api/ws")
        .WithAutomaticReconnect()
        .AddNewtonsoftJsonProtocol(options => {
            options.PayloadSerializerSettings = new JsonSerializerSettings().ConfigureSerializerSettings();
        })
        .Build()
);
builder.Services.AddFlux(flux => 
    flux.ScanMarkupTypeAssembly<App>()
);

builder.Services.AddSingleton<IKnobManager, KnobManager>();

await builder.Build().RunAsync();
