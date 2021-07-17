using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQueryable.Tokens;

namespace GraphQueryable.HotChocolate
{
    internal class ProjectionParser
    {
        private readonly StringBuilder _stringBuilder = new();
        
        public string Resolve(Field field)
        {
            _stringBuilder.Append(field.Name);

            if (field.Filters.Any())
            {
                var filterParser = new FilterParser();
                var filters = filterParser.Resolve(field.Filters);
                _stringBuilder.Append("(where: { ");
                _stringBuilder.Append(filters);
                _stringBuilder.Append(" })");
            }

            if (field.Projections.Any())
            {
                var projectionParser = new ProjectionParser();
                var projections = projectionParser.Resolve(field.Projections);
                _stringBuilder.Append(" { ");
                _stringBuilder.Append(projections);
                _stringBuilder.Append(" }");
            }
            
            return _stringBuilder.ToString();
        }
        
        public string Resolve(IEnumerable<Field> projections)
        {
            foreach (var projection in projections)
                Resolve(projection);
            
            return _stringBuilder.ToString();
        }
    }
}