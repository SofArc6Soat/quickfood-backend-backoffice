using Gateways.Cognito.Dtos.Request;
using Gateways.Dtos.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
        var request = new ClienteIdentifiqueSeRequestDto
        {
            Cpf = "12345678901",
            Senha = "SenhaTeste123"
        };

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
    public async Task Post_IdentificarFuncionarioEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new FuncinarioIdentifiqueSeRequestDto
        {
            Email = "funcionario@teste.com",
            Senha = "SenhaTeste123"
        };

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
    public async Task Post_ConfirmarEmailVerificaoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new ConfirmarEmailVerificacaoDto
        {
            Email = "usuario@teste.com",
            CodigoVerificacao = "123456"
        };

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
    public async Task Post_SolicitarRecuperacaoSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new SolicitarRecuperacaoSenhaDto
        {
            Email = "usuario@teste.com"
        };

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
    public async Task Post_EfetuarResetSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = new ResetarSenhaDto
        {
            Email = "usuario@teste.com",
            CodigoVerificacao = "123456",
            NovaSenha = "NovaSenhaTeste123"
        };

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
}
