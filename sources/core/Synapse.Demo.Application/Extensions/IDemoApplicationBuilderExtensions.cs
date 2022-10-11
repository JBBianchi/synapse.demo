namespace Synapse.Demo.Application.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the <see cref="IDemoApplicationBuilder"/>
/// </summary>
public static class IDemoApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the DI pipeline for the application
    /// </summary>
    /// <param name="demoBuilder">the <see cref="IDemoApplicationBuilder"/> to configure</param>
    /// <returns>The configured <see cref="IDemoApplicationBuilder"/> so that additional calls can be chained.</returns>
    internal static IDemoApplicationBuilder AddApplication(this IDemoApplicationBuilder demoBuilder)
    {
        if (demoBuilder == null) throw DomainException.ArgumentNull(nameof(demoBuilder));
        JsonConvert.DefaultSettings = () =>
        {
            var settings = new JsonSerializerSettings();
            return settings.ConfigureSerializerSettings();
        };
        demoBuilder.Services.AddNewtonsoftJsonSerializer(options =>
        {
            options.ConfigureSerializerSettings();
        });
        demoBuilder.Services.AddTransient<IEdmModelBuilder, EdmModelBuilder>();
        demoBuilder.Services.AddTransient(provider => provider.GetRequiredService<IEdmModelBuilder>().Build());
        demoBuilder.Services.AddDemoApplicationMediator();
        demoBuilder.Services.AddDemoApplicationMapper();
        demoBuilder.Services.AddGenericQueryHandlers();
        return demoBuilder;
    }
}
