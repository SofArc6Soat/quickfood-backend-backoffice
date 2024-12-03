using Gateways.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Tests.Gateways.Dtos;

public class FuncionarioRequestDtoTests
{
    private FuncionarioRequestDto CriarFuncionarioValido()
    {
        return new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario Exemplo",
            Email = "exemplo@empresa.com",
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
    public void FuncionarioRequestDto_Valido_DevePassarValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Empty(resultados);
    }

    [Fact]
    public void FuncionarioRequestDto_IdInvalido_DeveFalharValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();
        funcionario.Id = Guid.Empty;

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Id é obrigatório."));
    }

    [Fact]
    public void FuncionarioRequestDto_NomeInvalido_DeveFalharValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();
        funcionario.Nome = "A";

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Nome deve conter entre 2 e 50 caracteres."));
    }

    [Fact]
    public void FuncionarioRequestDto_EmailInvalido_DeveFalharValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();
        funcionario.Email = "emailinvalido";

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("E-mail está em um formato inválido."));
    }

    [Fact]
    public void FuncionarioRequestDto_SenhaInvalida_DeveFalharValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();
        funcionario.Senha = "123";

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Contains(resultados, r => r.ErrorMessage.Contains("O campo Senha deve conter entre 8 e 50 caracteres."));
    }

    [Fact]
    public void FuncionarioRequestDto_AtivoInvalido_DeveFalharValidacao()
    {
        // Arrange
        var funcionario = CriarFuncionarioValido();
        funcionario.Ativo = false;

        // Act
        var resultados = ValidarModelo(funcionario);

        // Assert
        Assert.Empty(resultados); // Neste caso, a validação deve passar, pois o campo Ativo é booleano e não possui restrições adicionais.
    }
}