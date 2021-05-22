using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GraphQueryable.Drivers;

namespace GraphQueryable
{
    public class GraphQueryContext
    {
        private readonly IGraphVisitor _visitor;

        public GraphQueryContext(IGraphVisitor visitor)
        {
            _visitor = visitor;
        }
        
        public object Execute(Expression expression)
        {
            return Execute(expression, false);
        }
        
        public TResult Execute<TResult>(Expression expression)
        {
            var isEnumerable = typeof(TResult).Name == "IEnumerable`1";
            return (TResult) Execute(expression, isEnumerable);
        }

        private object Execute(Expression expression, bool isEnumerable)
        {
            var objs = new List<object>();
            if (isEnumerable)
                return objs;
            else 
                return objs.First();
        }

        public string Parse(IQueryable queryable)
        {
            if (queryable.Provider is not GraphQueryProvider graphQueryProvider)
                throw new NotSupportedException("IQueryable provider must be of type GraphQueryProvider");

            return _visitor.ParseQuery(queryable.Expression, graphQueryProvider.ScopeName);
        }
    }
}