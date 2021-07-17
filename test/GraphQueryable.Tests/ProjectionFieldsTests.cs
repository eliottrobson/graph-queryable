using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Tests.Client;
using GraphQueryable.Tokens;
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

            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name")
                }
            };
            
            // Assert
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("code"),
                    new("name")
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("code"),
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name"),
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("code"),
                    new("name"),
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("code"),
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name")
                }
            };
            
            Assert.Equal(expected, countryField);
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
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name"),
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Select_MultipleDeepDuplicateFields_AppearsOnce()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Select(c => new {Name1 = c.Continent.Name, Name2 = c.Continent.Name});

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("continent")
                    {
                        Projections = new List<Field>
                        {
                            new("name")
                        }
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
        }
    }
}