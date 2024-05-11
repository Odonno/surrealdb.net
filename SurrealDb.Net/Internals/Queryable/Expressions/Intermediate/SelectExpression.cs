using System.Linq.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Expressions.Intermediate;

internal sealed class SelectExpression : IntermediateExpression
{
    public SourceExpression Source { get; }
    public ProjectionExpression Projection { get; private set; }
    public WhereExpression? Where { get; private set; }
    public GroupsExpression? Groups { get; private set; }
    public OrdersExpression? Orders { get; private set; }
    public TakeExpression? Take { get; private set; }
    public SkipExpression? Skip { get; private set; }

    private SelectExpression(SelectExpression from)
        : base(from.Type)
    {
        Source = from.Source;
        Projection = from.Projection;
        Where = from.Where;
        Groups = from.Groups;
        Orders = from.Orders;
        Take = from.Take;
        Skip = from.Skip;
    }

    public SelectExpression(Type resultType, TableExpression table, ProjectionExpression projection)
        : base(resultType)
    {
        Source = new TableSourceExpression(table);
        Projection = projection;
    }

    public SelectExpression(
        Type resultType,
        SelectExpression select,
        ProjectionExpression? projection = null
    )
        : base(resultType)
    {
        Source = new SelectSourceExpression(select);
        Projection = projection ?? select.Projection;
    }

    public SelectExpression WithProjection(ProjectionExpression projection)
    {
        return new SelectExpression(this) { Projection = projection };
    }

    public SelectExpression AppendWhere(Expression predicate)
    {
        var innerWhereExpression = Where is not null
            ? AndAlso(Where.Expression, predicate)
            : predicate;
        return new SelectExpression(this) { Where = new(innerWhereExpression) };
    }

    public SelectExpression WithGroup(Expression expression)
    {
        return new SelectExpression(this) { Groups = new GroupsExpression(expression) };
    }

    public SelectExpression WithGroupAll()
    {
        return new SelectExpression(this) { Groups = new GroupsExpression(null) };
    }

    public SelectExpression WithOrder(OrderByInfo value)
    {
        return new SelectExpression(this) { Orders = new OrdersExpression([value]) };
    }

    public SelectExpression AppendOrder(OrderByInfo value)
    {
        return Orders is null
            ? WithOrder(value)
            : new SelectExpression(this)
            {
                Orders = new OrdersExpression([.. Orders.Infos, value]),
            };
    }

    public SelectExpression WithTake(Expression value)
    {
        return new SelectExpression(this) { Take = new(value) };
    }

    public SelectExpression WithSkip(Expression value)
    {
        return new SelectExpression(this) { Skip = new(value) };
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        visitor.Visit(Projection);
        visitor.Visit(Where);
        visitor.Visit(Orders);
        visitor.Visit(Take);
        visitor.Visit(Skip);

        return this;
    }
}
