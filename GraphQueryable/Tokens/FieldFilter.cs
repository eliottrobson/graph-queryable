using System;
using System.Collections.Generic;
using GraphQueryable.Helpers;

namespace GraphQueryable.Tokens
{
    public sealed record FieldFilter
    {
        public List<string> Name { get; init; } = new();
        
        public FieldFilterType? Type { get; init; }
        
        public object? Value { get; init; }
        
        public bool Equals(FieldFilter? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return ValueEquality.Equal(Name, other.Name) &&
                   ValueEquality.Equal(Type, other.Type) &&
                   ValueEquality.Equal(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ValueEquality.GetHashCode(Name),
                ValueEquality.GetHashCode(Type),
                ValueEquality.GetHashCode(Value));
        }
    }

    public enum FieldFilterType
    {
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