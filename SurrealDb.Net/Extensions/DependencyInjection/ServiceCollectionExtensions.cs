﻿using Dahomey.Cbor;
using Microsoft.Extensions.Logging;
using SurrealDb.Net;
using SurrealDb.Net.Internals.Helpers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to register SurrealDB services.
/// Registers <see cref="ISurrealDbClient"/> as a singleton instance.
/// Registers <see cref="IHttpClientFactory"/> for HTTP requests.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers SurrealDB services from a ConnectionString.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="connectionString">Connection string to a SurrealDB instance.</param>
    /// <param name="lifetime">Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Singleton"/>.</param>
    /// <param name="configureCborOptions">An optional action to configure <see cref="CborOptions"/>.</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static SurrealDbBuilder AddSurreal(
        this IServiceCollection services,
        string connectionString,
        ServiceLifetime lifetime = ServiceLifetime.Singleton,
        Action<CborOptions>? configureCborOptions = null
    )
    {
        return AddSurreal<ISurrealDbClient>(
            services,
            connectionString,
            lifetime,
            configureCborOptions
        );
    }

    /// <summary>
    /// Registers SurrealDB services from a ConnectionString.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ISurrealDbClient"/> to register.</typeparam>
    /// <param name="services">Service collection.</param>
    /// <param name="connectionString">Connection string to a SurrealDB instance.</param>
    /// <param name="lifetime">Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Singleton"/>.</param>
    /// <param name="configureCborOptions">An optional action to configure <see cref="CborOptions"/>.</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static SurrealDbBuilder AddSurreal<T>(
        this IServiceCollection services,
        string connectionString,
        ServiceLifetime lifetime = ServiceLifetime.Singleton,
        Action<CborOptions>? configureCborOptions = null
    )
        where T : ISurrealDbClient
    {
        var configuration = SurrealDbOptions
            .Create()
            .FromConnectionString(connectionString)
            .Build();

        return AddSurreal<T>(services, configuration, lifetime, configureCborOptions);
    }

    /// <summary>
    /// Registers SurrealDB services with the specified configuration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configureOptions">A delegate that is used to configure a <see cref="SurrealDbOptionsBuilder"/>.</param>
    /// <param name="lifetime">Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Singleton"/>.</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static SurrealDbBuilder AddSurreal(
        this IServiceCollection services,
        Action<SurrealDbOptionsBuilder> configureOptions,
        ServiceLifetime lifetime = ServiceLifetime.Singleton
    )
    {
        var options = SurrealDbOptions.Create();
        configureOptions(options);
        return AddSurreal<ISurrealDbClient>(services, options.Build(), lifetime);
    }

    /// <summary>
    /// Registers SurrealDB services with the specified configuration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration options.</param>
    /// <param name="lifetime">Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Singleton"/>.</param>
    /// <param name="configureCborOptions">An optional action to configure <see cref="CborOptions"/>.</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static SurrealDbBuilder AddSurreal(
        this IServiceCollection services,
        SurrealDbOptions configuration,
        ServiceLifetime lifetime = ServiceLifetime.Singleton,
        Action<CborOptions>? configureCborOptions = null
    )
    {
        return AddSurreal<ISurrealDbClient>(
            services,
            configuration,
            lifetime,
            configureCborOptions
        );
    }

    /// <summary>
    /// Registers SurrealDB services with the specified configuration.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ISurrealDbClient"/> to register.</typeparam>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration options.</param>
    /// <param name="lifetime">Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Singleton"/>.</param>
    /// <param name="configureCborOptions">An optional action to configure <see cref="CborOptions"/>.</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static SurrealDbBuilder AddSurreal<T>(
        this IServiceCollection services,
        SurrealDbOptions configuration,
        ServiceLifetime lifetime = ServiceLifetime.Singleton,
        Action<CborOptions>? configureCborOptions = null
    )
        where T : ISurrealDbClient
    {
        if (configuration.Endpoint is null)
            throw new ArgumentNullException(nameof(configuration), "The endpoint is required.");

        RegisterHttpClient(services, configuration.Endpoint);

        var classClientType = typeof(SurrealDbClient);
        var interfaceClientType = typeof(ISurrealDbClient);
        var type = typeof(T);

        bool isBaseType = type == classClientType || type == interfaceClientType;

        if (isBaseType)
        {
            RegisterSurrealDbClient<ISurrealDbClient>(
                services,
                configuration,
                lifetime,
                configureCborOptions
            );
            RegisterSurrealDbClient<SurrealDbClient>(
                services,
                configuration,
                lifetime,
                configureCborOptions
            );
        }
        else
        {
            RegisterSurrealDbClient<T>(services, configuration, lifetime, configureCborOptions);
        }

        return new SurrealDbBuilder(services);
    }

    private static void RegisterHttpClient(IServiceCollection services, string endpoint)
    {
        var uri = new Uri(endpoint);
        string httpClientName = HttpClientHelper.GetHttpClientName(uri);

        services.AddHttpClient(
            httpClientName,
            client =>
            {
                client.BaseAddress = uri;
            }
        );
    }

    private static void RegisterSurrealDbClient<T>(
        IServiceCollection services,
        SurrealDbOptions configuration,
        ServiceLifetime lifetime,
        Action<CborOptions>? configureCborOptions = null
    )
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(
                    typeof(T),
                    serviceProvider =>
                    {
                        return new SurrealDbClient(
                            configuration,
                            serviceProvider,
                            serviceProvider.GetRequiredService<IHttpClientFactory>(),
                            configureCborOptions,
                            serviceProvider.GetService<ILoggerFactory>()
                        );
                    }
                );
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(
                    typeof(T),
                    serviceProvider =>
                    {
                        return new SurrealDbClient(
                            configuration,
                            serviceProvider,
                            serviceProvider.GetRequiredService<IHttpClientFactory>(),
                            configureCborOptions,
                            serviceProvider.GetService<ILoggerFactory>()
                        );
                    }
                );
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(
                    typeof(T),
                    serviceProvider =>
                    {
                        return new SurrealDbClient(
                            configuration,
                            serviceProvider,
                            serviceProvider.GetRequiredService<IHttpClientFactory>(),
                            configureCborOptions,
                            serviceProvider.GetService<ILoggerFactory>()
                        );
                    }
                );
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(lifetime),
                    lifetime,
                    "Invalid service lifetime."
                );
        }
    }
}
