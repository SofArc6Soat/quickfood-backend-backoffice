using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Validators
{
    public class ValidarResetSenhaTests
    {
        private readonly ValidarResetSenha _validator;

        public ValidarResetSenhaTests() => _validator = new ValidarResetSenha();

        [Fact]
        public void ValidarResetSenha_DevePassarParaDadosValidos()
        {
            // Arrange
            var resetSenha = new ResetSenha("teste@email.com", "123456", "NovaSenha123");

            // Act
            var result = _validator.Validate(resetSenha);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidarResetSenha_DeveFalharParaEmailNulo()
        {
            // Arrange
            var resetSenha = new ResetSenha(null, "123456", "NovaSenha123");

            // Act
            var result = _validator.Validate(resetSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email não pode ser nulo.");
        }

        [Fact]
        public void ValidarResetSenha_DeveFalharParaEmailInvalido()
        {
            // Arrange
            var resetSenha = new ResetSenha("email_invalido", "123456", "NovaSenha123");

            // Act
            var result = _validator.Validate(resetSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "O Email está em um formato inválido.");
        }

        [Fact]
        public void ValidarResetSenha_DeveFalharParaNovaSenhaNula()
        {
            // Arrange
            var resetSenha = new ResetSenha("teste@email.com", "123456", null);

            // Act
            var result = _validator.Validate(resetSenha);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "NovaSenha" && e.ErrorMessage == "O status não pode ser nulo.");
        }
    }
}
