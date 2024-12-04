using Domain.Entities;
using FluentValidation.TestHelper;

namespace Domain.Tests.Entities;

public class ClienteTests
{
    [Fact]
    public void DeveCriarClienteComInformacoesCompletas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "João Silva";
        var email = "joao@teste.com";
        var cpf = "12345678909"; // Certifique-se de que este CPF é válido
        var ativo = true;

        // Act
        var cliente = new Cliente(id, nome, email, cpf, ativo);

        // Assert
        Assert.Equal(id, cliente.Id);
        Assert.Equal(nome, cliente.Nome);
        Assert.Equal(email, cliente.Email);
        Assert.Equal(cpf, cliente.Cpf);
        Assert.True(cliente.Ativo);
    }

    [Fact]
    public void DeveCriarClienteComInformacoesMinimas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Maria";
        var ativo = true;

        // Act
        var cliente = new Cliente(id, nome, ativo);

        // Assert
        Assert.Equal(id, cliente.Id);
        Assert.Equal(nome, cliente.Nome);
        Assert.Equal(string.Empty, cliente.Email);
        Assert.Equal(string.Empty, cliente.Cpf);
        Assert.True(cliente.Ativo);
    }

    [Fact]
    public void DeveValidarClienteComInformacoesCompletas()
    {
        // Arrange
        var cliente = new Cliente(Guid.NewGuid(), "João Silva", "joao@teste.com", "12345678909", true); // CPF válido
        var validator = new ValidarCliente();

        // Act
        var result = validator.TestValidate(cliente);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeveValidarClienteComInformacoesMinimas()
    {
        // Arrange
        var cliente = new Cliente(Guid.NewGuid(), "Maria", true);
        var validator = new ValidarCliente();

        // Act
        var result = validator.TestValidate(cliente);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.Cpf);
    }

    [Fact]
    public void DeveRetornarErroParaClienteInvalido()
    {
        // Arrange
        var cliente = new Cliente(Guid.Empty, "J", "email_invalido", "123", false);
        var validator = new ValidarCliente();

        // Act
        var result = validator.TestValidate(cliente);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.Nome);
        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.Cpf);
    }
}
