using AutoMapper;

namespace Synapse.Demo.WebUI.Mappging;

/// <summary>
/// Represents the application mapping <see cref="Profile"/>
/// </summary>
public class MappingProfile
    : Profile
{

    /// <summary>
    /// Gets a <see cref="HashSet{T}"/> containing the types of all existing <see cref="IMappingConfiguration"/>s
    /// </summary>
    protected HashSet<Type> KnownConfigurationTypes { get; init; }

    /// <summary>
    /// Gets the types of the <see cref="IMappingConfiguration"/>s
    /// </summary>
    protected IEnumerable<Type> MappingConfigurationTypes { get; init; }

    /// <summary>
    /// Initializes a new <see cref="MappingProfile"/>
    /// </summary>
    public MappingProfile()
    {
        this.AllowNullCollections = true;
        this.KnownConfigurationTypes = new HashSet<Type>();
        this.MappingConfigurationTypes = TypeCacheUtil.FindFilteredTypes("webui:mapping-configuration", t => !t.IsAbstract && !t.IsInterface && t.IsClass && typeof(IMappingConfiguration).IsAssignableFrom(t), this.GetType().Assembly);
        this.AddConfiguredMappings();
    }

    /// <summary>
    /// Configures the <see cref="MappingProfile"/> of classes marked with <see cref="IMappingConfiguration"/>
    /// </summary>
    protected void AddConfiguredMappings()
    {
        foreach (Type mappingConfigurationType in this.MappingConfigurationTypes)
        {
            this.ApplyConfiguration((IMappingConfiguration)Activator.CreateInstance(mappingConfigurationType, Array.Empty<object>())!);
            this.KnownConfigurationTypes.Add(mappingConfigurationType);
        }
    }
}
