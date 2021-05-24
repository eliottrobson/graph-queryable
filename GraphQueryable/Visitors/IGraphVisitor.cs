using System.Linq.Expressions;

namespace GraphQueryable.Visitors
{
    public interface IGraphVisitor
    {
        string ParseQuery(Expression expression, string scopeName);
    }
}