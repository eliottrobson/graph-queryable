using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GraphQueryable.Drivers;

namespace GraphQueryable
{
    public class GraphQueryable<T> : IOrderedQueryable<T>
    {
        #region Constructors
        
        public GraphQueryable(string methodName, IGraphVisitor visitor)
        {
            if (methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            
            Provider = new GraphQueryProvider(methodName, visitor);
            Expression = Expression.Constant(this);
        }

        public GraphQueryable(GraphQueryProvider provider, Expression expression)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException(nameof(expression));

            Provider = provider;
            Expression = expression;
        }
        
        #endregion
        
        #region Properties
        
        public IQueryProvider Provider { get; }
        public Expression Expression { get; }

        public Type ElementType => typeof(T);

        #endregion
        
        #region Enumerators
        
        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Provider.Execute<System.Collections.IEnumerable>(Expression).GetEnumerator();
        }
        
        #endregion
    }
}