using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GraphQueryable.Attributes;

namespace GraphQueryable.Drivers.HotChocolate
{
    public class ProjectionVisitor : ExpressionVisitor
    {
        private readonly StringBuilder _queryStringBuilder;
        private readonly Stack<ProjectedItem> _projectionScope = new();
        private readonly List<ProjectedItem> _projections = new();

        public ProjectionVisitor()
        {
            _queryStringBuilder = new StringBuilder();
        }
        
        public string ParseQuery(Expression node)
        {
            base.Visit(node);
            _queryStringBuilder.AppendJoin(", ", ResolveProjections(_projections));
            var query = _queryStringBuilder.ToString();
            _queryStringBuilder.Clear();
            return query;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var graphFieldAttribute = node.Member.GetCustomAttribute<GraphFieldAttribute>();
            if (graphFieldAttribute != null)
            {
                var item = new ProjectedItem
                {
                    Name = graphFieldAttribute.Name,
                    Order = graphFieldAttribute.Order
                };
                
                if (_projectionScope.TryPeek(out var childItem))
                    item.Children.Add(childItem);
                    
                _projectionScope.Push(item);
            }

            var result = base.VisitMember(node);

            _projectionScope.Pop();

            return result;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_projectionScope.TryPeek(out var rootItem))
                _projections.Add(rootItem);
            
            return base.VisitParameter(node);
        }

        private static IEnumerable<string> ResolveProjections(IEnumerable<ProjectedItem> projections)
        {
           return projections
                .GroupBy(p => p.Name)
                .Select(p => new
                {
                    p.First().Name,
                    p.First().Order,
                    Children = p.SelectMany(ps => ps.Children)
                })
                .OrderBy(p => p.Order)
                .Select(p => p.Name + (p.Children.Any()
                    ? " { " + string.Join(", ", ResolveProjections(p.Children)) + " }"
                    : ""
                ));
        }
    }

    public class ProjectedItem
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public List<ProjectedItem> Children { get; } = new();
    }
}