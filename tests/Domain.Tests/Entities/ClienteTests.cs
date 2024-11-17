using Domain.Entities;

namespace Domain.Tests.Entities
{
    public class ClienteTests
    {
        [Fact]
        public void DeveCriarClienteComInformacoesCorretas()
        {
            // Arrange
            var id = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao@teste.com";
            var cpf = "12345678901";
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
    }
}