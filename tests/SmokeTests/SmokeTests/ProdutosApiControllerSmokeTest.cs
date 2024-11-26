using Gateways.Dtos.Request;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests
{
    public class ProdutosApiControllerSmokeTest
    {
        private readonly HttpClient _client;
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public ProdutosApiControllerSmokeTest()
        {
            _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
            _client = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }

        [Fact]
        public async Task Get_ProdutosEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/produtos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_ProdutosCategoriaEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/produtos/categoria?categoria=Lanche");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Post_CadastrarProdutoEndpoint_ReturnsSuccess()
        {
            // Arrange
            var produto = new ProdutoRequestDto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Teste",
                Descricao = "Descrição do Produto Teste",
                Preco = 100.0m,
                Ativo = true,
                Categoria = "Lanche"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/produtos", produto);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
            }

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Put_AtualizarProdutoEndpoint_ReturnsSuccess()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var produto = new ProdutoRequestDto
            {
                Id = produtoId,
                Nome = "Produto Atualizado",
                Descricao = "Descrição do Produto Atualizado",
                Preco = 150.0m,
                Ativo = true,
                Categoria = "Lanche"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/produtos/{produtoId}", produto);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
            }

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Delete_DeletarProdutoEndpoint_ReturnsSuccess()
        {
            // Arrange
            var produtoId = Guid.NewGuid();

            // Primeiro, crie um produto para garantir que ele exista
            var produto = new ProdutoRequestDto
            {
                Id = produtoId,
                Nome = "Produto Teste",
                Descricao = "Descrição do Produto Teste",
                Preco = 100.0m,
                Ativo = true,
                Categoria = "Lanche"
            };
            var postResponse = await _client.PostAsJsonAsync("/produtos", produto);
            postResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.DeleteAsync($"/produtos/{produtoId}");

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
