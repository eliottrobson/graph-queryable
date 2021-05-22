using GraphQueryable.Tests.Client;
using Xunit;

namespace GraphQueryable.Tests
{
    public class ContextTests
    {
        [Fact]
        public void QueryName_Is_Passed()
        {
            // Arrange
            var context = new GraphContext();
            var queryable = context.Countries;

            // Assert
            var graphProvider = Assert.IsType<GraphQueryProvider>(queryable.Provider);
            Assert.Equal("countries", graphProvider.MethodName);
        }
    }
}