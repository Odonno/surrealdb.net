namespace SurrealDb.Net.Internals.Queryable;

internal interface ISurrealDbQueryProvider : IQueryProvider
{
    string FromTable { get; }
}
