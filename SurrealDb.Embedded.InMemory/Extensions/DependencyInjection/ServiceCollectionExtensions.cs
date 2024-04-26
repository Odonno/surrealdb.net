using SurrealDb.Embedded.InMemory.Internals;
using SurrealDb.Net.Internals;
using SurrealDb.Net.Internals.Resolvers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to register SurrealDB services for in-memory provider.
/// Registers <see cref="ISurrealDbInMemoryEngine"/> as a factory instance (scoped lifetime).
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryProvider(this IServiceCollection services)
    {
        services.AddScoped<ISurrealDbInMemoryEngine>(serviceProvider =>
        {
            var resolver = serviceProvider.GetRequiredService<SurrealDbProviderArgsResolver>();
            var engine = new SurrealDbInMemoryEngine(resolver);

            engine.Initialize();

            return engine;
        });

        return services;
    }
}
