namespace SurrealDb.Net.Internals.Ws;

internal sealed class SurrealWsTaskCompletionSource : TaskCompletionSource<SurrealDbWsOkResponse>
{
    public SurrealDbWsRequestPriority Priority { get; }

    public SurrealWsTaskCompletionSource(SurrealDbWsRequestPriority priority)
        : base(TaskCreationOptions.RunContinuationsAsynchronously)
    {
        Priority = priority;
    }
}
