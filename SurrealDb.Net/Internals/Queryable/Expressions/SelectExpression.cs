using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions;

internal class SelectExpression : SurrealExpression
{
    public string Alias { get; }
    public bool AllFields { get; }
    public ReadOnlyCollection<ColumnDeclaration> Columns { get; }
    public Expression From { get; }
    public Expression? Where { get; }
    public ReadOnlyCollection<OrderExpression> OrderBy { get; }
    public Expression? Limit { get; }
    public Expression? Start { get; }

    internal SelectExpression(
        Type type,
        string alias,
        Expression from,
        Expression? where,
        IEnumerable<OrderExpression> orderBy,
        Expression? limit,
        Expression? start
    )
        : base(type, SurrealExpressionType.Select)
    {
        Alias = alias;
        AllFields = true;
        Columns = new List<ColumnDeclaration>().AsReadOnly(); // TODO : ?
        From = from;
        Where = where;
        OrderBy = orderBy.ToList().AsReadOnly();
        Limit = limit;
        Start = start;
    }

    internal SelectExpression(
        Type type,
        string alias,
        IEnumerable<ColumnDeclaration> columns,
        Expression from,
        Expression? where,
        IEnumerable<OrderExpression> orderBy,
        Expression? limit,
        Expression? start
    )
        : base(type, SurrealExpressionType.Select)
    {
        Alias = alias;
        AllFields = false;
        Columns =
            columns as ReadOnlyCollection<ColumnDeclaration>
            ?? new List<ColumnDeclaration>(columns).AsReadOnly();
        From = from;
        Where = where;
        OrderBy = orderBy.ToList().AsReadOnly();
        Limit = limit;
        Start = start;
    }
}
