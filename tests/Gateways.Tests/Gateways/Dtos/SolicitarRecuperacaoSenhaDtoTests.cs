using Gateways.Cognito.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Tests.Gateways.Dtos;

public class SolicitarRecuperacaoSenhaDtoTests
{
    private IList<ValidationResult> ValidateModel(SolicitarRecuperacaoSenhaDto model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Null()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = null };
        var result = ValidateModel(model);
        Assert.Contains(result, v => v.ErrorMessage == "O campo E-mail é obrigatório.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "invalid-email" };
        var result = ValidateModel(model);
        Assert.Contains(result, v => v.ErrorMessage == "E-mail está em um formato inválido.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Too_Short()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "a@b" }; // Menos de 5 caracteres
        var result = ValidateModel(model);
        Assert.Contains(result, v => v.ErrorMessage == "O campo E-mail deve conter entre 5 e 100 caracteres.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Too_Long()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = new string('a', 101) + "@example.com" };
        var result = ValidateModel(model);
        Assert.Contains(result, v => v.ErrorMessage == "O campo E-mail deve conter entre 5 e 100 caracteres.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "valid.email@example.com" };
        var result = ValidateModel(model);
        Assert.DoesNotContain(result, v => v.ErrorMessage == "O campo E-mail é obrigatório.");
        Assert.DoesNotContain(result, v => v.ErrorMessage == "E-mail está em um formato inválido.");
        Assert.DoesNotContain(result, v => v.ErrorMessage == "O campo E-mail deve conter entre 5 e 100 caracteres.");
    }
}
