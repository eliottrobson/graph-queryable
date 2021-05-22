using System.Linq;
using GraphQueryable.Drivers.HotChocolate;
using GraphQueryable.Tests.Client;
using Xunit;

namespace GraphQueryable.Tests
{
    public class ProjectionTests
    {
        [Fact]
        public void Select_Field_IsIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => c.Name);

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { name }", query);
        }
        
        [Fact]
        public void Select_MultipleFields_AreIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { c.Name, c.Capital });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { name, capital }", query);
        }
    }
}