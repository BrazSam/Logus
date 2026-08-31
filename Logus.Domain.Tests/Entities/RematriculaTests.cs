using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Tests.Entities;

public class RematriculaTests
{
    [Fact(DisplayName = "Rematricula: Criar -> sucesso com dados válidos")]
    public void Deve_CriarRematricula_Quando_DadosValidos()
    {
        var result = Rematricula.Criar(1, 1, 1, StatusRematricula.NaoContatado, null, null);
        Assert.True(result.IsSuccess);
        Assert.Equal(StatusRematricula.NaoContatado, result.Value!.Status);
    }

    [Theory(DisplayName = "Rematricula: Criar -> valida ids")]
    [InlineData(0, 1, false)] // aluno inválido
    [InlineData(1, 0, false)] // solicitacao inválida
    public void Deve_Falhar_Quando_IdInvalido(int alunoId, int solicitacaoId, bool esperado)
    {
        var result = Rematricula.Criar(1, alunoId, solicitacaoId, StatusRematricula.NaoContatado, null, null);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "Rematricula: Criar -> falha com status inválido")]
    public void Deve_Falhar_Quando_StatusInvalido()
    {
        var result = Rematricula.Criar(1, 1, 1, (StatusRematricula)99, null, null);
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Rematricula: Criar -> normaliza observação comercial")]
    public void Deve_Normalizar_Observacao_Quando_Criar()
    {
        var result = Rematricula.Criar(1, 1, 1, StatusRematricula.Contatado, "  Cliente interessado  ", 2);
        Assert.True(result.IsSuccess);
        Assert.Equal("Cliente interessado", result.Value!.ObservacaoComercial);
    }
}