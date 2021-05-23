using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQueryable.Drivers.HotChocolate
{
    public class HotChocolateConventionVisitor : IGraphVisitor
    {
        public string ParseQuery(Expression expression, string scopeName)
        {
            var queryStringBuilder = new StringBuilder();
            var scopeVisitor = new ScopeVisitor(scopeName);
            queryStringBuilder.Append(scopeVisitor.ParseQuery(expression));
            var query = queryStringBuilder.ToString().Trim();
            queryStringBuilder.Clear();
                
            return query;
        }
    }
}