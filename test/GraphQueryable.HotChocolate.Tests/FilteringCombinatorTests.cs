using System.Collections.Generic;
using GraphQueryable.Tokens;
using Xunit;

namespace GraphQueryable.HotChocolate.Tests
{
    public class FilteringCombinatorTests
    {
        [Fact]
        public void Where_AndTwoPredicates_IsTranslated()
        {
            // Arrange
            var field = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name")
                },
                Filters = new List<FieldFilter>
                {
                    new FieldFilterAnd
                    {
                        Name = new List<string>(),
                        Value = (
                            Left: new FieldFilterNotEqual<string>
                            {
                                Name = new List<string> {"code"},
                                Value = "FR"
                            },
                            Right: new FieldFilterNotEqual<string>
                            {
                                Name = new List<string> {"code"},
                                Value = "DE"
                            }
                        )
                    }
                }
            };

            // Act
            var parser = new HotChocolateConventionParser();
            var query = parser.Parse(field);
            
            // Assert
            Assert.Equal("countries(where: { and: [{ code: { neq: \"FR\" } }, { code: { neq: \"DE\" } }] }) { name }", query);
        }
        
        [Fact]
        public void Where_OrTwoPredicates_IsTranslated()
        {
            // Arrange
            var field = new Field("countries")
            {
                Projections = new List<Field>
                {
                    new("name")
                },
                Filters = new List<FieldFilter>
                {
                    new FieldFilterOr
                    {
                        Name = new List<string>(),
                        Value = (
                            Left: new FieldFilterEqual<string>
                            {
                                Name = new List<string> {"code"},
                                Value = "GB"
                            },
                            Right: new FieldFilterEqual<string>
                            {
                                Name = new List<string> {"code"},
                                Value = "FR"
                            }
                        )
                    }
                }
            };
        
            // Act
            var parser = new HotChocolateConventionParser();
            var query = parser.Parse(field);
            
            // Assert
            Assert.Equal("countries(where: { or: [{ code: { eq: \"GB\" } }, { code: { eq: \"FR\" } }] }) { name }", query);
        }
    }
}