using System.Collections.Generic;

namespace GraphQueryable.Tokens
{
    public class FieldFilter
    {
        public List<string> Scope { get; set; }
        
        public FieldFilterType Type { get; set; }
        
        public object Value { get; set; }
    }

    public enum FieldFilterType
    {
        Unknown, 
        
        Equal
    }
}