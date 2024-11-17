using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Validators
{
    public class ValidarEmailVerificacaoTests
    {
        private readonly ValidarEmailVerificacao _validator;

        public ValidarEmailVerificacaoTests() => _validator = new ValidarEmailVerificacao();

        [Fact]
        public void ValidarEmailVerificacao_DevePassarParaDadosValidos()
        {
            // Arrange
            var emailVerificacao = new EmailVerificacao("teste@email.com", "123456");

            // Act
            var result = _validator.Validate(emailVerificacao);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidarEmailVerificacao_DeveFalharParaEmailNulo()
        {
            // Arrange
            var emailVerificacao = new EmailVerificacao(null, "123456");

            // Act
            var result = _validator.Validate(emailVerificacao);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email não pode ser nulo.");
        }

        [Fact]
        public void ValidarEmailVerificacao_DeveFalharParaEmailInvalido()
        {
            // Arrange
            var emailVerificacao = new EmailVerificacao("email_invalido", "123456");

            // Act
            var result = _validator.Validate(emailVerificacao);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email está em um formato inválido.");
        }

        [Fact]
        public void ValidarEmailVerificacao_DeveFalharParaCodigoVerificacaoNulo()
        {
            // Arrange
            var emailVerificacao = new EmailVerificacao("teste@email.com", null);

            // Act
            var result = _validator.Validate(emailVerificacao);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "CodigoVerificacao" && e.ErrorMessage == "O status não pode ser nulo.");
        }
    }
}
