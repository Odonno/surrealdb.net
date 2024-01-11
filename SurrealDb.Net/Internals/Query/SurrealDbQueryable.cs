using System.Collections;
using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Query;

internal class SurrealDbQueryable<T> : IOrderedQueryable<T>, IAsyncEnumerable<T>
{
    //private readonly WeakReference<ISurrealDbEngine> _surrealDbEngine;
    //private readonly string _fromTable;

    public Type ElementType => typeof(T);
    public Expression Expression { get; private set; }
    public IQueryProvider Provider { get; private set; }

    public SurrealDbQueryable(IQueryProvider provider, Expression expression)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public SurrealDbQueryable(IQueryProvider provider)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Expression = Expression.Constant(this);
    }

    //public SurrealDbQueryable(ISurrealDbEngine surrealDbEngine, string _table)
    //{
    //    _surrealDbEngine = new WeakReference<ISurrealDbEngine>(surrealDbEngine);
    //    _fromTable = _table;
    //}

    public IEnumerator<T> GetEnumerator()
    {
        return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        //return _surrealDbEngine.TryGetTarget(out var engine)
        //    ? engine.SelectAll<T>(_fromTable, _cancellationToken).Result.GetEnumerator()
        //    : Enumerable.Empty<T>().GetEnumerator();
        //return _defaultSelector(_fromTable, _cancellationToken).Result.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        //return _surrealDbEngine.TryGetTarget(out var engine)
        //    ? engine.SelectAll<T>(_fromTable, _cancellationToken).Result.GetEnumerator()
        //    : Enumerable.Empty<T>().GetEnumerator();
        //return _defaultSelector(_fromTable, _cancellationToken).Result.GetEnumerator();
    }

    public async IAsyncEnumerator<T> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        if (Provider is IAsyncQueryProvider asyncProvider)
        {
            foreach (
                var item in await asyncProvider
                    .ExecuteAsync<IEnumerable<T>>(Expression, cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                yield return item;
            }
            yield break;
        }

        throw new InvalidOperationException(
            "The inner provider of IQueryable does not handle async"
        );
    }
}
