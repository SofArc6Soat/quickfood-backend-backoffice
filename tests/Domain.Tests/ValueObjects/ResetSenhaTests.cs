using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class ResetSenhaTests
    {
        [Fact]
        public void ResetSenha_DeveSerCriadoComSucesso()
        {
            // Arrange
            var email = "teste@email.com";
            var codigoVerificacao = "123456";
            var novaSenha = "NovaSenha123";

            // Act
            var resetSenha = new ResetSenha(email, codigoVerificacao, novaSenha);

            // Assert
            resetSenha.Id.Should().NotBeEmpty();
            resetSenha.Email.Should().Be(email);
            resetSenha.CodigoVerificacao.Should().Be(codigoVerificacao);
            resetSenha.NovaSenha.Should().Be(novaSenha);
        }
    }
}
