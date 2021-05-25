using System;
using System.Collections.Generic;
using GraphQueryable.Helpers;

namespace GraphQueryable.Tokens
{
    public sealed record Field
    {
        public Field()
        {
            
        }
        
        public Field(string name) : this()
        {
            Name = name;
        }
        
        public string? Name { get; init; }
        
        public List<FieldFilter> Filters { get; init; } = new();
        
        public List<Field> Children { get; init; } = new();

        public bool Equals(Field? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return ValueEquality.Equal(Name, other.Name) &&
                   ValueEquality.Equal(Filters, other.Filters) &&
                   ValueEquality.Equal(Children, other.Children);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ValueEquality.GetHashCode(Name),
                ValueEquality.GetHashCode(Filters),
                ValueEquality.GetHashCode(Children));
        }
    }
}