using Domain.Entities;
using Domain.Tests.TestHelpers;
using FluentValidation.TestHelper;

namespace Domain.Tests.Validators
{
    public class ValidarClienteTests
    {
        private readonly ValidarCliente _validarCliente;

        public ValidarClienteTests() => _validarCliente = new ValidarCliente();

        [Fact]
        public void DeveSerValido_QuandoInformacoesCorretas()
        {
            // Arrange
            var cliente = ClienteFakeDataFactory.CriarClienteValido();

            // Act
            var result = _validarCliente.TestValidate(cliente);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void DeveSerInvalido_QuandoNomeInvalido()
        {
            // Arrange
            var cliente = ClienteFakeDataFactory.CriarClienteComNomeInvalido();

            // Act
            var result = _validarCliente.TestValidate(cliente);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Nome);
        }

        [Fact]
        public void DeveSerInvalido_QuandoCpfInvalido()
        {
            // Arrange
            var cliente = new Cliente(Guid.NewGuid(), "João Silva", "joao@teste.com", "12345678900", true);

            // Act
            var result = _validarCliente.TestValidate(cliente);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Cpf);
        }

        [Fact]
        public void DeveSerInvalido_QuandoCpfTemDigitosRepetidos()
        {
            // Arrange
            var cliente = new Cliente(Guid.NewGuid(), "João Silva", "joao@teste.com", "11111111111", true);

            // Act
            var result = _validarCliente.TestValidate(cliente);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Cpf);
        }
    }
}