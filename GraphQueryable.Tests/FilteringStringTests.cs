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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.Equal, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.Equal, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            Assert.Collection(
                stringFilter.Name,
                f => Assert.Equal("continent", f),
                f => Assert.Equal("code", f)
            );
            Assert.Equal(FieldFilterType.Equal, stringFilter.Type);
            Assert.Equal("EU", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.NotEqual, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.StringContains, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.In, stringFilter.Type);
            var stringFilterValue = Assert.IsType<List<object>>(stringFilter.Value);
            Assert.Collection(
                stringFilterValue,
                f => Assert.Equal("GB", f),
                f => Assert.Equal("FR", f)
            );
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.In, stringFilter.Type);
            var stringFilterValue = Assert.IsType<List<object>>(stringFilter.Value);
            Assert.Collection(
                stringFilterValue,
                f => Assert.Equal("GB", f),
                f => Assert.Equal("FR", f)
            );
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.StringStartsWith, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
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
            var stringFilter = Assert.Single(countryField.Filters);
            Assert.NotNull(stringFilter);
            var stringFilterName = Assert.Single(stringFilter.Name);
            Assert.Equal("code", stringFilterName);
            Assert.Equal(FieldFilterType.StringEndsWith, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
        }
    }
}