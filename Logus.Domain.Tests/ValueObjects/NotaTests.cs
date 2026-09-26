using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class NotaTests
{
    public static TheoryData<decimal, bool> DadosNota => new()
    {
        { 0m, true },
        { 5.5m, true },
        { 10m, true },
        { -1m, false },
        { 10.1m, false },
    };

    [Theory(DisplayName = "Nota: Criar -> valida faixa 0 a 10")]
    [MemberData(nameof(DadosNota))]
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