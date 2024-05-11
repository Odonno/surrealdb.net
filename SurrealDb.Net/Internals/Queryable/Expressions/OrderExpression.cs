using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions;

internal enum OrderType
{
    Ascending,
    Descending
}

internal class OrderExpression
{
    public OrderType OrderType { get; }
    public Expression Expression { get; }

    internal OrderExpression(OrderType orderType, Expression expression)
    {
        OrderType = orderType;
        Expression = expression;
    }
}
