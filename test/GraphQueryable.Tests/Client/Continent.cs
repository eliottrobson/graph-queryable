using GraphQueryable.Attributes;

namespace GraphQueryable.Tests.Client
{
    public class Continent
    {
        [GraphField("code")]
        public string Code { get; set; }
        
        [GraphField("name")]
        public string Name { get; set; }
    }
}