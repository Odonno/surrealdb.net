﻿using Dahomey.Cbor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SurrealDb.Embedded.Internals;
using SurrealDb.Net;

namespace SurrealDb.Embedded.RocksDb;

public class SurrealDbRocksDbClient : BaseSurrealDbClient
{
    internal const string BASE_ENDPOINT = "rocksdb://";

    /// <summary>
    /// Creates a new <see cref="SurrealDbRocksDbClient"/>.
    /// </summary>
    /// <param name="filePath">The path to the database file.</param>
    /// <param name="namingPolicy">The naming policy to use for serialization.</param>
    /// <exception cref="ArgumentException"></exception>
    public SurrealDbRocksDbClient(string filePath, string? namingPolicy = null)
        : this(new SurrealDbOptions($"{BASE_ENDPOINT}{filePath}", namingPolicy)) { }

    /// <summary>
    /// Creates a new <see cref="SurrealDbRocksDbClient"/> using a specific configuration.
    /// </summary>
    /// <param name="configuration">The configuration options for the SurrealDbClient.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public SurrealDbRocksDbClient(SurrealDbOptions configuration)
        : this(configuration, null) { }

    internal SurrealDbRocksDbClient(
        SurrealDbOptions parameters,
        Action<CborOptions>? configureCborOptions = null,
        ILoggerFactory? loggerFactory = null
    )
    {
        if (string.IsNullOrWhiteSpace(parameters.Endpoint))
        {
            throw new ArgumentException("Endpoint is required", nameof(parameters));
        }

        Uri = new Uri(parameters.Endpoint);
        NamingPolicy = parameters.NamingPolicy;

        var engine = new SurrealDbEmbeddedEngine();
        InitializeProviderEngine(engine, parameters, configureCborOptions, loggerFactory);

        _engine = engine;
    }
}
