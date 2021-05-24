using System.Collections.Generic;

namespace GraphQueryable.Tokens
{
    public record Field
    {
        public string Name { get; set; }
        
        public List<FieldFilter> Filters { get; init; } = new();
        
        public List<Field> Children { get; init; } = new();
    }
}