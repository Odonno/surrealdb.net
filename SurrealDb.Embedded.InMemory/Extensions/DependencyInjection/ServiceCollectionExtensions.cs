using Microsoft.Extensions.DependencyInjection;
using SurrealDb.Embedded.InMemory.Internals;
using SurrealDb.Net.Internals;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to register SurrealDB services for in-memory provider.
/// Registers <see cref="ISurrealDbInMemoryEngine"/> as a factory instance (transient lifetime).
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryProvider(this IServiceCollection services)
    {
        services.AddTransient<ISurrealDbInMemoryEngine, SurrealDbInMemoryEngine>();

        return services;
    }
}
