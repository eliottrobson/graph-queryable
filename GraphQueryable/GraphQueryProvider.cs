using System;
using System.Linq;
using System.Linq.Expressions;
using GraphQueryable.Helpers;

namespace GraphQueryable
{
    public class GraphQueryProvider : IQueryProvider
    {
        public string ScopeName { get; }

        public GraphQueryProvider(string scopeName)
        {
            ScopeName = scopeName;
        }
        
        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                var type = typeof(GraphQueryable<>).MakeGenericType(elementType);
                return (IQueryable) Activator.CreateInstance(type, this, expression);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new GraphQueryable<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            // return new GraphQueryContext(_visitor).Execute(expression);
            return default;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // return new GraphQueryContext(_visitor).Execute<TResult>(expression);
            return default;
        }
    }
}