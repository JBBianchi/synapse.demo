﻿using AutoMapper;

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
    protected Type[] DomainTypes { get; init; } = new List<Type>().ToArray();

    /// <summary>
    /// Initializes a new <see cref="MappingProfile"/>
    /// </summary>
    public MappingProfile()
    {
        this.AllowNullCollections = true;
        this.MappingConfigurationTypes = new HashSet<Type>();
        this.AssemblyTypes = this.GetType().Assembly.GetTypes();
        this.DomainTypes = typeof(Domain.Models.Device).Assembly.GetTypes();
        this.AddConfiguredMappings();
        this.AddCommandsMappings();
        this.AddDomainMappings();
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
    /// Configures the <see cref="MappingProfile"/> for integration commands to application commands marked with <see cref="DataTransferObjectTypeAttribute"/>
    /// </summary>
    protected void AddCommandsMappings()
    {
        foreach (Type applicationType in this.AssemblyTypes
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass))
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
        foreach (Type domainType in this.DomainTypes
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass))
        {
            DataTransferObjectTypeAttribute? integrationTypeAttribute = domainType.GetCustomAttribute<DataTransferObjectTypeAttribute>();
            if (integrationTypeAttribute?.Type == null) continue;
            this.CreateMapIfNoneExists(domainType, integrationTypeAttribute.Type);
        }
    }

    protected void CreateMapIfNoneExists(Type sourceType, Type destinationType)
    {
        if (!this.MappingConfigurationTypes.Any(t => typeof(IMappingConfiguration<,>).MakeGenericType(sourceType, destinationType).IsAssignableFrom(t)))
        {
            this.CreateMap(sourceType, destinationType);
        }
    }
}
