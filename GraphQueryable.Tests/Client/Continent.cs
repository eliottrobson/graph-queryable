using GraphQueryable.Attributes;

namespace GraphQueryable.Tests.Client
{
    public class Continent
    {
        [GraphField("code", Order = 1)]
        public string Code { get; set; }
        
        [GraphField("name", Order = 2)]
        public string Name { get; set; }
    }
}