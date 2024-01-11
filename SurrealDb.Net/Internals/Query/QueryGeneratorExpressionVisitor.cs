using System.Linq.Expressions;
using System.Text;
using SurrealDb.Net.Internals.Formatters;

namespace SurrealDb.Net.Internals.Query;

internal class QueryGeneratorExpressionVisitor : ExpressionVisitor
{
    private string _fromTable = string.Empty;
    private StringBuilder _surqlQueryBuilder = null!;

    public string Visit(Expression expression, string fromTable)
    {
        _fromTable = fromTable;
        _surqlQueryBuilder = new StringBuilder();

        _surqlQueryBuilder.Append($"SELECT * FROM {fromTable}");

        Visit(expression);

        return _surqlQueryBuilder.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _surqlQueryBuilder.Append('(');
        Visit(node.Left);

        switch (node.NodeType)
        {
            case ExpressionType.And:
            case ExpressionType.AndAlso:
                _surqlQueryBuilder.Append(" && ");
                break;
            case ExpressionType.Or:
            case ExpressionType.OrElse:
                _surqlQueryBuilder.Append(" || ");
                break;
            case ExpressionType.GreaterThan:
                _surqlQueryBuilder.Append(" > ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                _surqlQueryBuilder.Append(" >= ");
                break;
            case ExpressionType.LessThan:
                _surqlQueryBuilder.Append(" < ");
                break;
            case ExpressionType.LessThanOrEqual:
                _surqlQueryBuilder.Append(" <= ");
                break;
            case ExpressionType.Equal:
                _surqlQueryBuilder.Append(" == ");
                break;
            case ExpressionType.NotEqual:
                _surqlQueryBuilder.Append(" != ");
                break;
            case ExpressionType.Coalesce:
                _surqlQueryBuilder.Append(" ?? ");
                break;
            case ExpressionType.Add:
                _surqlQueryBuilder.Append(" + ");
                break;
            case ExpressionType.Subtract:
                _surqlQueryBuilder.Append(" - ");
                break;
            case ExpressionType.Multiply:
                _surqlQueryBuilder.Append(" * ");
                break;
            case ExpressionType.Divide:
                _surqlQueryBuilder.Append(" / ");
                break;
            case ExpressionType.Modulo:
                _surqlQueryBuilder.Append(" % ");
                break;
            default:
                throw new NotSupportedException(
                    string.Format("The binary operator '{0}' is not supported", node.NodeType)
                );
        }

        Visit(node.Right);
        _surqlQueryBuilder.Append(')');

        return node;
        //return base.VisitBinary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        string output = node.Value switch
        {
            null => "null",
            bool value => value ? "true" : "false",
            int value => value.ToString(),
            string value => $"\"{value}\"",
            _
                => throw new NotSupportedException(
                    string.Format("The constant '{0}' is not supported", node.Value)
                )
        };

        _surqlQueryBuilder.Append(output);

        return node;
        //return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        switch (node.NodeType)
        {
            case ExpressionType.MemberAccess:
                _surqlQueryBuilder.Append(node.Member.Name);
                break;
            default:
                throw new NotSupportedException(
                    string.Format(
                        "The member operation '{0}' on '{1}' is not supported",
                        node.NodeType,
                        node.Member.Name
                    )
                );
        }

        return node;
        //return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        switch (node.Method)
        {
            //case var method when method.Name == "Contains":
            //    _surqlQueryBuilder.Append(" CONTAINS ");
            //    Visit(node.Object);
            //    _surqlQueryBuilder.Append(", ");
            //    Visit(node.Arguments[0]);
            //    break;
            case var method when method.Name == "Select":
                _surqlQueryBuilder.Append("SELECT ");
                Visit(node.Arguments[1]);
                break;
            case var method when method.Name == "Where":
                _surqlQueryBuilder.Append(" WHERE ");
                Visit(node.Arguments[1]);
                break;
            case var method when method.Name == "FromDays" && node.Type == typeof(TimeSpan):
                var timeSpan = TimeSpan.FromDays(
                    (double)((ConstantExpression)node.Arguments[0]).Value!
                );
                _surqlQueryBuilder.Append(TimeSpanFormatter.Format(timeSpan));
                //_surqlQueryBuilder.Append(" FROMDAYS ");
                //Visit(node.Arguments[0]);
                break;
            default:
                throw new NotSupportedException(
                    string.Format("The method call '{0}' is not supported", node.Method.Name)
                );
        }

        return node;
        //return base.VisitMethodCall(node);
    }

    //private void ExpectSelectStatement()
    //{
    //    if (_surqlQueryBuilder.Length <= 0)
    //    {
    //        _surqlQueryBuilder.Append($"SELECT * FROM {_fromTable}");
    //    }
    //}
}
