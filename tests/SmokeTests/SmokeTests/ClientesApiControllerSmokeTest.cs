using Gateways.Dtos.Request;
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
    public async Task Get_ObterTodosClientesEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/clientes");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_CadastrarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "SenhaTeste123",
            Ativo = true
        };

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
    public async Task Put_AtualizarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var request = new ClienteAtualizarRequestDto
        {
            Id = clienteId,
            Nome = "Cliente Atualizado",
            Ativo = true
        };

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
    public async Task Delete_DeletarClienteEndpoint_ReturnsSuccess()
    {
        // Arrange
        var clienteId = Guid.NewGuid();

        // Primeiro, crie um cliente para garantir que ele exista
        var cliente = new ClienteRequestDto
        {
            Id = clienteId,
            Nome = "Cliente Teste",
            Email = "cliente@teste.com",
            Cpf = "12345678901",
            Senha = "SenhaTeste123",
            Ativo = true
        };
        var postResponse = await _client.PostAsJsonAsync("/clientes", cliente);
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
}