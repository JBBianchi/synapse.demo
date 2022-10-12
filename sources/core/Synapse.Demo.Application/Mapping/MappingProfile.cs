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
    protected HashSet<Type> KnownConfigurationTypes { get; init; }

    /// <summary>
    /// Gets the types of the <see cref="IMappingConfiguration"/>s
    /// </summary>
    protected IEnumerable<Type> MappingConfigurationTypes { get; init; }

    /// <summary>
    /// Gets the types of the application assembly marked with <see cref="DataTransferObjectTypeAttribute"/>
    /// </summary>
    protected IEnumerable<Type> CommandsDtoTypes { get; init; }

    /// <summary>
    /// Gets the types of the domain assembly marked with <see cref="DataTransferObjectTypeAttribute"/>
    /// </summary>
    protected IEnumerable<Type> DomainDtoTypes { get; init; }

    /// <summary>
    /// Initializes a new <see cref="MappingProfile"/>
    /// </summary>
    public MappingProfile()
    {
        this.AllowNullCollections = true;
        this.KnownConfigurationTypes = new HashSet<Type>();
        this.MappingConfigurationTypes = TypeCacheUtil.FindFilteredTypes("application:mapping-configuration", t => !t.IsAbstract && !t.IsInterface && t.IsClass && typeof(IMappingConfiguration).IsAssignableFrom(t), this.GetType().Assembly);
        this.CommandsDtoTypes = TypeCacheUtil.FindFilteredTypes("application:commands", t => !t.IsAbstract && !t.IsInterface && t.IsClass && t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _), this.GetType().Assembly);
        this.DomainDtoTypes = TypeCacheUtil.FindFilteredTypes("domain:aggregates-and-events", t => !t.IsAbstract && !t.IsInterface && t.IsClass && t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _), typeof(Domain.Models.Device).Assembly);
        this.AddConfiguredMappings();
        this.AddCommandsMappings();
        this.AddDomainMappings();
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

    /// <summary>
    /// Configures the <see cref="MappingProfile"/> for integration commands to application commands marked with <see cref="DataTransferObjectTypeAttribute"/>
    /// </summary>
    protected void AddCommandsMappings()
    {
        foreach (Type applicationType in this.CommandsDtoTypes)
        {
            DataTransferObjectTypeAttribute? integrationTypeAttribute = applicationType.GetCustomAttribute<DataTransferObjectTypeAttribute>();
            if (integrationTypeAttribute?.Type == null) continue;
            this.CreateMapIfNoneExists(integrationTypeAttribute.Type, applicationType);
        }
    }

    /// <summary>
    /// Configures the <see cref="MappingProfile"/> for domain models/events marked with <see cref="DataTransferObjectTypeAttribute"/> to integration models/events
    /// </summary>
    protected void AddDomainMappings()
    {
        foreach (Type domainType in this.DomainDtoTypes)
        {
            DataTransferObjectTypeAttribute? integrationTypeAttribute = domainType.GetCustomAttribute<DataTransferObjectTypeAttribute>();
            if (integrationTypeAttribute?.Type == null) continue;
            this.CreateMapIfNoneExists(domainType, integrationTypeAttribute.Type);
        }
    }

    protected void CreateMapIfNoneExists(Type sourceType, Type destinationType)
    {
        var mappingConfigurationType = typeof(IMappingConfiguration<,>).MakeGenericType(sourceType, destinationType);
        if (!this.KnownConfigurationTypes.Any(t => mappingConfigurationType.IsAssignableFrom(t)))
        {
            this.CreateMap(sourceType, destinationType);
            this.KnownConfigurationTypes.Add(mappingConfigurationType);
        }
    }
}
