using System.Linq;
using GraphQueryable.Tests.Client;
using GraphQueryable.Tokens;
using Xunit;

namespace GraphQueryable.Tests
{
    public class FilteringStringTests
    {
        [Fact]
        public void Where_StringEquals_IsTranslated()
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
            Assert.Equal("code", stringFilter.Field.Name);
            Assert.Equal(FieldFilterType.Equal, stringFilter.Type);
            Assert.Equal("GB", stringFilter.Value);
        }
    }
}