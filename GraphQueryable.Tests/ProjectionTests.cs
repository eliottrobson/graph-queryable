using System.Linq;
using GraphQueryable.Drivers;
using GraphQueryable.Tests.Client;
using Xunit;

namespace GraphQueryable.Tests
{
    class CountryProjection
    {
        public string Name { get; set; }
    }
    
    public class ProjectionTests
    {
        [Fact]
        public void Projection_Fields_Are_Parsed()
        {
            // Arrange
            var graph = new GraphContext();

            var queryable = graph.Countries
                .Select(c => new CountryProjection { Name = c.Name });

            // Act
            var visitor = new HotChocolateConventionVisitor();
            var context = new GraphQueryContext(visitor);
            context.Parse(queryable.Expression);
            
            // Assert
            Assert.Equal("countries { name }", context.Query);
        }
    }
}