using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Validators
{
    public class ValidarSolicitacaoRecuperacaoSenhaTests
    {
        private readonly ValidarSolicitacaoRecuperacaoSenha _validator;

        public ValidarSolicitacaoRecuperacaoSenhaTests() => _validator = new ValidarSolicitacaoRecuperacaoSenha();

        [Fact]
        public void ValidarSolicitacaoRecuperacaoSenha_DevePassarParaDadosValidos()
        {
            // Arrange
            var recuperacaoSenha = new RecuperacaoSenha("teste@email.com");

            // Act
            var result = _validator.Validate(recuperacaoSenha);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidarSolicitacaoRecuperacaoSenha_DeveFalharParaEmailNulo()
        {
            // Arrange
            var recuperacaoSenha = new RecuperacaoSenha(null);

            // Act
            var result = _validator.Validate(recuperacaoSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email não pode ser nulo.");
        }

        [Fact]
        public void ValidarSolicitacaoRecuperacaoSenha_DeveFalharParaEmailInvalido()
        {
            // Arrange
            var recuperacaoSenha = new RecuperacaoSenha("email_invalido");

            // Act
            var result = _validator.Validate(recuperacaoSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email está em um formato inválido.");
        }

        [Fact]
        public void ValidarSolicitacaoRecuperacaoSenha_DeveFalharParaEmailLongoDemais()
        {
            // Arrange
            var email = new string('a', 101) + "@dominio.com";
            var recuperacaoSenha = new RecuperacaoSenha(email);

            // Act
            var result = _validator.Validate(recuperacaoSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("O Email precisa ter entre 5 e 100 caracteres"));
        }
    }
}
