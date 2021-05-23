using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQueryable.Drivers.HotChocolate
{
    public class ScopeVisitor : ExpressionVisitor
    {
        private readonly string _scopeName;
        private readonly StringBuilder _queryStringBuilder;

        public ScopeVisitor(string scopeName)
        {
            _scopeName = scopeName;
            _queryStringBuilder = new StringBuilder();
        }

        public string ParseQuery(Expression node)
        {
            _queryStringBuilder.Append(_scopeName);
            base.Visit(node);
            var query = _queryStringBuilder.ToString();
            _queryStringBuilder.Clear();
            return query;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == nameof(Queryable.Select) && node.Method.DeclaringType == typeof(Queryable))
            {
                var projectionVisitor = new ProjectionVisitor();
                
                _queryStringBuilder.Append(" { ");
                _queryStringBuilder.Append(projectionVisitor.ParseQuery(node.Arguments[1]));
                _queryStringBuilder.Append(" } ");
            }
            
            return base.VisitMethodCall(node);
        }
    }
}