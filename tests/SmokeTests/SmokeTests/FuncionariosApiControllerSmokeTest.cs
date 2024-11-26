using Gateways.Dtos.Request;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class FuncionariosApiControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public FuncionariosApiControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Post_CadastrarFuncionarioEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario Teste",
            Email = "funcionario@teste.com",
            Senha = "SenhaTeste123",
            Ativo = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/funcionarios", request);

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
}