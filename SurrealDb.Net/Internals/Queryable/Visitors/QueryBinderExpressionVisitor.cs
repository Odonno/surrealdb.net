using System.Linq.Expressions;
using System.Reflection;
using SurrealDb.Net.Internals.Queryable.Expressions;

namespace SurrealDb.Net.Internals.Queryable.Visitors;

internal class QueryBinderExpressionVisitor : ExpressionVisitor
{
    public ColumnProjectorExpressionVisitor ColumnProjector { get; }
    public Dictionary<ParameterExpression, Expression>? Map { get; private set; }
    private int _aliasCount;

    internal QueryBinderExpressionVisitor()
    {
        ColumnProjector = new(CanBeColumn);
    }

    internal Expression Bind(Expression expression)
    {
        Map = [];
        return Visit(expression);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (
            node.Method.DeclaringType == typeof(System.Linq.Queryable)
            || node.Method.DeclaringType == typeof(Enumerable)
        )
        {
            return node.Method.Name switch
            {
                "Where"
                    => BindWhere(
                        node.Type,
                        node.Arguments[0],
                        (LambdaExpression)StripQuotes(node.Arguments[1])
                    ),
                "Select"
                    => BindSelect(
                        node.Type,
                        node.Arguments[0],
                        (LambdaExpression)StripQuotes(node.Arguments[1])
                    ),
                "Take" => BindTake(node.Arguments[0], node.Arguments[1]),
                "Skip" => BindSkip(node.Arguments[0], node.Arguments[1]),
                _
                    => throw new NotSupportedException(
                        string.Format("The method '{0}' is not supported", node.Method.Name)
                    ),
            };
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (IsTable(node.Value))
        {
            var table = node.Value as IQueryable;
            if (table is null)
            {
                throw new InvalidOperationException("Invalid source node type");
            }

            var provider = table.Provider as ISurrealDbQueryProvider;
            if (provider is null)
            {
                throw new InvalidOperationException("Invalid provider type");
            }

#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
            var resultType = typeof(IEnumerable<>).MakeGenericType(table.ElementType);
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.

            return new SelectExpression(
                resultType,
                string.Empty,
                new TableExpression(resultType, string.Empty, provider.FromTable),
                null,
                null,
                null
            );

            //return GetTableProjection(node.Value, node.Type);
        }

        //return base.VisitConstant(node);
        return node;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return Map?.TryGetValue(node, out var e) == true ? e : node;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        return node;
    }

    protected override Expression VisitNew(NewExpression node)
    {
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        return node;
    }

    //protected override Expression VisitMemberAccess(MemberExpression m)
    //{
    //    var source = Visit(m.Expression);

    //    switch (source.NodeType)
    //    {
    //        case ExpressionType.MemberInit:

    //            var min = (MemberInitExpression)source;

    //            for (int i = 0, n = min.Bindings.Count; i < n; i++)
    //            {
    //                MemberAssignment assign = min.Bindings[i] as MemberAssignment;

    //                if (assign != null && MembersMatch(assign.Member, m.Member))
    //                {
    //                    return assign.Expression;
    //                }
    //            }

    //            break;

    //        case ExpressionType.New:

    //            NewExpression nex = (NewExpression)source;

    //            if (nex.Members != null)
    //            {
    //                for (int i = 0, n = nex.Members.Count; i < n; i++)
    //                {
    //                    if (MembersMatch(nex.Members[i], m.Member))
    //                    {
    //                        return nex.Arguments[i];
    //                    }
    //                }
    //            }

    //            break;
    //    }

    //    if (source == m.Expression)
    //    {
    //        return m;
    //    }

    //    return MakeMemberAccess(source, m.Member);
    //}

    private Expression BindSkip(Expression source, Expression value)
    {
        var sourceSelect = (SelectExpression)Visit(source);

        return new SelectExpression(
            sourceSelect.Type,
            sourceSelect.Alias,
            sourceSelect.From,
            sourceSelect.Where,
            sourceSelect.Limit,
            value
        );
    }

    private Expression BindTake(Expression source, Expression value)
    {
        var sourceSelect = (SelectExpression)Visit(source);

        return new SelectExpression(
            sourceSelect.Type,
            sourceSelect.Alias,
            sourceSelect.From,
            sourceSelect.Where,
            value,
            sourceSelect.Start
        );
    }

    private Expression BindWhere(Type resultType, Expression source, LambdaExpression predicate)
    {
        var sourceSelect = (SelectExpression)Visit(source);

        Map?.Add(predicate.Parameters[0], sourceSelect);
        var where = Visit(predicate.Body);

        var newWhere = sourceSelect.Where is not null
            ? Expression.AndAlso(sourceSelect.Where, where)
            : where;

        return new SelectExpression(
            resultType,
            string.Empty,
            sourceSelect.From,
            newWhere,
            sourceSelect.Start,
            sourceSelect.Limit
        );

        // ----------------------------------------------------------------
        //var sourceSelect = (SelectExpression)Visit(source);

        //Map?.Add(predicate.Parameters[0], sourceSelect);
        //var where = Visit(predicate.Body);

        //if (TryMergeSelects(sourceSelect, out var mergedSelect))
        //{
        //    return mergedSelect;
        //}

        //return new SelectExpression(resultType, string.Empty, sourceSelect, where);
        // ----------------------------------------------------------------

        //var projection = (ProjectionExpression)Visit(source);

        //Map?.Add(predicate.Parameters[0], projection.Projector);

        //var where = Visit(predicate.Body);

        //string alias = GetNextAlias();

        //var pc = ProjectColumns(projection.Projector, alias, GetExistingAlias(projection.Source));

        //return new ProjectionExpression(
        //    new SelectExpression(resultType, alias, projection.Source, where),
        //    pc.Projector
        //);
    }

    private Expression BindSelect(Type resultType, Expression source, LambdaExpression selector)
    {
        var sourceSelect = (SelectExpression)Visit(source);

        Map?.Add(selector.Parameters[0], sourceSelect);

        var expression = Visit(selector.Body); // TODO : Used to get Columns

        bool canMerge = true; // TODO

        if (canMerge)
        {
            if (sourceSelect.AllFields)
            {
                return new SelectExpression(
                    resultType,
                    string.Empty,
                    sourceSelect.From,
                    sourceSelect.Where,
                    sourceSelect.Start,
                    sourceSelect.Limit
                );
            }

            // TODO : Detect columns
            return new SelectExpression(
                resultType,
                string.Empty,
                [],
                sourceSelect.From,
                sourceSelect.Where,
                sourceSelect.Start,
                sourceSelect.Limit
            );
        }

        return new SelectExpression(resultType, string.Empty, [], sourceSelect, null, null, null); // TODO

        //var projection = (ProjectionExpression)Visit(source);

        //Map?.Add(selector.Parameters[0], projection.Projector);

        //var expression = Visit(selector.Body);

        //string alias = GetNextAlias();

        //var pc = ProjectColumns(expression, alias, GetExistingAlias(projection.Source));

        //return new ProjectionExpression(
        //    new SelectExpression(resultType, alias, pc.Columns, projection.Source, []),
        //    pc.Projector
        //);
    }

    private ProjectionExpression GetTableProjection(object? value, Type type)
    {
        var table = value as IQueryable;
        if (table is null)
        {
            throw new InvalidOperationException("Invalid source node type");
        }

        string tableAlias = GetNextAlias();
        string selectAlias = GetNextAlias();

        List<MemberBinding> bindings = [];
        List<ColumnDeclaration> columns = [];

        //#pragma warning disable IL2070 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
        //        var typeProperties = type.GetProperties().ToArray();
        //#pragma warning restore IL2070 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.

        foreach (var mi in GetMappedMembers(table.ElementType))
        {
            string columnName = GetColumnName(mi);
            var columnType = GetColumnType(mi);

            bindings.Add(
                Expression.Bind(mi, new ColumnExpression(columnType, selectAlias, columnName))
            );

            columns.Add(
                new ColumnDeclaration(
                    columnName,
                    new ColumnExpression(columnType, tableAlias, columnName)
                )
            );
        }

#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
        var projector = Expression.MemberInit(Expression.New(table.ElementType), bindings);
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
        var resultType = typeof(IEnumerable<>).MakeGenericType(table.ElementType);
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.

        return new ProjectionExpression(
            new SelectExpression(
                resultType,
                selectAlias,
                columns,
                new TableExpression(resultType, tableAlias, GetTableName(table)),
                null,
                null,
                null
            ),
            projector
        );
    }

    //private static bool IsQuery(Expression expression)
    //{
    //    //var elementType = TypeHelper.GetElementType(expression.Type);
    //    return elementType is not null
    //        && typeof(IQueryable<>).MakeGenericType(elementType).IsAssignableFrom(expression.Type);
    //}

    private static bool IsTable(object? value)
    {
        var q = value as IQueryable;
        return q is not null && q.Expression.NodeType == ExpressionType.Constant;
    }

    private ProjectedColumns ProjectColumns(
        Expression expression,
        string newAlias,
        string existingAlias
    )
    {
        return ColumnProjector.ProjectColumns(expression, newAlias, existingAlias);
    }

    private string GetNextAlias()
    {
        return "t" + _aliasCount++;
    }

    private static bool CanBeColumn(Expression expression)
    {
        return expression is SurrealExpression surrealExpression
            && surrealExpression.SurrealNodeType == SurrealExpressionType.Column;
    }

    private static Expression StripQuotes(Expression e)
    {
        while (e.NodeType == ExpressionType.Quote)
        {
            e = ((UnaryExpression)e).Operand;
        }

        return e;
    }

    private static IEnumerable<MemberInfo> GetMappedMembers(Type rowType)
    {
#pragma warning disable IL2070 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
        return rowType.GetFields().Cast<MemberInfo>();
#pragma warning restore IL2070 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
    }

    private static string GetColumnName(MemberInfo member)
    {
        return member.Name;
    }

    private static Type GetColumnType(MemberInfo member)
    {
        var fi = member as FieldInfo;
        if (fi is not null)
        {
            return fi.FieldType;
        }

        var pi = (PropertyInfo)member;
        return pi.PropertyType;
    }

    private static string GetTableName(object table)
    {
        var tableQuery = (IQueryable)table;
        var rowType = tableQuery.ElementType;

        return rowType.Name;
    }

    private static string GetExistingAlias(Expression node)
    {
        var surrealExpression = node as SurrealExpression;
        if (surrealExpression is null)
        {
            throw new InvalidOperationException(
                string.Format("Invalid source node type '{0}'", node.NodeType)
            );
        }

        return surrealExpression.SurrealNodeType switch
        {
            SurrealExpressionType.Select => ((SelectExpression)node).Alias,
            SurrealExpressionType.Table => ((TableExpression)node).Alias,
            _
                => throw new InvalidOperationException(
                    string.Format(
                        "Invalid source node type '{0}'",
                        surrealExpression.SurrealNodeType
                    )
                ),
        };
    }
}
