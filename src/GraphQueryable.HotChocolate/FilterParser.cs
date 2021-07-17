using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQueryable.Tokens;

namespace GraphQueryable.HotChocolate
{
    internal class FilterParser
    {
        private readonly StringBuilder _stringBuilder = new();
        
        public string Resolve(IEnumerable<FieldFilter> fields)
        {
            foreach (var field in fields)
                ResolveFilter(field);
            
            return _stringBuilder.ToString();
        }

        private void ResolveFilter(FieldFilter filter)
        {
            if (filter.Name.Any())
            {
                foreach (var name in filter.Name)
                    _stringBuilder.Append(name);

                _stringBuilder.Append(": { ");
            }
            
            ResolveFilterType(filter);

            if (filter.Name.Any())
            {
                _stringBuilder.Append(" }");
            }
        }

        private void ResolveFilterType(FieldFilter filter)
        {
            switch (filter)
            {
                case FieldFilterAnd filterAnd:
                    ResolveFilterType(filterAnd);
                    break;
                case FieldFilterOr filterOr:
                    ResolveFilterType(filterOr);
                    break;
                case FieldFilterEqual<string> filterEqual:
                    ResolveFilterType(filterEqual);
                    break;
                case FieldFilterNotEqual<string> filterNotEqual:
                    ResolveFilterType(filterNotEqual);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported filter type: '{filter.GetType()}'");
            }
        }

        private void ResolveFilterType(FieldFilterAnd filter)
        {
            _stringBuilder.Append("and: [{ ");
            ResolveFilter(filter.Value.Left);
            _stringBuilder.Append(" }, { ");
            ResolveFilter(filter.Value.Right);
            _stringBuilder.Append(" }]");
        }
        
        private void ResolveFilterType(FieldFilterOr filter)
        {
            _stringBuilder.Append("or: [{ ");
            ResolveFilter(filter.Value.Left);
            _stringBuilder.Append(" }, { ");
            ResolveFilter(filter.Value.Right);
            _stringBuilder.Append(" }]");
        }
        
        private void ResolveFilterType(FieldFilterEqual<string> filter)
        {
            _stringBuilder.Append("eq: \"");
            _stringBuilder.Append(filter.Value);
            _stringBuilder.Append("\"");
        }
        
        private void ResolveFilterType(FieldFilterNotEqual<string> filter)
        {
            _stringBuilder.Append("neq: \"");
            _stringBuilder.Append(filter.Value);
            _stringBuilder.Append("\"");
        }
    }
}