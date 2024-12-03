using Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Domain.Tests.Entities;

public class FuncionarioTests
{
    [Fact]
    public void Funcionario_DeveSerCriadoComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "João";
        var email = "joao@email.com";
        var ativo = true;

        // Act
        var funcionario = new Funcionario(id, nome, email, ativo);

        // Assert
        funcionario.Id.Should().Be(id);
        funcionario.Nome.Should().Be(nome);
        funcionario.Email.Should().Be(email);
        funcionario.Ativo.Should().Be(ativo);
    }

    [Fact]
    public void DeveValidarFuncionarioComInformacoesCorretas()
    {
        // Arrange
        var funcionario = new Funcionario(Guid.NewGuid(), "João Silva", "joao@teste.com", true);
        var validator = new ValidarFuncionario();

        // Act
        var result = validator.TestValidate(funcionario);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeveRetornarErroParaFuncionarioInvalido()
    {
        // Arrange
        var funcionario = new Funcionario(Guid.Empty, "J", "email_invalido", false);
        var validator = new ValidarFuncionario();

        // Act
        var result = validator.TestValidate(funcionario);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.Nome);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
}