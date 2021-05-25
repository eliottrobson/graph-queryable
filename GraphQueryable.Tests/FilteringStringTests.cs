using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Tests.Client;
using GraphQueryable.Tokens;
using Xunit;

namespace GraphQueryable.Tests
{
    public class FilteringStringTests
    {
        [Fact]
        public void Where_FieldEqualsString_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => c.Code == "GB");

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.Equal,
                        Value = "GB"
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_StringEqualsField_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => "GB" == c.Code);

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.Equal,
                        Value = "GB"
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_NestedFieldEqualsString_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => c.Continent.Code == "EU");

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"continent", "code"},
                        Type = FieldFilterType.Equal,
                        Value = "EU"
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_StringNotEqualsField_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => "GB" != c.Code);

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.NotEqual,
                        Value = "GB"
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_FieldContainsString_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => c.Code.Contains("GB"));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.StringContains,
                        Value = "GB"
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }


        [Fact]
        public void Where_StringsContainsField_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var items = new List<string> {"GB", "FR"};
            var queryable = countries.Where(c => items.Contains(c.Code));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.In,
                        Value = new List<string> {"GB", "FR"}
                    }
                }
            };

            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_NestedStringsContainsField_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => new List<string> {"GB", "FR"}.Contains(c.Code));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.In,
                        Value = new List<string> {"GB", "FR"}
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
        }


        [Fact]
        public void Where_StringStartsWith_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => c.Code.StartsWith("GB"));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.StringStartsWith,
                        Value = "GB"
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
        }

        [Fact]
        public void Where_StringEndsWith_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => c.Code.EndsWith("GB"));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filters = new List<FieldFilter>
                {
                    new()
                    {
                        Name = new List<string> {"code"},
                        Type = FieldFilterType.StringEndsWith,
                        Value = "GB"
                    }
                }
            };
            
            Assert.Equal(expected, countryField);
        }
    }
}