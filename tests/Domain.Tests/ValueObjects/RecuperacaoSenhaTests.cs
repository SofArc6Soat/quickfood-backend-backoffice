using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class RecuperacaoSenhaTests
    {
        [Fact]
        public void RecuperacaoSenha_DeveSerCriadoComSucesso()
        {
            // Arrange
            var email = "teste@email.com";

            // Act
            var recuperacaoSenha = new RecuperacaoSenha(email);

            // Assert
            recuperacaoSenha.Id.Should().NotBeEmpty();
            recuperacaoSenha.Email.Should().Be(email);
        }
    }
}
