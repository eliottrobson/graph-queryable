using System;
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
        private readonly Stack<FilteredField> _memberScope = new();
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
                var item = new FilteredField
                {
                    Name = graphFieldAttribute.Name
                };

                if (_memberScope.TryPeek(out var childItem))
                    item.Child = childItem;

                _memberScope.Push(item);
            }
            else
            {
                throw new NotImplementedException("TODO: add support for accessing external objects.");
            }

            var result = base.VisitMember(node);

            if (graphFieldAttribute != null)
            {
                var field = _memberScope.Pop();

                if (_filterScope.TryPeek(out var rootItem) && rootItem.Field == default)
                    rootItem.Field = field;
            }

            return result;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (_filterScope.TryPeek(out var scopeItem))
            {
                if (scopeItem.Filter.Value == default)
                {
                    scopeItem.Filter.Value = node.Value;
                }
                else if (scopeItem.Filter.Value is List<object> filterList)
                {
                    filterList.Add(node.Value);
                }
                else
                {
                    scopeItem.Filter.Value = new[] {scopeItem.Filter.Value}.Append(node.Value).ToList();
                }
            }

            return base.VisitConstant(node);
        }
        
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var item = new FilteredItem
            {
                Filter = new FilteredItemFilter
                {
                    Type = FieldFilterType.None
                }
            };

            _filterScope.Push(item);

            var result = base.VisitBinary(node);

            _filterScope.Pop();

            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    item.Filter.Type = FieldFilterType.Equal;
                    break;
                case ExpressionType.NotEqual:
                    item.Filter.Type = FieldFilterType.NotEqual;
                    break;
            }

            return result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var item = new FilteredItem
            {
                Filter = new FilteredItemFilter
                {
                    Type = FieldFilterType.None
                }
            };

            _filterScope.Push(item);

            item.Filter.Type = node.Method.Name switch
            {
                nameof(string.Contains) when node.Method.DeclaringType == typeof(string) =>
                    FieldFilterType.StringContains,
                nameof(string.StartsWith) when node.Method.DeclaringType == typeof(string) =>
                    FieldFilterType.StringStartsWith,
                nameof(string.EndsWith) when node.Method.DeclaringType == typeof(string) =>
                    FieldFilterType.StringEndsWith,
                nameof(Enumerable.Contains) when
                    node.Method.DeclaringType == typeof(Enumerable) ||
                    (node.Arguments.Count == 1 && node.Method.DeclaringType ==
                        typeof(List<>).MakeGenericType(node.Arguments[0].Type)) =>
                    FieldFilterType.In,
                _ => throw new NotSupportedException($"Unsupported method type: '{node.Method}'")
            };

            var result = base.VisitMethodCall(node);

            _filterScope.Pop();

            return result;
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
                .Select(f => new FieldFilter
                {
                    Name = FlattenFieldName(f.Field),
                    Type = f.Filter.Type,
                    Value = f.Filter.Value
                })
                .ToList();
        }

        private static List<string> FlattenFieldName(FilteredField field)
        {
            var name = new List<string> {field.Name};
            while (field.Child != null)
            {
                field = field.Child;
                name.Add(field.Name);
            }

            return name;
        }

        private class FilteredField
        {
            public string Name { get; set; }

            public FilteredField Child { get; set; }
        }

        private class FilteredItem
        {
            public FilteredField Field { get; set; }

            public FilteredItemFilter Filter { get; set; }
        }

        private class FilteredItemFilter
        {
            public FieldFilterType Type { get; set; }

            public object Value { get; set; }
        }
    }
}