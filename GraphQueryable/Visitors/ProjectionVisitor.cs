using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GraphQueryable.Attributes;
using GraphQueryable.Tokens;

namespace GraphQueryable.Visitors
{
    public class ProjectionVisitor : ExpressionVisitor
    {
        private readonly Stack<ProjectedItem> _projectionScope = new();
        private readonly List<ProjectedItem> _projections = new();

        public IEnumerable<Field> ParseExpression(Expression node)
        {
            base.Visit(node);
            return ResolveProjections(_projections);
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

            if (graphFieldAttribute != null)
            {
                _projectionScope.Pop();
            }

            return result;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_projectionScope.TryPeek(out var rootItem))
                _projections.Add(rootItem);
            
            return base.VisitParameter(node);
        }

        private static List<Field> ResolveProjections(IEnumerable<ProjectedItem> projections)
        {
            return projections
                .GroupBy(p => p.Name)
                .Select(p => new Field
                {
                    Name = p.First().Name,
                    Order = p.First().Order,
                    Children = ResolveProjections(p.SelectMany(ps => ps.Children))
                })
                .ToList();
        }
        
        private class ProjectedItem
        {
            public string Name { get; set; }

            public int Order { get; set; }

            public List<ProjectedItem> Children { get; } = new();
        }
    }
}