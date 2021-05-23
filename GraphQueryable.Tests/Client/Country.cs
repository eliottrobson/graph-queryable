using GraphQueryable.Attributes;

namespace GraphQueryable.Tests.Client
{
    public class Country
    {
        [GraphField("code", Order = 1)]
        public string Code { get; set; }
        
        [GraphField("name", Order = 2)]
        public string Name { get; set; } 
        
        [GraphField("continent", Order = 3)]
        public Continent Continent { get; set; }
    }
}