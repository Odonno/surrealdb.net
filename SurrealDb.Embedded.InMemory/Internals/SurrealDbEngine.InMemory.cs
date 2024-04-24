using SurrealDb.Net.Internals;
using SurrealDb.Net.Internals.Models.LiveQuery;
using SurrealDb.Net.Models;
using SurrealDb.Net.Models.Auth;
using SurrealDb.Net.Models.LiveQuery;
using SurrealDb.Net.Models.Response;
using SystemTextJsonPatch;

namespace SurrealDb.Embedded.InMemory.Internals;

internal class SurrealDbInMemoryEngine : ISurrealDbInMemoryEngine
{
    public Task Authenticate(Jwt jwt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Configure(string? ns, string? db, string? username, string? password)
    {
        throw new NotImplementedException();
    }

    public void Configure(string? ns, string? db, string? token = null)
    {
        throw new NotImplementedException();
    }

    public Task Connect(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T> Create<T>(T data, CancellationToken cancellationToken)
        where T : Record
    {
        throw new NotImplementedException();
    }

    public Task<T> Create<T>(string table, T? data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string table, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Thing thing, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Health(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T> Info<T>(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Invalidate(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Kill(
        Guid queryUuid,
        SurrealDbLiveQueryClosureReason reason,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public SurrealDbLiveQuery<T> ListenLive<T>(Guid queryUuid)
    {
        throw new NotImplementedException();
    }

    public Task<SurrealDbLiveQuery<T>> LiveQuery<T>(
        FormattableString query,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<SurrealDbLiveQuery<T>> LiveRawQuery<T>(
        string query,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<SurrealDbLiveQuery<T>> LiveTable<T>(
        string table,
        bool diff,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<TOutput> Merge<TMerge, TOutput>(TMerge data, CancellationToken cancellationToken)
        where TMerge : Record
    {
        throw new NotImplementedException();
    }

    public Task<T> Merge<T>(
        Thing thing,
        Dictionary<string, object> data,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TOutput>> MergeAll<TMerge, TOutput>(
        string table,
        TMerge data,
        CancellationToken cancellationToken
    )
        where TMerge : class
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> MergeAll<T>(
        string table,
        Dictionary<string, object> data,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<T> Patch<T>(
        Thing thing,
        JsonPatchDocument<T> patches,
        CancellationToken cancellationToken
    )
        where T : class
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> PatchAll<T>(
        string table,
        JsonPatchDocument<T> patches,
        CancellationToken cancellationToken
    )
        where T : class
    {
        throw new NotImplementedException();
    }

    public Task<SurrealDbResponse> Query(
        FormattableString query,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<SurrealDbResponse> RawQuery(
        string query,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> Select<T>(string table, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T?> Select<T>(Thing thing, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Set(string key, object value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SignIn(RootAuth root, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Jwt> SignIn(NamespaceAuth nsAuth, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Jwt> SignIn(DatabaseAuth dbAuth, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Jwt> SignIn<T>(T scopeAuth, CancellationToken cancellationToken)
        where T : ScopeAuth
    {
        throw new NotImplementedException();
    }

    public Task<Jwt> SignUp<T>(T scopeAuth, CancellationToken cancellationToken)
        where T : ScopeAuth
    {
        throw new NotImplementedException();
    }

    public SurrealDbLiveQueryChannel SubscribeToLiveQuery(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Unset(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> UpdateAll<T>(
        string table,
        T data,
        CancellationToken cancellationToken
    )
        where T : class
    {
        throw new NotImplementedException();
    }

    public Task<T> Upsert<T>(T data, CancellationToken cancellationToken)
        where T : Record
    {
        throw new NotImplementedException();
    }

    public Task Use(string ns, string db, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> Version(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
