using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GraphQueryable.Attributes;

namespace GraphQueryable.Drivers.HotChocolate
{
    public class ProjectionVisitor : ExpressionVisitor
    {
        public List<string> Projections { get; set; } = new();

        protected override Expression VisitMember(MemberExpression node)
        {
            var graphFieldAttribute = node.Member.GetCustomAttribute<GraphFieldAttribute>();
            if (graphFieldAttribute != null)
                Projections.Add(graphFieldAttribute.Name);
            
            return base.VisitMember(node);
        }
    }
}