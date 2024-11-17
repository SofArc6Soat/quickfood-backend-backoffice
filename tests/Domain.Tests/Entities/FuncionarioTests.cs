using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities
{
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
    }
}
