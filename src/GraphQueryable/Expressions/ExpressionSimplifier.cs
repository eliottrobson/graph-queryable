using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GraphQueryable.Expressions
{
    public class ExpressionSimplifier : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            return ReduceMemberExpressionValue(node);
        }
        
        private Expression ReduceMemberExpressionValue(MemberExpression node)
        {
            var nestedNode = node;
            while (nestedNode.Expression is MemberExpression memberExpression)
                nestedNode = memberExpression;
            var constant = nestedNode.Expression as ConstantExpression;
            var anonymousClassInstance = constant?.Value;
            var anonymousField = nestedNode.Member as FieldInfo;
            var instanceValue = anonymousField?.GetValue(anonymousClassInstance);
            
            if (instanceValue == default)
                return base.VisitMember(node);

            // Inline list values
            if (instanceValue is IList listValue)
            {
                var instanceType = instanceValue.GetType();
                if (instanceType.IsGenericType && instanceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                {
                    var listInitExpression = Expression.ListInit(
                        Expression.New(instanceType),
                        listValue.OfType<object>().Select(Expression.Constant)
                    );

                    return Visit(listInitExpression);
                }
            }

            return base.VisitMember(node);
        }
    }
}