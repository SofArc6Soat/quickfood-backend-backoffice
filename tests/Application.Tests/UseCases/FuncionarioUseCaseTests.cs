using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Tests.TestHelpers;
using Gateways;
using Moq;
using UseCases;

namespace Application.Tests.UseCases;

public class FuncionarioUseCaseTests
{
    private readonly Mock<IFuncionarioGateway> _funcionarioGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly IFuncionarioUseCase _funcionarioUseCase;

    public FuncionarioUseCaseTests()
    {
        _funcionarioGatewayMock = new Mock<IFuncionarioGateway>();
        _notificadorMock = new Mock<INotificador>();
        _funcionarioUseCase = new FuncionarioUseCase(_funcionarioGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarTrue_QuandoCadastroBemSucedido()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "senha123";

        _funcionarioGatewayMock.Setup(x => x.VerificarFuncionarioExistente(funcionario.Id, funcionario.Email, It.IsAny<CancellationToken>()))
            .Returns(false);
        _funcionarioGatewayMock.Setup(x => x.CadastrarFuncionarioAsync(funcionario, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _funcionarioUseCase.CadastrarFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarFalse_QuandoFuncionarioJaExistente()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "senha123";

        _funcionarioGatewayMock.Setup(x => x.VerificarFuncionarioExistente(funcionario.Id, funcionario.Email, It.IsAny<CancellationToken>()))
            .Returns(true);

        // Act
        var result = await _funcionarioUseCase.CadastrarFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarFalse_QuandoCadastroFalhar()
    {
        // Arrange
        var funcionario = new Funcionario(FuncionarioFakeDataFactory.ObterGuid(), "Funcionario Exemplo", "funcionario@exemplo.com", true);
        var senha = "senha123";

        _funcionarioGatewayMock.Setup(x => x.VerificarFuncionarioExistente(funcionario.Id, funcionario.Email, It.IsAny<CancellationToken>()))
            .Returns(false);
        _funcionarioGatewayMock.Setup(x => x.CadastrarFuncionarioAsync(funcionario, senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _funcionarioUseCase.CadastrarFuncionarioAsync(funcionario, senha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }
}