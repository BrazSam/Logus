using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class NotaTests
{
    [Theory(DisplayName = "Nota: Criar -> valida faixa 0 a 10")]
    [InlineData(0, true)]
    [InlineData(5.5, true)]
    [InlineData(10, true)]
    [InlineData(-1, false)]  // negativa
    [InlineData(10.1, false)] // acima de 10
    public void Deve_CriarNota_Quando_ValorNaFaixa(decimal valor, bool esperado)
    {
        var result = Nota.Criar(valor);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "Nota: Criar -> armazena valor corretamente")]
    public void Deve_Armazenar_Valor_Quando_Criar()
    {
        var result = Nota.Criar(8.5m);
        Assert.True(result.IsSuccess);
        Assert.Equal(8.5m, result.Value!.Valor);
    }

    [Fact(DisplayName = "Nota: ToString -> formata com uma casa decimal")]
    public void Deve_Formatar_ToString_Quando_Chamado()
    {
        var nota = Nota.Criar(8.5m).Value!;
        Assert.Equal("8.5", nota.ToString());
    }
}