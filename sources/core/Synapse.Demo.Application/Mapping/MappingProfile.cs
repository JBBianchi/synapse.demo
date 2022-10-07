using AutoMapper;

namespace Synapse.Demo.Application.Mapping;

/// <summary>
/// Represents the application mapping <see cref="Profile"/>
/// </summary>
public class MappingProfile
    : Profile
{

    /// <summary>
    /// Gets a <see cref="HashSet{T}"/> containing the types of all existing <see cref="IMappingConfiguration"/>s
    /// </summary>
    protected HashSet<Type> MappingConfigurationTypes { get; init; }

    /// <summary>
    /// Gets the types of the current assembly 
    /// </summary>
    protected Type[] AssemblyTypes { get; init; } = new List<Type>().ToArray();

    /// <summary>
    /// Gets the types of the Integration assembly
    /// </summary>
    protected Type[] IntegrationTypes { get; init; } = new List<Type>().ToArray();

    /// <summary>
    /// Initializes a new <see cref="MappingProfile"/>
    /// </summary>
    public MappingProfile()
    {
        this.AllowNullCollections = true;
        this.MappingConfigurationTypes = new HashSet<Type>();
        this.AssemblyTypes = this.GetType().Assembly.GetTypes();
        this.IntegrationTypes = typeof(Integration.Attributes.DataTransferObjectForAttribute).Assembly.GetTypes();
        this.AddConfiguredMappings();
        this.AddIntegrationMappings();
    }

    /// <summary>
    /// Configures the <see cref="MappingProfile"/> of classes marked with <see cref="IMappingConfiguration"/>
    /// </summary>
    protected void AddConfiguredMappings()
    {
        foreach (Type mappingConfigurationType in this.AssemblyTypes
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass && typeof(IMappingConfiguration).IsAssignableFrom(t)))
        {
            this.MappingConfigurationTypes.Add(mappingConfigurationType);
            this.ApplyConfiguration((IMappingConfiguration)Activator.CreateInstance(mappingConfigurationType, Array.Empty<object>())!);
        }
    }

    /// <summary>
    /// Configures the <see cref="MappingProfile"/> for integration classes marked with <see cref="DataTransferObjectForAttribute"/>
    /// </summary>
    protected void AddIntegrationMappings()
    {
        foreach (Type dtoType in this.IntegrationTypes
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass))
        {
            DataTransferObjectForAttribute? entityTypeAttribute = dtoType.GetCustomAttribute<DataTransferObjectForAttribute>();
            if (entityTypeAttribute != null && !this.MappingConfigurationTypes.Any(t => typeof(IMappingConfiguration<,>).MakeGenericType(entityTypeAttribute.Type, dtoType).IsAssignableFrom(t)))
                this.CreateMap(entityTypeAttribute.Type, dtoType);
        }
    }
}
