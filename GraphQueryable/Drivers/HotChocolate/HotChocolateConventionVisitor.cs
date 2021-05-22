using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQueryable.Drivers.HotChocolate
{
    public class HotChocolateConventionVisitor : ExpressionVisitor, IGraphVisitor
    {
        private readonly StringBuilder _queryStringBuilder;
        
        public HotChocolateConventionVisitor()
        {
            _queryStringBuilder = new StringBuilder();
        }

        public string ParseQuery(Expression expression, string scopeName)
        {
            _queryStringBuilder.Append(scopeName);
            Visit(expression);
            var query = _queryStringBuilder.ToString().Trim();
            _queryStringBuilder.Clear();
                
            return query;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == nameof(Queryable.Select) && node.Method.DeclaringType == typeof(Queryable))
            {
                var projectionVisitor = new ProjectionVisitor();
                projectionVisitor.Visit(node.Arguments[1]);
                
                _queryStringBuilder.Append(" { ");
                _queryStringBuilder.AppendJoin(", ", projectionVisitor.Projections);
                _queryStringBuilder.Append(" } ");
            }
            
            return base.VisitMethodCall(node);
        }
    }
}