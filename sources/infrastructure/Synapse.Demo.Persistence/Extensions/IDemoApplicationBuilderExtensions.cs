namespace Synapse.Demo.Persistence.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the <see cref="IDemoApplicationBuilder"/>
/// </summary>
public static class IDemoApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the DI pipeline for the persistence
    /// </summary>
    /// <param name="demoBuilder">the <see cref="IDemoApplicationBuilder"/> to configure</param>
    /// <returns></returns>
    public static IDemoApplicationBuilder AddPersistence(this IDemoApplicationBuilder demoBuilder)
    {
        if (demoBuilder == null) throw DomainException.ArgumentNull(nameof(demoBuilder));
        demoBuilder.Services.AddDemoPersistence();
        return demoBuilder;
    }
}
