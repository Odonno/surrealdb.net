using System.Runtime.InteropServices;
using Dahomey.Cbor;
using Microsoft.IO;
using SurrealDb.Net.Exceptions;
using SurrealDb.Net.Internals;
using SurrealDb.Net.Internals.Cbor;
using SurrealDb.Net.Internals.Constants;
using SurrealDb.Net.Internals.Models;
using SurrealDb.Net.Internals.Models.LiveQuery;
using SurrealDb.Net.Internals.Resolvers;
using SurrealDb.Net.Models;
using SurrealDb.Net.Models.Auth;
using SurrealDb.Net.Models.LiveQuery;
using SurrealDb.Net.Models.Response;
using SystemTextJsonPatch;

namespace SurrealDb.Embedded.InMemory.Internals;

internal class SurrealDbInMemoryEngine : ISurrealDbInMemoryEngine
{
    // TODO : Single RecyclableMemoryStreamManager
    private static readonly RecyclableMemoryStreamManager _memoryStreamManager = new();

    private readonly SurrealDbProviderArgsResolver _surrealDbProviderArgsResolver;
    private SurrealDbClientParams? _parameters;

    public SurrealDbInMemoryEngine(SurrealDbProviderArgsResolver surrealDbProviderArgsResolver)
    {
        _surrealDbProviderArgsResolver = surrealDbProviderArgsResolver;
    }

    public void Initialize()
    {
        _parameters = _surrealDbProviderArgsResolver.GetClientParams(this);

        if (_parameters.Serialization?.ToLowerInvariant() == SerializationConstants.JSON)
        {
            // TODO : Add test
            throw new NotSupportedException(
                "The JSON serialization is not supported for the in-memory provider."
            );
        }
    }

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
        _surrealDbProviderArgsResolver.EvictClientParams(this);

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

    public async Task Use(string ns, string db, CancellationToken cancellationToken)
    {
        //string[] @params = [ns, db];

        var request = new SurrealDbEmbeddedRequest { Method = "use", Parameters = [ns, db] };
        await SendAsync(request, cancellationToken).ConfigureAwait(false);

        //await using var stream = _memoryStreamManager.GetStream();
        //await CborSerializer
        //    .SerializeAsync(request, stream, GetCborOptions(), cancellationToken)
        //    .ConfigureAwait(false);

        //bool canGetBuffer = stream.TryGetBuffer(out var bytes);
        //if (!canGetBuffer)
        //{
        //    throw new SurrealDbException("Failed to retrieve serialized buffer.");
        //}

        //var taskCompletionSource = new TaskCompletionSource<SurrealDbEmbeddedOkResponse>();

        //Action<ByteBuffer> success = (byteBuffer) => {
        //    // TODO
        //};
        //Action<ByteBuffer> fail = (byteBuffer) => {
        //    // TODO
        //};

        //var successHandle = GCHandle.Alloc(success);
        //var failureHandle = GCHandle.Alloc(fail);

        //unsafe
        //{
        //    var successAction = new SuccessAction()
        //    {
        //        handle = new RustGCHandle()
        //        {
        //            ptr = GCHandle.ToIntPtr(successHandle),
        //            drop_callback = &NativeBindings.DropGcHandle
        //        },
        //        callback = &NativeBindings.SuccessCallback,
        //    };

        //    var failureAction = new FailureAction()
        //    {
        //        handle = new RustGCHandle()
        //        {
        //            ptr = GCHandle.ToIntPtr(failureHandle),
        //            drop_callback = &NativeBindings.DropGcHandle
        //        },
        //        callback = &NativeBindings.FailureCallback,
        //    };

        //    fixed (byte* payload = bytes.Array)
        //    {
        //        NativeMethods.execute(payload, bytes.Count, successAction, failureAction);
        //    }
        //}

        //await taskCompletionSource.Task.ConfigureAwait(false);
    }

    public Task<string> Version(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private CborOptions GetCborOptions()
    {
        // TODO : Copy from updated HTTP/WS engine
        return SurrealDbCborOptions.GetCborSerializerOptions(_parameters!.NamingPolicy);
    }

    private async Task<SurrealDbEmbeddedOkResponse> SendAsync(
        SurrealDbEmbeddedRequest request,
        CancellationToken cancellationToken
    )
    {
        await using var stream = _memoryStreamManager.GetStream();
        await CborSerializer
            .SerializeAsync(request, stream, GetCborOptions(), cancellationToken)
            .ConfigureAwait(false);

        bool canGetBuffer = stream.TryGetBuffer(out var bytes);
        if (!canGetBuffer)
        {
            throw new SurrealDbException("Failed to retrieve serialized buffer.");
        }

        var taskCompletionSource = new TaskCompletionSource<SurrealDbEmbeddedOkResponse>();

        Action<ByteBuffer> success = (byteBuffer) =>
        {
            // TODO : Deserialize OK
            taskCompletionSource.SetResult(null!);
        };
        Action<ByteBuffer> fail = (byteBuffer) =>
        {
            // TODO : Deserialize KO
            taskCompletionSource.SetException(new SurrealDbException("KO"));
        };

        var successHandle = GCHandle.Alloc(success);
        var failureHandle = GCHandle.Alloc(fail);

        unsafe
        {
            var successAction = new SuccessAction()
            {
                handle = new RustGCHandle()
                {
                    ptr = GCHandle.ToIntPtr(successHandle),
                    drop_callback = &NativeBindings.DropGcHandle
                },
                callback = &NativeBindings.SuccessCallback,
            };

            var failureAction = new FailureAction()
            {
                handle = new RustGCHandle()
                {
                    ptr = GCHandle.ToIntPtr(failureHandle),
                    drop_callback = &NativeBindings.DropGcHandle
                },
                callback = &NativeBindings.FailureCallback,
            };

            fixed (byte* payload = bytes.Array)
            {
                NativeMethods.execute(payload, bytes.Count, successAction, failureAction);
            }
        }

        return await taskCompletionSource.Task.ConfigureAwait(false);
    }

    //private async Task<bool> TrySerializeRequest(
    //    object?[] parameters,
    //    CancellationToken cancellationToken
    //)
    //{
    //    await using var stream = _memoryStreamManager.GetStream();
    //    await CborSerializer
    //        .SerializeAsync(parameters, stream, GetCborOptions(), cancellationToken)
    //        .ConfigureAwait(false);

    //    return stream.TryGetBuffer(out var bytes);
    //}
}
