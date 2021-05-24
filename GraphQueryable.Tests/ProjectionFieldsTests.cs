using System.Linq;
using GraphQueryable.Tests.Client;
using Xunit;

namespace GraphQueryable.Tests
{
    public class ProjectionFieldsTests
    {
        [Fact]
        public void Select_Field_IsIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => c.Name);

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            Assert.Equal("countries", countryField.Name);
            var countryNameField = Assert.Single(countryField.Children);
            Assert.NotNull(countryNameField);
            Assert.Equal("name", countryNameField.Name);
        }

        [Fact]
        public void Select_MultipleFields_AreIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {c.Code, c.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            Assert.Equal(2, countryField.Children.Count);
            Assert.Single(countryField.Children, f => f.Name == "code");
            Assert.Single(countryField.Children, f => f.Name == "name");
        }

        [Fact]
        public void Select_DeepField_IsIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => c.Continent.Name);

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var continentField = Assert.Single(countryField.Children);
            Assert.NotNull(continentField);
            Assert.Equal("continent", continentField.Name);
            var nameField = Assert.Single(continentField.Children);
            Assert.NotNull(nameField);
            Assert.Equal("name", nameField.Name);
        }

        [Fact]
        public void Select_MultipleDeepFields_AreIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {c.Continent.Code, c.Continent.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var continentField = Assert.Single(countryField.Children);
            Assert.NotNull(continentField);
            Assert.Equal("continent", continentField.Name);
            Assert.Equal(2, continentField.Children.Count);
            Assert.Single(continentField.Children, f => f.Name == "code");
            Assert.Single(continentField.Children, f => f.Name == "name");
        }

        [Fact]
        public void Select_MixedDeepFields_AreIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {Country = c.Name, Continent = c.Continent.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            Assert.Equal(2, countryField.Children.Count);
            Assert.Single(countryField.Children, f => f.Name == "name");
            var continentField = Assert.Single(countryField.Children, c => c.Name == "continent");
            Assert.NotNull(continentField);
            var continentNameField = Assert.Single(continentField.Children);
            Assert.NotNull(continentNameField);
            Assert.Equal("name", continentNameField.Name);
        }

        [Fact]
        public void Select_MultipleMixedDeepFields_AreIncluded()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new
                {c.Code, c.Name, Continent = new {c.Continent.Code, c.Continent.Name}});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            Assert.Equal(3, countryField.Children.Count);
            Assert.Single(countryField.Children, f => f.Name == "code");
            Assert.Single(countryField.Children, f => f.Name == "name");
            var continentField = Assert.Single(countryField.Children, c => c.Name == "continent");
            Assert.NotNull(continentField);
            Assert.Equal(2, continentField.Children.Count);
            Assert.Single(continentField.Children, f => f.Name == "code");
            Assert.Single(continentField.Children, f => f.Name == "name");
        }

        [Fact]
        public void Select_DuplicateField_AppearsOnce()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {Name1 = c.Name, Name2 = c.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var countryNameField = Assert.Single(countryField.Children);
            Assert.NotNull(countryNameField);
            Assert.Equal("name", countryNameField.Name);
        }

        [Fact]
        public void Select_DeepDuplicateFields_AppearsOnce()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new
            {
                Name1 = c.Name, Name2 = c.Name,
                Continent = new {Name1 = c.Continent.Name, Name2 = c.Continent.Name}
            });

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            Assert.Equal(2, countryField.Children.Count);
            Assert.Single(countryField.Children, f => f.Name == "name");
            var continentField = Assert.Single(countryField.Children, f => f.Name == "continent");
            Assert.NotNull(continentField);
            var continentNameField = Assert.Single(continentField.Children);
            Assert.NotNull(continentNameField);
            Assert.Equal("name", continentNameField.Name);
        }

        [Fact]
        public void Select_MultipleMixedDeepDuplicateFields_AppearsOnce()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {Name1 = c.Continent.Name, Name2 = c.Continent.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var continentField = Assert.Single(countryField.Children);
            Assert.NotNull(continentField);
            Assert.Equal("continent", continentField.Name);
            var continentNameField = Assert.Single(continentField.Children);
            Assert.NotNull(continentNameField);
            Assert.Equal("name", continentNameField.Name);
        }
    }
}