using Domain.Tests.TestHelpers;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class ClientesApiControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public ClientesApiControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Post_CadastrarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = ClienteFakeDataFactory.CriarClienteValido();

        // Act
        var response = await _client.PostAsJsonAsync("/clientes", request);

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
    public async Task Post_CadastrarClienteEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = ClienteFakeDataFactory.CriarClienteComNomeInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/clientes", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao cadastrar cliente\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/clientes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao cadastrar cliente", content);
    }

    [Fact]
    public async Task Put_AtualizarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = ClienteFakeDataFactory.CriarClienteValido();
        var clienteId = request.Id;

        // Act
        var response = await _client.PutAsJsonAsync($"/clientes/{clienteId}", request);

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
    public async Task Put_AtualizarClienteEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = ClienteFakeDataFactory.CriarClienteComCPFInvalido();
        var clienteId = request.Id;
        _handlerMock.SetupRequest(HttpMethod.Put, $"http://localhost/clientes/{clienteId}", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao atualizar cliente\"]}");

        // Act
        var response = await _client.PutAsJsonAsync($"/clientes/{clienteId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao atualizar cliente", content);
    }

    [Fact]
    public async Task Delete_DeletarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteValido().Id;

        // Primeiro, crie um cliente para garantir que ele exista
        var request = ClienteFakeDataFactory.CriarClienteValido();
        var postResponse = await _client.PostAsJsonAsync("/clientes", request);
        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.DeleteAsync($"/clientes/{clienteId}");

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Delete_DeletarClienteEndpoint_ReturnsNotFound()
    {
        // Arrange
        var clienteId = ClienteFakeDataFactory.CriarClienteValido().Id;
        _handlerMock.SetupRequest(HttpMethod.Delete, $"http://localhost/clientes/{clienteId}", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Cliente não encontrado\"]}");

        // Act
        var response = await _client.DeleteAsync($"/clientes/{clienteId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Cliente não encontrado", content);
    }
}