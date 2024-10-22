using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class EmailVerificacaoTests
    {
        [Fact]
        public void EmailVerificacao_DeveSerCriadoComSucesso()
        {
            // Arrange
            var email = "teste@email.com";
            var codigoVerificacao = "123456";

            // Act
            var emailVerificacao = new EmailVerificacao(email, codigoVerificacao);

            // Assert
            emailVerificacao.Id.Should().NotBeEmpty();
            emailVerificacao.Email.Should().Be(email);
            emailVerificacao.CodigoVerificacao.Should().Be(codigoVerificacao);
        }
    }
}
