using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions;

internal class SurrealExpression : Expression
{
    public SurrealExpressionType SurrealNodeType { get; }
    public override Type Type { get; }

    internal SurrealExpression(SurrealExpressionType surrealNodeType)
    {
        Type = null!; // TODO : ?
        SurrealNodeType = surrealNodeType;
    }

    internal SurrealExpression(Type type, SurrealExpressionType surrealNodeType)
    {
        Type = type;
        SurrealNodeType = surrealNodeType;
    }
}
