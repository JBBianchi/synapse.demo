﻿namespace Synapse.Demo.Api.WebSocket.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the <see cref="IDemoApplicationBuilder"/>
/// </summary>
public static class IDemoApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the DI pipeline for the WebSocket API
    /// </summary>
    /// <param name="demoBuilder">the <see cref="IDemoApplicationBuilder"/> to configure</param>
    /// <returns></returns>
    public static IDemoApplicationBuilder AddWebSocketApi(this IDemoApplicationBuilder demoBuilder)
    {
        if (demoBuilder == null) throw DomainException.ArgumentNull(nameof(demoBuilder));
        demoBuilder.Services.AddSignalR()
            .AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ConfigureSerializerSettings();
            });
        return demoBuilder;
    }
}