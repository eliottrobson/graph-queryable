// using System.Collections.Generic;
// using System.Linq;
// using GraphQueryable.Tests.Client;
// using GraphQueryable.Visitors.HotChocolate;
// using Xunit;
//
// namespace GraphQueryable.Tests
// {
//     public class FilteringStringTests
//     {
//         [Fact]
//         public void Where_StringEquals_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code == "GB").Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { eq: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_StringNotEquals_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code != "GB").Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { neq: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotStringEquals_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             // ReSharper disable once NegativeEqualityExpression
//             // Part of test to check negation works as expected
//             var queryable = countries.Where(c => !(c.Code == "GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { neq: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_StringContains_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code.Contains("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { contains: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotStringContains_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => !c.Code.Contains("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { ncontains: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_StringIn_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var items = new[] {"GB", "FR"};
//             var queryable = countries.Where(c => items.Contains(c.Code)).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { in: [\"GB\", \"FR\"] } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NestedStringIn_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => new[] {"GB", "FR"}.Contains(c.Code)).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { in: [\"GB\", \"FR\"] } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotStringIn_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var items = new[] {"GB", "FR"};
//             var queryable = countries.Where(c => !items.Contains(c.Code)).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { nin: [\"GB\", \"FR\"] } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotNestedStringIn_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => !new[] {"GB", "FR"}.Contains(c.Code)).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { nin: [\"GB\", \"FR\"] } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_StringStartsWith_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code.StartsWith("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { startsWith: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotStringStartsWith_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => !c.Code.StartsWith("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { nstartsWith: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_StringEndsWith_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code.EndsWith("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { endsWith: \"GB\" } }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_NotStringEndsWith_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => !c.Code.EndsWith("GB")).Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("countries(where: { code: { nendsWith: \"GB\" } }) { name }", query);
//         }
//     }
// }