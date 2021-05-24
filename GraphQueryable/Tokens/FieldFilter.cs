using System.Collections.Generic;

namespace GraphQueryable.Tokens
{
    public record FieldFilter
    {
        public List<string> Name { get; set; }
        
        public FieldFilterType Type { get; set; }
        
        public object Value { get; set; }
    }

    public enum FieldFilterType
    {
        None, 
        
        Not,
        And,
        Or,
        
        Equal,
        NotEqual,
        
        In,
        
        StringContains,
        StringStartsWith,
        StringEndsWith
    }
}