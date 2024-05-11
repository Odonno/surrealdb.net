using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions.Surreal;

internal abstract class SurrealExpression : Expression
{
    public override Type Type { get; }

    protected SurrealExpression()
    {
        Type = GetType();
    }
}
