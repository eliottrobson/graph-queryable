using System.Collections.Generic;

namespace GraphQueryable.Tokens
{
    public record Field
    {
        public string Name { get; set; }
        
        public int Order { get; set; }
        public List<Field> Children { get; set; } = new();
    }
}