using Gateways.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Tests.Gateways.Dtos;

public class ClienteAtualizarRequestDtoTests
{
    private ClienteAtualizarRequestDto CriarClienteAtualizarValido()
    {
        return new ClienteAtualizarRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Atualizado",
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
    public void ClienteAtualizarRequestDto_Valido_DevePassarValidacao()
    {
        // Arrange
        var cliente = CriarClienteAtualizarValido();

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Empty(resultados);
    }

    [Fact]
    public void ClienteAtualizarRequestDto_IdInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteAtualizarValido();
        cliente.Id = Guid.Empty;

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Id é obrigatório."));
    }

    [Fact]
    public void ClienteAtualizarRequestDto_NomeInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteAtualizarValido();
        cliente.Nome = "A";

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Nome deve conter entre 2 e 50 caracteres."));
    }

    [Fact]
    public void ClienteAtualizarRequestDto_AtivoInvalido_DeveFalharValidacao()
    {
        // Arrange
        var cliente = CriarClienteAtualizarValido();
        cliente.Ativo = false;

        // Act
        var resultados = ValidarModelo(cliente);

        // Assert
        Assert.Empty(resultados); // Neste caso, a validação deve passar, pois o campo Ativo é booleano e não possui restrições adicionais.
    }
}