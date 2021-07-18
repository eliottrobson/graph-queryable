using System;
using System.Collections.Generic;
using GraphQueryable.Helpers;

namespace GraphQueryable.Tokens
{
    public class FieldFilter : IEquatable<FieldFilter>
    {
        public List<string> Name { get; set; } = new();

        public bool Equals(FieldFilter? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return GetType() == other.GetType() &&
                   ValueEquality.Equal(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                GetType(),
                ValueEquality.GetHashCode(Name));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FieldFilter);
        }
    }

    public class FieldFilter<T> : FieldFilter, IEquatable<FieldFilter<T>>
    {
        public T? Value { get; set; }

        public bool Equals(FieldFilter<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return GetType() == other.GetType() &&
                   ValueEquality.Equal(Name, other.Name) &&
                   ValueEquality.Equal(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                GetType(),
                ValueEquality.GetHashCode(Name),
                ValueEquality.GetHashCode(Value));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FieldFilter);
        }
    }
}