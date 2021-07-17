using System;
using System.Collections;
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
        private readonly List<FieldFilter> _filters = new();

        public FieldFilter? ParseExpression(Expression node)
        {
            base.Visit(node);

            return _filters.SingleOrDefault();
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
                scopeItem.Filter ??= new FilteredItemFilter();

                if (scopeItem.Filter.Value == default)
                {
                    scopeItem.Filter.Value = node.Value;
                }
                else if (scopeItem.Filter.Value is IList filterList)
                {
                    filterList.Add(node.Value);
                }
                else
                {
                    var listType = typeof(List<>).MakeGenericType(scopeItem.Filter.Value.GetType());
                    var list = Activator.CreateInstance(listType) as IList;
                    list.Add(node.Value);
                    scopeItem.Filter.Value = list;
                }
                    
            }

            return base.VisitConstant(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var item = new FilteredItem
            {
                Filter = new FilteredItemFilter()
            };

            _filterScope.Push(item);

            var result = base.VisitBinary(node);

            _filterScope.Pop();

            var filter = node.NodeType switch
            {
                ExpressionType.Equal when item.Filter?.Value is not null =>
                    CreateFieldFilter(item, typeof(FieldFilterEqual<>)),
                ExpressionType.NotEqual when item.Filter?.Value is not null =>
                    CreateFieldFilter(item, typeof(FieldFilterNotEqual<>)),
                _ => throw new NotSupportedException($"Unsupported node type: '{node.NodeType}'")
            };

            if (filter is not null && item.Field is not null)
                filter.Name = FlattenFieldName(item.Field);

            if (filter is not null)
                _filters.Add(filter);

            return result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var item = new FilteredItem
            {
                Filter = new FilteredItemFilter()
            };

            _filterScope.Push(item);

            var result = base.VisitMethodCall(node);

            _filterScope.Pop();
            
            var filter = node.Method.Name switch
            {
                nameof(string.Contains) when node.Method.DeclaringType == typeof(string) =>
                    CreateFieldFilter(item, typeof(FieldFilterContains<>)),
                nameof(string.StartsWith) when node.Method.DeclaringType == typeof(string) =>
                    CreateFieldFilter(item, typeof(FieldFilterStartsWith<>)),
                nameof(string.EndsWith) when node.Method.DeclaringType == typeof(string) =>
                    CreateFieldFilter(item, typeof(FieldFilterEndsWith<>)),
                nameof(Enumerable.Contains) when
                    node.Method.DeclaringType == typeof(Enumerable) ||
                    (node.Arguments.Count == 1 && node.Method.DeclaringType ==
                        typeof(List<>).MakeGenericType(node.Arguments[0].Type)) =>
                    CreateFieldFilter(item, typeof(FieldFilterContains<>).MakeGenericType(typeof(List<>).MakeGenericType(node.Arguments[0].Type))),
                _ => throw new NotSupportedException($"Unsupported method type: '{node.Method}'")
            };

            if (filter is not null && item.Field is not null)
                filter.Name = FlattenFieldName(item.Field);

            if (filter is not null)
                _filters.Add(filter);

            return result;
        }

        private static List<string> FlattenFieldName(FilteredField field)
        {
            var filteredField = field;
            var name = new List<string>();

            do
            {
                if (filteredField.Name != null)
                    name.Add(filteredField.Name);

                filteredField = filteredField.Child;
            } while (filteredField != null);

            return name;
        }

        private static FieldFilter? CreateFieldFilter(FilteredItem item, Type type)
        {
            if (item.Filter is null) return null;

            if (type.IsGenericTypeDefinition && item.Filter.Value is not null)
                type = type.MakeGenericType(item.Filter.Value.GetType());

            var instance = Activator.CreateInstance(type);

            if (item.Filter.Value is not null)
                type.GetProperty("Value")?.SetValue(instance, item.Filter.Value, null);
            
            return instance as FieldFilter;
        }

        private class FilteredField
        {
            public string? Name { get; set; }

            public FilteredField? Child { get; set; }
        }

        private class FilteredItem
        {
            public FilteredField? Field { get; set; }

            public FilteredItemFilter? Filter { get; set; }
        }

        private class FilteredItemFilter
        {
            public object? Value { get; set; }
        }
    }
}