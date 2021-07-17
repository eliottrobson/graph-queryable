using System.Linq;
using System.Linq.Expressions;
using GraphQueryable.Tokens;

namespace GraphQueryable.Visitors
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
                _field.Projections.AddRange(children);
            }
            else if (node.Method.Name == nameof(Queryable.Where) && node.Method.DeclaringType == typeof(Queryable))
            {
                var filterVisitor = new FilterVisitor();

                var filter = filterVisitor.ParseExpression(node.Arguments[1]);

                if (filter is not null)
                {
                    _field.Filter = _field.Filter is null
                        ? filter
                        : new FieldFilterAnd
                        {
                            Value = (Left: _field.Filter, Right: filter)
                        };
                }
                
            }
            
            return base.VisitMethodCall(node);
        }
    }
}