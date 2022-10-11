namespace Synapse.Demo.Api.Rest.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the <see cref="IDemoApplicationBuilder"/>
/// </summary>
public static class IDemoApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the DI pipeline for the RESTful API
    /// </summary>
    /// <param name="demoBuilder">the <see cref="IDemoApplicationBuilder"/> to configure</param>
    /// <returns></returns>
    public static IDemoApplicationBuilder AddRestApi(this IDemoApplicationBuilder demoBuilder)
    {
        if (demoBuilder == null) throw DomainException.ArgumentNull(nameof(demoBuilder));
        var searchBinder = new ODataSearchBinder();
        demoBuilder.Services.AddSingleton<ISearchBinder>(searchBinder);
        demoBuilder.Services.AddTransient<IODataQueryOptionsParser, ODataQueryOptionsParser>();
        demoBuilder.Services
                .AddControllers()
                .AddOData((options, provider) =>
                {
                    IEdmModelBuilder builder = provider.GetRequiredService<IEdmModelBuilder>();
                    options.AddRouteComponents("api/odata", builder.Build(), services => services.AddSingleton<ISearchBinder>(searchBinder))
                        .EnableQueryFeatures(50);
                    options.RouteOptions.EnableControllerNameCaseInsensitive = true;
                    options.RouteOptions.EnableActionNameCaseInsensitive = true;
                    options.RouteOptions.EnablePropertyNameCaseInsensitive = true;
                })
                .AddODataNewtonsoftJson()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ConfigureSerializerSettings();
                })
                .AddApplicationPart(typeof(IDemoApplicationBuilderExtensions).Assembly)
                ;
        demoBuilder.Services.AddSwaggerGen(builder =>
        {
            builder.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            builder.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Demo API",
                Version = "v1",
                Description = "The Open API documentation for the Demo App API",
                Contact = new()
                {
                    Name = "Neuroglia SRL",
                    Email = "info@neuroglia.io",
                    Url = new Uri("https://www.neuroglia.io")
                }
            });
            builder.IncludeXmlComments(typeof(IDemoApplicationBuilderExtensions).Assembly.Location.Replace(".dll", ".xml"));
            builder.IncludeXmlComments(typeof(Integration.Models.Device).Assembly.Location.Replace(".dll", ".xml"));
        });
        return demoBuilder;
    }
}
