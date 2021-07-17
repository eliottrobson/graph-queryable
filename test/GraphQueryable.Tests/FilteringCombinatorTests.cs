using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Tests.Client;
using GraphQueryable.Tokens;
using Xunit;

namespace GraphQueryable.Tests
{
    public class FilteringCombinatorTests
    {
        [Fact]
        public void Where_AndTwoPredicates_IsTranslated()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");
            var queryable = countries
                .Where(c => c.Continent.Code == "EU")
                .Where(c => c.Code == "GB");

            // Act
            var context = new GraphQueryContext();
            var countryField = context.Parse(queryable);

            // Assert
            var expected = new Field("countries")
            {
                Filter = new FieldFilterAnd
                {
                    Value = (
                        Left: new FieldFilterEqual<string>
                        {
                            Name = new List<string> {"continent", "code"},
                            Value = "EU"
                        },
                        Right: new FieldFilterEqual<string>
                        {
                            Name = new List<string> {"code"},
                            Value = "GB"
                        }
                    )
                }
            };

            Assert.Equal(expected, countryField);
        }
    }
}