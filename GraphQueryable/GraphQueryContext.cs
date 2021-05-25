using System;
using System.Linq;
using GraphQueryable.Expressions;
using GraphQueryable.Tokens;
using GraphQueryable.Visitors;

namespace GraphQueryable
{
    public class GraphQueryContext
    {
        public Field Parse(IQueryable queryable)
        {
            if (queryable.Provider is not GraphQueryProvider graphQueryProvider)
                throw new NotSupportedException("IQueryable provider must be of type GraphQueryProvider");

            var simplifier = new ExpressionSimplifier();
            var simplifiedExpression = simplifier.Visit(queryable.Expression);
            
            var visitor = new ScopeVisitor();
            return visitor.ParseExpression(simplifiedExpression, graphQueryProvider.ScopeName);
        }
    }
}