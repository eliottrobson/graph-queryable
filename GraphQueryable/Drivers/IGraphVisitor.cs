using System.Linq.Expressions;

namespace GraphQueryable.Drivers
{
    public interface IGraphVisitor
    {
        Expression Visit(Expression expression);
    }
}