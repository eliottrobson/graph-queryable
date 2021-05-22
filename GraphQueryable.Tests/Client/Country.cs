using GraphQueryable.Attributes;

namespace GraphQueryable.Tests.Client
{
    public class Country
    {
        [GraphField("name")]
        public string Name { get; set; } 
        
        [GraphField("capital")]
        public string Capital { get; set; }
    }
}