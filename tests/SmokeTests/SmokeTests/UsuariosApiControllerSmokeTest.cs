using Moq;
using QuickFood_Backoffice.Tests.TestHelpers;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class UsuariosApiControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public UsuariosApiControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Post_IdentificarClienteCpfEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarClienteIdentifiqueSeRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/cliente/identifique-se", request);

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
    public async Task Post_IdentificarClienteCpfEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarClienteIdentifiqueSeRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/cliente/identifique-se", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao identificar cliente\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/cliente/identifique-se", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao identificar cliente", content);
    }

    [Fact]
    public async Task Post_IdentificarFuncionarioEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = FuncionarioFakeDataFactory.CriarFuncionarioIdentifiqueSeRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/funcionario/identifique-se", request);

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
    public async Task Post_IdentificarFuncionarioEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = FuncionarioFakeDataFactory.CriarFuncionarioIdentifiqueSeRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/funcionario/identifique-se", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao identificar funcionário\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/funcionario/identifique-se", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao identificar funcionário", content);
    }

    [Fact]
    public async Task Post_ConfirmarEmailVerificaoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarConfirmarEmailVerificacaoRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/email-verificacao:confirmar", request);

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
    public async Task Post_ConfirmarEmailVerificaoEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarConfirmarEmailVerificacaoRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/email-verificacao:confirmar", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao confirmar email\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/email-verificacao:confirmar", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao confirmar email", content);
    }

    [Fact]
    public async Task Post_SolicitarRecuperacaoSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarSolicitarRecuperacaoSenhaRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/esquecia-senha:solicitar", request);

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
    public async Task Post_SolicitarRecuperacaoSenhaEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarSolicitarRecuperacaoSenhaRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/esquecia-senha:solicitar", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao solicitar recuperação de senha\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/esquecia-senha:solicitar", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao solicitar recuperação de senha", content);
    }

    [Fact]
    public async Task Post_EfetuarResetSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarResetarSenhaRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/esquecia-senha:resetar", request);

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
    public async Task Post_EfetuarResetSenhaEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarResetarSenhaRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/esquecia-senha:resetar", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao resetar senha\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/esquecia-senha:resetar", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao resetar senha", content);
    }
}