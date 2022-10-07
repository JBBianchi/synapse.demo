global using CloudNative.CloudEvents;
global using FluentValidation;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Neuroglia;
global using Neuroglia.Data;
global using Neuroglia.Eventing;
global using Neuroglia.Mediation;
global using Neuroglia.Mapping;
global using System.Reflection;

global using Synapse.Demo.Application.Behaviors;
global using Synapse.Demo.Domain.Events.Devices;
global using Synapse.Demo.Integration.Attributes;
global using Synapse.Demo.Integration.Commands.Devices;
global using Synapse.Demo.Integration.Events.Devices;
global using Synapse.Demo.Integration.Models;

// TODO: improve constructors by checking nullability of arguments
// TODO: cloud events ingestion ?