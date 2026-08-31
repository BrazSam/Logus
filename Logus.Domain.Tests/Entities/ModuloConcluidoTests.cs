using Logus.Domain.Entities;

namespace Logus.Domain.Tests.Entities;

public class ModuloConcluidoTests
{
    [Theory(DisplayName = "ModuloConcluido: Criar -> valida nota (0 a 10)")]
    [InlineData(1, 1, 1, 7.5, true)]
    [InlineData(1, 1, 1, 0, true)]
    [InlineData(1, 1, 1, 10, true)]
    [InlineData(1, 1, 1, -0.1, false)]  // nota negativa
    [InlineData(1, 1, 1, 10.1, false)]  // nota acima de 10
    public void Deve_CriarModuloConcluido_Quando_NotaValida(int id, int solicitacaoId, int moduloId, decimal nota, bool esperado)
    {
        var result = ModuloConcluido.Criar(id, solicitacaoId, moduloId, nota);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Theory(DisplayName = "ModuloConcluido: Criar -> valida ids")]
    [InlineData(1, 0, 1, 7.5, false)] // solicitacao inválida
    [InlineData(1, 1, 0, 7.5, false)] // modulo inválido
    public void Deve_Falhar_Quando_IdInvalido(int id, int solicitacaoId, int moduloId, decimal nota, bool esperado)
    {
        var result = ModuloConcluido.Criar(id, solicitacaoId, moduloId, nota);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "ModuloConcluido: Criar -> armazena nota corretamente")]
    public void Deve_Armazenar_Nota_Quando_Criar()
    {
        var result = ModuloConcluido.Criar(1, 1, 1, 8.5m);
        Assert.True(result.IsSuccess);
        Assert.Equal(8.5m, result.Value!.Nota);
    }
}