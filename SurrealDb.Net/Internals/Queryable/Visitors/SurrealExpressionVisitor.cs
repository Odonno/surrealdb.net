using System.Collections.ObjectModel;
using System.Linq.Expressions;
using SurrealDb.Net.Internals.Queryable.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Visitors;

internal abstract class SurrealExpressionVisitor : ExpressionVisitor
{
    public override Expression? Visit(Expression? node)
    {
        if (node is null)
        {
            return null;
        }

        var surrealExpression = node as SurrealExpression;
        if (surrealExpression is not null)
        {
            return surrealExpression.SurrealNodeType switch
            {
                SurrealExpressionType.Table => VisitTable((TableExpression)node),
                SurrealExpressionType.Column => VisitColumn((ColumnExpression)node),
                SurrealExpressionType.Select => VisitSelect((SelectExpression)node),
                SurrealExpressionType.Projection => VisitProjection((ProjectionExpression)node),
                _ => base.Visit(node),
            };
        }

        return base.Visit(node);
    }

    protected virtual Expression VisitTable(TableExpression table)
    {
        return table;
    }

    protected virtual Expression VisitColumn(ColumnExpression column)
    {
        return column;
    }

    protected virtual Expression VisitSelect(SelectExpression node)
    {
        var from = VisitSource(node.From);
        var where = Visit(node.Where);
        var columns = VisitColumnDeclarations(node.Columns);
        var start = Visit(node.Start);
        var limit = Visit(node.Limit);

        if (
            from != node.From
            || where != node.Where
            || columns != node.Columns
            || start != node.Start
            || limit != node.Limit
        )
        {
            return new SelectExpression(node.Type, node.Alias, columns, from!, where, start, limit);
        }

        return node;
    }

    protected virtual Expression? VisitSource(Expression node)
    {
        return Visit(node);
    }

    protected virtual Expression VisitProjection(ProjectionExpression node)
    {
        var source = Visit(node.Source) as SelectExpression;
        var projector = Visit(node.Projector);

        if (source != node.Source || projector != node.Projector)
        {
            return new ProjectionExpression(source!, projector!);
        }

        return node;
    }

    protected ReadOnlyCollection<ColumnDeclaration> VisitColumnDeclarations(
        ReadOnlyCollection<ColumnDeclaration> columns
    )
    {
        List<ColumnDeclaration>? alternate = null;

        for (int i = 0, n = columns.Count; i < n; i++)
        {
            var column = columns[i];
            var e = Visit(column.Expression);

            if (alternate is null && e != column.Expression)
            {
                alternate = columns.Take(i).ToList();
            }

            alternate?.Add(new(column.Name, e!));
        }

        return alternate is not null ? alternate.AsReadOnly() : columns;
    }
}
