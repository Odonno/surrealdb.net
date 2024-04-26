using SurrealDb.Net.Internals.Models;

namespace SurrealDb.Net.Internals.Resolvers;

internal class SurrealDbProviderArgsResolver
{
    private readonly Dictionary<ISurrealDbEngine, SurrealDbClientParams> _engineParams = [];

    public SurrealDbClientParams GetClientParams(ISurrealDbEngine engine)
    {
        return _engineParams.GetValueOrDefault(engine)
            ?? throw new KeyNotFoundException("This engine is not registered");
    }

    public void SetClientParams(ISurrealDbEngine engine, SurrealDbClientParams parameters)
    {
        _engineParams.Add(engine, parameters);
    }

    public void EvictClientParams(ISurrealDbEngine engine)
    {
        _engineParams.Remove(engine);
    }
}
