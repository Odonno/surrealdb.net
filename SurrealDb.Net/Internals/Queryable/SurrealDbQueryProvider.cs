using System.Collections.Immutable;
using System.Linq.Expressions;
using SurrealDb.Net.Internals.Queryable.Expressions;
using SurrealDb.Net.Internals.Queryable.Visitors;

namespace SurrealDb.Net.Internals.Queryable;

// TODO : remove generic T?

internal class SurrealDbQueryProvider<T> : ISurrealDbQueryProvider, IAsyncQueryProvider
{
    private readonly WeakReference<ISurrealDbEngine> _surrealDbEngine;

    public string FromTable { get; }

    public SurrealDbQueryProvider(ISurrealDbEngine surrealDbEngine, string _table)
    {
        _surrealDbEngine = new WeakReference<ISurrealDbEngine>(surrealDbEngine);
        FromTable = _table;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        throw new NotSupportedException(
            $"Non-generic method '{nameof(CreateQuery)}' is not supported. Please use the generic version."
        );

        // TODO : Find a way to avoid Reflection
        // ------------------------------------------------
        //var elementType = expression.Type.GetGenericArguments().First();
        //return (IQueryable)
        //    Activator.CreateInstance(
        //        typeof(SurrealDbQueryable<>).MakeGenericType(elementType),
        //        this,
        //        expression
        //    )!;
        // ------------------------------------------------

        //Type elementType = TypeSystem.GetElementType(expression.Type);
        //try
        //{
        //    return
        //       (IQueryable)Activator.CreateInstance(typeof(Queryable<>).
        //              MakeGenericType(elementType), new object[] { this, expression });
        //}
        //catch (TargetInvocationException e)
        //{
        //    throw e.InnerException;
        //}
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        //return new Queryable<TElement>(this, expression);
        return new SurrealDbQueryable<TElement>(this, expression);
    }

    public object Execute(Expression expression)
    {
        //return _surrealDbEngine.TryGetTarget(out var engine)
        //    ? engine.SelectAll<T>(_fromTable, default).Result.GetEnumerator()
        //    : Enumerable.Empty<T>().GetEnumerator();

        //if (_surrealDbEngine.TryGetTarget(out var engine))
        //{
        //    return engine.SelectAll<T>(_fromTable, default).Result.GetEnumerator();
        //}

        //throw new Exception("SurrealDB instance has been disposed.");

        throw new NotImplementedException();
    }

    public TResult Execute<TResult>(Expression expression)
    {
        if (_surrealDbEngine.TryGetTarget(out var engine))
        {
            if (
                expression.NodeType == ExpressionType.Constant
                && expression.Type == typeof(SurrealDbQueryable<T>)
            )
            {
                return (TResult)engine.SelectAll<T>(FromTable, default).Result.GetEnumerator();
            }

            //var query = "SELECT * FROM " + _fromTable;
            //var surrealExpression = new SurrealExpressionVisitor().Visit(expression);
            //string query = new QueryGeneratorExpressionVisitor().Translate(expression, _fromTable);

            string query = Translate(expression, FromTable);
            var result = engine
                .RawQuery(query, ImmutableDictionary<string, object?>.Empty, default)
                .Result;

            return result.GetValue<TResult>(0)!;
        }

        throw new Exception("SurrealDB instance has been disposed.");

        // TODO : check if TResult is IEnumerable
        //return _surrealDbEngine.TryGetTarget(out var engine)
        //    ? (TResult)engine.SelectAll<T>(_fromTable, default).Result.GetEnumerator()
        //    : (TResult)Enumerable.Empty<T>().GetEnumerator();
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Expression expression,
        CancellationToken cancellationToken
    )
    {
        if (_surrealDbEngine.TryGetTarget(out var engine))
        {
            if (
                expression.NodeType == ExpressionType.Constant
                && expression.Type == typeof(SurrealDbQueryable<T>)
            )
            {
                var result = await engine
                    .SelectAll<T>(FromTable, cancellationToken)
                    .ConfigureAwait(false);
                return (TResult)result.GetEnumerator();
            }

            //var query = "SELECT * FROM " + _fromTable;
            //var surrealExpression = new SurrealExpressionVisitor().Visit(expression);
            //expression = Evaluator.PartialEval(expression);
            //var projection = (ProjectionExpression)
            //    new QueryBinderExpressionVisitor().Bind(expression);
            //string query = new QueryGeneratorExpressionVisitor().Translate(
            //    projection.Source,
            //    _fromTable
            //);
            string query = Translate(expression, FromTable);

            {
                var result = await engine
                    .RawQuery(query, ImmutableDictionary<string, object?>.Empty, cancellationToken)
                    .ConfigureAwait(false);

                return result.GetValue<TResult>(0)!;
            }
        }

        //if (_surrealDbEngine.TryGetTarget(out var engine))
        //{
        //    if (
        //        expression.NodeType == ExpressionType.Constant
        //        && expression.Type == typeof(SurrealDbQueryable<T>)
        //    )
        //    {
        //        return (TResult).Result.GetEnumerator();
        //    }

        //    var query = "SELECT * FROM " + _fromTable;
        //    var result = engine
        //        .Query(query, ImmutableDictionary<string, object>.Empty, default)
        //        .Result;

        //    return result.GetValue<TResult>(0)!;
        //}

        throw new Exception("SurrealDB instance has been disposed.");
    }

    internal static string Translate(Expression expression, string fromTable)
    {
        //var evaluatedExpression = Evaluator.PartialEval(expression);
        //var selectExpression = (SelectExpression)
        //    new QueryBinderExpressionVisitor().Bind(evaluatedExpression);

        var selectExpression = (SelectExpression)
            new QueryBinderExpressionVisitor().Bind(expression);

        return new QueryGeneratorExpressionVisitor().Translate(selectExpression, fromTable);
    }
}
