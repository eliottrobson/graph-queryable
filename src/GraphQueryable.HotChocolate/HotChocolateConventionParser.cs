using GraphQueryable.Tokens;

namespace GraphQueryable.HotChocolate
{
    public class HotChocolateConventionParser
    {
        public string Parse(Field field)
        {
            var projectionParser = new ProjectionParser();
            return projectionParser.Resolve(field);
        }
    }
}