using Moq;
using QuickFood_Backoffice.Tests.TestHelpers;
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
        var request = FuncionarioFakeDataFactory.CriarFuncionarioIdentifiqueSeRequestValido();

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

    [Fact]
    public async Task Post_CadastrarFuncionarioEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = FuncionarioFakeDataFactory.CriarFuncionarioIdentifiqueSeRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/funcionarios", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao cadastrar funcionário\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/funcionarios", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao cadastrar funcionário", content);
    }
}