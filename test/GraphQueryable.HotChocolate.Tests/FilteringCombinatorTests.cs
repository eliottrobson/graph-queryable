// using System.Linq;
// using GraphQueryable.Tests.Client;
// using GraphQueryable.Visitors.HotChocolate;
// using Xunit;
//
// namespace GraphQueryable.Tests
// {
//     public class FilteringCombinatorTests
//     {
//         [Fact]
//         public void Where_AndTwoPredicates_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code != "FR" && c.Code != "DE").Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("(where: { and: [{ code: { neq: \"FR\" } }, { code: { neq: \"DE\" } }] }) { name }", query);
//         }
//         
//         [Fact]
//         public void Where_OrTwoPredicates_IsTranslated()
//         {
//             // Arrange
//             var countries = new GraphQueryable<Country>("countries");
//             var queryable = countries.Where(c => c.Code == "GB" || c.Code == "FR").Select(c => c.Name);
//
//             // Act
//             var visitor = new HotChocolateConventionVisitor();
//             var context = new GraphQueryContext(visitor);
//             var query = context.Parse(queryable);
//             
//             // Assert
//             Assert.Equal("(where: { or: [{ code: { eq: \"GB\" } }, { code: { eq: \"FR\" } }] }) { name }", query);
//         }
//     }
// }