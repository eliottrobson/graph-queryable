using GraphQueryable.Attributes;

namespace GraphQueryable.Tests.Client
{
    public class Country
    {
        [GraphField("code")]
        public string Code { get; set; }
        
        [GraphField("name")]
        public string Name { get; set; } 
        
        [GraphField("continent")]
        public Continent Continent { get; set; }
    }
}