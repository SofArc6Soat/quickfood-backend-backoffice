using Moq;
using QuickFood_Backoffice.Tests.TestHelpers;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

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
    public async Task Get_ProdutosEndpoint_ReturnsNotFound()
    {
        // Arrange
        _handlerMock.SetupRequest(HttpMethod.Get, "http://localhost/produtos", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Produtos não encontrados\"]}");

        // Act
        var response = await _client.GetAsync("/produtos");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Produtos não encontrados", content);
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
    public async Task Get_ProdutosCategoriaEndpoint_ReturnsNotFound()
    {
        // Arrange
        _handlerMock.SetupRequest(HttpMethod.Get, "http://localhost/produtos/categoria?categoria=Lanche", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Categoria não encontrada\"]}");

        // Act
        var response = await _client.GetAsync("/produtos/categoria?categoria=Lanche");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Categoria não encontrada", content);
    }

    [Fact]
    public async Task Post_CadastrarProdutoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();

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
    public async Task Post_CadastrarProdutoEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/produtos", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao cadastrar produto\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/produtos", produto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao cadastrar produto", content);
    }

    [Fact]
    public async Task Put_AtualizarProdutoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.AlterarProdutoValido();

        // Act
        var response = await _client.PutAsJsonAsync($"/produtos/{produto.Id}", produto);

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
    public async Task Put_AtualizarProdutoEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoInvalido();
        _handlerMock.SetupRequest(HttpMethod.Put, $"http://localhost/produtos/{produto.Id}", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao atualizar produto\"]}");

        // Act
        var response = await _client.PutAsJsonAsync($"/produtos/{produto.Id}", produto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao atualizar produto", content);
    }

    [Fact]
    public async Task Delete_DeletarProdutoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();

        // Primeiro, crie um produto para garantir que ele exista
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();
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

    [Fact]
    public async Task Delete_DeletarProdutoEndpoint_ReturnsNotFound()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();
        _handlerMock.SetupRequest(HttpMethod.Delete, $"http://localhost/produtos/{produtoId}", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Produto não encontrado\"]}");

        // Act
        var response = await _client.DeleteAsync($"/produtos/{produtoId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Produto não encontrado", content);
    }
}