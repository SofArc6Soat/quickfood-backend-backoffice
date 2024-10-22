using Gateways;
using Gateways.Cognito;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories
{
    public class ClienteRepositoryTests
    {
        [Fact]
        public async Task DeveRetornarTodosClientesAsync()
        {
            // Arrange
            var mockRepository = new Mock<IClienteRepository>();
            var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
            var mockCognitoGateway = new Mock<ICognitoGateway>();

            var clienteDb = new ClienteDb
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                Cpf = "12345678901",
                Ativo = true
            };
            var clientes = new List<ClienteDb> { clienteDb };

            mockRepository.Setup(r => r.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(clientes);

            var clienteGateway = new ClienteGateway(mockRepository.Object, mockFuncionarioRepository.Object, mockCognitoGateway.Object);

            // Act
            var result = await clienteGateway.ObterTodosClientesAsync(CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(clienteDb.Nome, result.First().Nome);
        }

        [Fact]
        public async Task DeveRetornarClientePorId()
        {
            // Arrange
            var mockRepository = new Mock<IClienteRepository>();
            var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
            var mockCognitoGateway = new Mock<ICognitoGateway>();

            var clienteDb = new ClienteDb
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                Cpf = "12345678901",
                Ativo = true
            };

            mockRepository.Setup(r => r.FindByIdAsync(clienteDb.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(clienteDb);

            var clienteGateway = new ClienteGateway(mockRepository.Object, mockFuncionarioRepository.Object, mockCognitoGateway.Object);

            // Act
            var result = await clienteGateway.VerificarClienteExistenteAsync(clienteDb.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }
    }
}