using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GraphQueryable.Attributes;
using GraphQueryable.Tokens;

namespace GraphQueryable.Visitors
{
    public class FilterVisitor : ExpressionVisitor
    {
        private readonly Stack<FilteredItem> _filterScope = new();
        private readonly List<FilteredItem> _filters = new();
        
        public IEnumerable<FieldFilter> ParseExpression(Expression node)
        {
            base.Visit(node);
            return ResolveFilters(_filters);
        }
        
        protected override Expression VisitMember(MemberExpression node)
        {
            var graphFieldAttribute = node.Member.GetCustomAttribute<GraphFieldAttribute>();
            if (graphFieldAttribute != null)
            {
                if (_filterScope.TryPeek(out var childItem))
                {
                    childItem.Name = graphFieldAttribute.Name;
                    childItem.Order = graphFieldAttribute.Order;
                }
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var item = new FilteredItem
            {
                Filter = new FilteredItemFilter
                {
                    Type = FieldFilterType.Unknown
                }
            };
            
            _filterScope.Push(item);

            Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    item.Filter.Type = FieldFilterType.Equal;
                    break;
            }

            Visit(node.Right);

            _filterScope.Pop();
                            
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (_filterScope.TryPeek(out var scopeItem))
            {
                scopeItem.Filter.Value = node.Value;
            }

            return base.VisitConstant(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_filterScope.TryPeek(out var rootItem))
                _filters.Add(rootItem);
            
            return base.VisitParameter(node);
        }
        
        private static List<FieldFilter> ResolveFilters(IEnumerable<FilteredItem> filters)
        {
            return filters
                .GroupBy(f => f.Name)
                .SelectMany(f =>
                    f.Select(i => new FieldFilter
                    {
                        Field = new Field
                        {
                            Name = i.Name
                        },
                        Type = i.Filter.Type,
                        Value = i.Filter.Value
                    })
                    .ToList())
                .ToList();
        }
        
        private class FilteredItem
        {
            public string Name { get; set; }

            public int Order { get; set; }
            
            public FilteredItemFilter Filter { get; set; }

            public List<FilteredItem> Children { get; } = new();
        }

        private class FilteredItemFilter
        {
            public FieldFilterType Type { get; set; }
            
            public object Value { get; set; }
        }
    }
}