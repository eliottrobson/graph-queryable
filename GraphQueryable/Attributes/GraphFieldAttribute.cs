using System;
using System.Runtime.CompilerServices;

namespace GraphQueryable.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GraphFieldAttribute : Attribute
    {
        public string Name { get; }

        public GraphFieldAttribute(string name)
        {
            Name = name;
        }
    }
}