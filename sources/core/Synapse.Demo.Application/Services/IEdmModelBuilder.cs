namespace Synapse.Demo.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to build <see cref="IEdmModel"/>s
/// </summary>
public interface IEdmModelBuilder
{

    /// <summary>
    /// Builds a new <see cref="IEdmModel"/>
    /// </summary>
    /// <returns>The application's <see cref="IEdmModel"/></returns>
    IEdmModel Build();

}

