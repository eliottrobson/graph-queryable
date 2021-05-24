using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GraphQueryable.Tokens;

namespace GraphQueryable.Visitors.HotChocolate
{
    public class ScopeVisitor : ExpressionVisitor
    {
        private Field _field;
        
        public Field ParseExpression(Expression node, string scopeName)
        {
            _field = new Field
            {
                Name = scopeName
            };
            
            base.Visit(node);
            
            return _field;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == nameof(Queryable.Select) && node.Method.DeclaringType == typeof(Queryable))
            {
                var projectionVisitor = new ProjectionVisitor();

                var children = projectionVisitor.ParseExpression(node.Arguments[1]);
                _field.Children.AddRange(children);
            }
            
            return base.VisitMethodCall(node);
        }
    }
}