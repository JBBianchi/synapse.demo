namespace Synapse.Demo.Infrastructure.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the <see cref="IDemoApplicationBuilder"/>
/// </summary>
public static class IDemoApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the DI pipeline for the infrastructure
    /// </summary>
    /// <param name="demoBuilder">the <see cref="IDemoApplicationBuilder"/> to configure</param>
    /// <returns></returns>
    public static IDemoApplicationBuilder AddInfrastructure(this IDemoApplicationBuilder demoBuilder)
    {
        if (demoBuilder == null) throw DomainException.ArgumentNull(nameof(demoBuilder));
        demoBuilder.Services.AddDemoInfrastructure(demoBuilder.Options);
        return demoBuilder;
    }
}
