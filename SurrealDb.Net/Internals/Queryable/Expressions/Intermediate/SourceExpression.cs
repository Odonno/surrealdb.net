using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions.Intermediate;

internal abstract class SourceExpression : IntermediateExpression;

internal sealed class TableSourceExpression : SourceExpression
{
    public TableExpression Table { get; }

    public TableSourceExpression(TableExpression table)
    {
        Table = table;
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        visitor.Visit(Table);
        return this;
    }
}

internal sealed class SelectSourceExpression : SourceExpression
{
    public SelectExpression Select { get; }

    public SelectSourceExpression(SelectExpression select)
    {
        Select = select;
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        visitor.Visit(Select);
        return this;
    }
}
