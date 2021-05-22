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

        public string Query { get; set; }

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

        public void Parse(Expression expression)
        {
            _visitor.Visit(expression);
        }
        
        // private object ExecuteExpression(Expression expression)
        // {
        //     var objs = new List<object>();
        //     if (isEnumerable)
        //         return objs;
        //     else 
        //         return objs.First();
        //     
        //     
        //     // The expression must represent a query over the data source. 
        //     if (!IsQueryOverDataSource(expression))
        //         throw new InvalidProgramException("No query over the data source was specified.");
        //     
        //     // Call the Web service and get the results.
        //     // TODO: call api?
        //     List<object> places = new List<object>();
        //
        //     // Copy the IEnumerable places to an IQueryable.
        //     IQueryable<object> queryablePlaces = places.AsQueryable<object>();
        //
        //     // Copy the expression tree that was passed in, changing only the first 
        //     // argument of the innermost MethodCallExpression.
        //     ExpressionTreeModifier treeCopier = new ExpressionTreeModifier(queryablePlaces);
        //     Expression newExpressionTree = treeCopier.Visit(expression);
        //
        //     // This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods. 
        //     if (isEnumerable)
        //         return queryablePlaces.Provider.CreateQuery(newExpressionTree);
        //     else 
        //         return queryablePlaces.Provider.Execute(newExpressionTree);
        // }

        // private static bool IsQueryOverDataSource(Expression expression)
        // {
        //     // If expression represents an unqueried IQueryable data source instance, 
        //     // expression is of type ConstantExpression, not MethodCallExpression. 
        //     return (expression is MethodCallExpression);
        // }
        
        private IEnumerable<string> ParseProjections(Expression expression)
        {
            return new List<string>();
        }
    }
}