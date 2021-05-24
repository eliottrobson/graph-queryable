using System.Collections.Generic;
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

            return new List<FieldFilter>();
        }
        
        protected override Expression VisitMember(MemberExpression node)
        {
            var graphFieldAttribute = node.Member.GetCustomAttribute<GraphFieldAttribute>();
            if (graphFieldAttribute != null)
            {
                var item = new FilteredItem
                {
                    Name = graphFieldAttribute.Name,
                    Order = graphFieldAttribute.Order,
                    FilterType = FieldFilterType.Unknown
                };
                
                if (_filterScope.TryPeek(out var childItem))
                    item.Children.Add(childItem);
                    
                _filterScope.Push(item);
            }

            var result = base.VisitMember(node);

            if (graphFieldAttribute != null)
            {
                _filterScope.Pop();
            }

            return result;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);

            if (_filterScope.TryPeek(out var scopeItem))
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Equal:
                        scopeItem.FilterType = FieldFilterType.Equal;
                        break;
                }
            }

            Visit(node.Right);
            
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (_filterScope.TryPeek(out var scopeItem))
            {
                scopeItem.FilterValue = node.Value;
            }

            return base.VisitConstant(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_filterScope.TryPeek(out var rootItem))
                _filters.Add(rootItem);
            
            return base.VisitParameter(node);
        }
        
        private class FilteredItem
        {
            public string Name { get; set; }

            public int Order { get; set; }
            
            public FieldFilterType FilterType { get; set; }
            
            public object FilterValue { get; set; }

            public List<FilteredItem> Children { get; } = new();
        }
    }
}