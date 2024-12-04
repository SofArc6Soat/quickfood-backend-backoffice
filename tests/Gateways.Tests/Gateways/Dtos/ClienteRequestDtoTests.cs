using Gateways.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Tests.Gateways.Dtos;

public class ClienteRequestDtoTests
{
    private ClienteRequestDto CriarClienteValido()
    {
        return new ClienteRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Exemplo",
            Email = "exemplo@empresa.com",
            Cpf = "12345678901",
            Senha = "SenhaSegura123",
            Ativo = true
        };
    }

    private List<ValidationResult> ValidarModelo(object modelo)
    {
        var contexto = new ValidationContext(modelo, null, null);
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(modelo, contexto, resultados, true);
        return resultados;
    }

    [Fact]
    public void ClienteRequestDto_Valido_DevePassarValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Empty(resultados);
    }

    [Fact]
    public void ClienteRequestDto_IdInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Id = Guid.Empty;

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Id é obrigatório."));
    }

    [Fact]
    public void ClienteRequestDto_NomeInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Nome = "A";

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Nome deve conter entre 2 e 50 caracteres."));
    }

    [Fact]
    public void ClienteRequestDto_EmailInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Email = "emailinvalido";

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("E-mail está em um formato inválido."));
    }

    [Fact]
    public void ClienteRequestDto_CpfInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Cpf = "123";

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo CPF deve conter 11 caracteres."));
    }

    [Fact]
    public void ClienteRequestDto_SenhaInvalida_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Senha = "123";

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Senha deve conter entre 8 e 50 caracteres."));
    }

    [Fact]
    public void ClienteRequestDto_AtivoInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Ativo = false;

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Empty(resultados); // Neste caso, a validação deve passar, pois o campo Ativo é booleano e não possui restrições adicionais.
    }
}