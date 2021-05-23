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
            var queryable = countries.Select(c => new { c.Code, c.Name });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { code, name }", query);
        }

        [Fact]
        public void Select_DeepField_IsIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => c.Continent.Name);

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { continent { name } }", query);
        }
        
        [Fact]
        public void Select_MultipleDeepFields_AreIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { c.Continent.Code, c.Continent.Name });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { continent { code, name } }", query);
        }
        
        [Fact]
        public void Select_MixedDeepFields_AreIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { Country = c.Name, Continent = c.Continent.Name });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { name, continent { name } }", query);
        }
        
        [Fact]
        public void Select_MultipleMixedDeepFields_AreIncluded()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { c.Code, c.Name, Continent = new { c.Continent.Code, c.Continent.Name } });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { code, name, continent { code, name } }", query);
        }
        
        [Fact]
        public void Select_FieldsOrder_IsFixed()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { c.Name, c.Code });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { code, name }", query);
        }
        
        [Fact]
        public void Select_MultipleMixedDeepFieldsOrder_IsFixed()
        {
            // Arrange
            var visitor = new HotChocolateConventionVisitor();
            var countries = new GraphQueryable<Country>("countries", visitor);
            var queryable = countries.Select(c => new { Continent = new { c.Continent.Name, c.Continent.Code }, c.Name, c.Code });

            // Act
            var context = new GraphQueryContext(visitor);
            var query = context.Parse(queryable);
            
            // Assert
            Assert.Equal("countries { code, name, continent { code, name } }", query);
        }
    }
}