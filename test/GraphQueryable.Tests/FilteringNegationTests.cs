using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Tests.Client;
using GraphQueryable.Tokens;
using Xunit;

namespace GraphQueryable.Tests
{
    public class FilteringNegationTests
    {
        [Fact]
        public void Where_NotFieldEqualsString_IsNegated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => !(c.Code == "GB"));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filter = new FieldFilterNotEqual<string>
                {
                    Name = new List<string> {"code"},
                    Value = "GB"
                }
            };

            Assert.Equal(expected, countryField);
        }
        
        [Fact]
        public void Where_NotFieldContainsString_IsNegated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries.Where(c => !c.Code.Contains("GB"));

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filter = new FieldFilterNotContains<string>
                {
                    Name = new List<string> {"code"},
                    Value = "GB"
                }
            };

            Assert.Equal(expected, countryField);
        }
    }
}