using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Query;

internal interface IAsyncQueryProvider : IQueryProvider
{
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
}
