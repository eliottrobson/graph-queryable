using System.Linq;
using GraphQueryable.Tests.Client;
using Xunit;

namespace GraphQueryable.Tests
{
    public class ContextTests
    {
        [Fact]
        public void ScopeName_IsAccessible_FromProvider()
        {
            // Arrange
            var countries = new GraphQueryable<Country>("countries");

            // Assert
            var graphProvider = Assert.IsType<GraphQueryProvider>(countries.Provider);
            Assert.Equal("countries", graphProvider.ScopeName);
        }
    }
}