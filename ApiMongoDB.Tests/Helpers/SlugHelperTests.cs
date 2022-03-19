using ApiMongoDB.Helpers;
using Xunit;

namespace ApiMongoDB.Tests.Helpers
{
    public class SlugHelperTests
    {
        [Fact]
        public void Should_Return_A_Valid_Slud()
        {
            var title = "Teste de % Slúg";

            var slug = SlugHelper.GenerateSlug(title);

            Assert.Equal("teste-de-slug", slug);
        }
    }
}
