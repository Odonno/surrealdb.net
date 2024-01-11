using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Query;

internal interface IQueryContext
{
    //object Execute(Expression expression, bool isEnumerable);
    TResult Execute<TResult>(Expression expression);
    TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
}
