using Controllers;
using Domain.Entities;
using Gateways.Dtos.Request;
using Moq;
using UseCases;

namespace QuickFood_Backoffice.Tests.Controllers;

public class FuncionarioControllerTests
{
    private readonly Mock<IFuncionarioUseCase> _funcionarioUseCaseMock;
    private readonly IFuncionarioController _funcionarioController;

    public FuncionarioControllerTests()
    {
        _funcionarioUseCaseMock = new Mock<IFuncionarioUseCase>();
        _funcionarioController = new FuncionarioController(_funcionarioUseCaseMock.Object);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarTrue_QuandoFuncionarioValido()
    {
        // Arrange
        var funcionarioRequestDto = new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario Teste",
            Email = "funcionario@teste.com",
            Senha = "senha123",
            Ativo = true
        };
        _funcionarioUseCaseMock.Setup(x => x.CadastrarFuncionarioAsync(It.IsAny<Funcionario>(), funcionarioRequestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _funcionarioController.CadastrarFuncionarioAsync(funcionarioRequestDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DeveRetornarFalse_QuandoCadastroFalhar()
    {
        // Arrange
        var funcionarioRequestDto = new FuncionarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Funcionario Teste",
            Email = "funcionario@teste.com",
            Senha = "senha123",
            Ativo = true
        };
        _funcionarioUseCaseMock.Setup(x => x.CadastrarFuncionarioAsync(It.IsAny<Funcionario>(), funcionarioRequestDto.Senha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _funcionarioController.CadastrarFuncionarioAsync(funcionarioRequestDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}