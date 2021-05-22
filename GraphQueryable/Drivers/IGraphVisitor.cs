using System.Linq.Expressions;

namespace GraphQueryable.Drivers
{
    public interface IGraphVisitor
    {
        string ParseQuery(Expression expression, string scopeName);
    }
}