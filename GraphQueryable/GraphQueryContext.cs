using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            var visitor = new ScopeVisitor();
            return visitor.ParseExpression(queryable.Expression, graphQueryProvider.ScopeName);
        }
    }
}