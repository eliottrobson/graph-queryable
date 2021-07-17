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
        private readonly Stack<ProjectedField> _memberScope = new();
        private readonly List<ProjectedField> _projections = new();

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
                var item = new ProjectedField
                {
                    Name = graphFieldAttribute.Name
                };

                if (_memberScope.TryPeek(out var childItem))
                    item.Child = childItem;

                _memberScope.Push(item);
            }

            var result = base.VisitMember(node);

            if (graphFieldAttribute != null)
            {
                _memberScope.Pop();
            }

            return result;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_memberScope.TryPeek(out var rootItem))
                _projections.Add(rootItem);

            return base.VisitParameter(node);
        }

        private static List<Field> ResolveProjections(IEnumerable<ProjectedField?> projections)
        {
            return projections
                .Where(p => p != null)
                .GroupBy(p => p.Name)
                .Select(p => new Field
                {
                    Name = p.First().Name,
                    Projections = ResolveProjections(p.Select(ps => ps.Child))
                })
                .ToList();
        }

        private class ProjectedField
        {
            public string? Name { get; set; }

            public ProjectedField? Child { get; set; }
        }
    }
}