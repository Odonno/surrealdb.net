namespace SurrealDb.Net.Internals.ObjectPool;

internal interface IAsyncResettable
{
    Task<bool> TryResetAsync();
}
