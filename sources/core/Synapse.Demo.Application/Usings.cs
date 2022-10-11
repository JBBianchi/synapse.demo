global using CloudNative.CloudEvents;
global using FluentValidation;
global using Microsoft.AspNetCore.OData.Query;
global using Microsoft.AspNetCore.OData.Query.Expressions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.OData.Edm;
global using Microsoft.OData.ModelBuilder;
global using Microsoft.OData.UriParser;
global using Neuroglia;
global using Neuroglia.Data;
global using Neuroglia.Eventing;
global using Neuroglia.Mediation;
global using Neuroglia.Mapping;
global using Neuroglia.Serialization;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using System.Linq.Expressions;
global using System.Reflection;

global using Synapse.Demo.Application.Behaviors;
global using Synapse.Demo.Application.Commands;
global using Synapse.Demo.Application.Configuration;
global using Synapse.Demo.Application.Extensions.DependencyInjection;
global using Synapse.Demo.Application.Queries;
global using Synapse.Demo.Application.Services;
global using Synapse.Demo.Domain.Events.Devices;
global using Synapse.Demo.Integration.Attributes;
global using Synapse.Demo.Integration.Events.Devices;
global using Synapse.Demo.Integration.Models;

// TODO: improve constructors by checking nullability of arguments
// TODO: cloud events ingestion ?