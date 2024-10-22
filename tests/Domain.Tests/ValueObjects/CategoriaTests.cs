using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class CategoriaTests
    {
        [Fact]
        public void Categoria_DeveConterTodosOsValoresDefinidos()
        {
            // Act
            var categorias = Enum.GetValues(typeof(Categoria)).Cast<Categoria>();

            // Assert
            categorias.Should().HaveCount(4)
                .And.Contain(
                [
                    Categoria.Lanche,
                    Categoria.Acompanhamento,
                    Categoria.Bebida,
                    Categoria.Sobremesa
                ]);
        }

        [Theory]
        [InlineData(Categoria.Lanche, 0)]
        [InlineData(Categoria.Acompanhamento, 1)]
        [InlineData(Categoria.Bebida, 2)]
        [InlineData(Categoria.Sobremesa, 3)]
        public void Categoria_DeveTerValoresCorretos(Categoria categoria, int valorEsperado) =>
            // Assert
            ((int)categoria).Should().Be(valorEsperado);

        [Theory]
        [InlineData(0, Categoria.Lanche)]
        [InlineData(1, Categoria.Acompanhamento)]
        [InlineData(2, Categoria.Bebida)]
        [InlineData(3, Categoria.Sobremesa)]
        public void Categoria_DeveConverterDeInteiroCorretamente(int valor, Categoria categoriaEsperada)
        {
            // Act
            var categoria = (Categoria)valor;

            // Assert
            categoria.Should().Be(categoriaEsperada);
        }

        [Fact]
        public void Categoria_InvalidaDeveGerarExcecao()
        {
            // Act
            Action act = () => Enum.Parse<Categoria>("Invalido");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Invalido*");
        }
    }
}
