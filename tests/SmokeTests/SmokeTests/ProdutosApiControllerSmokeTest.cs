namespace SmokeTests.SmokeTests
{
    public class ProdutosApiControllerSmokeTest(SmokeTestStartup factory) : IClassFixture<SmokeTestStartup>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Get_ProdutosEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/produtos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_ProdutosCategoriaEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/produtos/categoria?categoria=Lanche");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
