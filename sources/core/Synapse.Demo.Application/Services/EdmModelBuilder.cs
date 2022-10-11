namespace Synapse.Demo.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IEdmModelBuilder"/>
/// </summary>
public class EdmModelBuilder
    : IEdmModelBuilder
{

    /// <inheritdoc/>
    public virtual IEdmModel Build()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();

        builder.EntitySet<Integration.Models.Device>("Devices");

        return builder.GetEdmModel();
    }

}

