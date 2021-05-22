using System.Linq;
using GraphQueryable.Attributes;
using GraphQueryable.Drivers;

namespace GraphQueryable.Tests.Client
{
    public class Country
    {
        [GraphField("name")]
        public string Name { get; set; } 
        
        [GraphField("capital")]
        public string Capital { get; set; }
    }
    
    public class GraphContext
    {
        public IQueryable<Country> Countries => new GraphQueryable<Country>("countries", new HotChocolateConventionVisitor());
    }
}