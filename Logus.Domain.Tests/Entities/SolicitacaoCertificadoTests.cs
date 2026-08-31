using Logus.Domain.Entities;
using Logus.Domain.Enums;
using Logus.Domain.Exceptions;

namespace Logus.Domain.Tests.Entities;

public class SolicitacaoCertificadoTests
{
    [Fact(DisplayName = "SolicitacaoCertificado: Criar -> sucesso com dados válidos")]
    public void Deve_CriarSolicitacao_Quando_DadosValidos()
    {
        var result = SolicitacaoCertificado.Criar(
            1, 1, 2, 1,
            new DateOnly(2026, 1, 10), new DateOnly(2026, 6, 20),
            StatusSolicitacao.PendenteNotas, null);

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusSolicitacao.PendenteNotas, result.Value!.Status);
    }

    [Theory(DisplayName = "SolicitacaoCertificado: Criar -> valida ids")]
    [InlineData(0, 2, 1, false)] // aluno inválido
    [InlineData(1, 0, 1, false)] // professor inválido
    [InlineData(1, 2, 0, false)] // curso inválido
    public void Deve_Falhar_Quando_IdInvalido(int alunoId, int professorId, int cursoId, bool esperado)
    {
        var result = SolicitacaoCertificado.Criar(
            1, alunoId, professorId, cursoId,
            new DateOnly(2026, 1, 10), new DateOnly(2026, 6, 20),
            StatusSolicitacao.PendenteNotas, null);

        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "SolicitacaoCertificado: Criar -> falha quando data término menor que início")]
    public void Deve_Falhar_Quando_DataTerminoMenorQueInicio()
    {
        var result = SolicitacaoCertificado.Criar(
            1, 1, 2, 1,
            new DateOnly(2026, 6, 20), new DateOnly(2026, 1, 10),
            StatusSolicitacao.PendenteNotas, null);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "SolicitacaoCertificado: Criar -> falha com status inválido")]
    public void Deve_Falhar_Quando_StatusInvalido()
    {
        var result = SolicitacaoCertificado.Criar(
            1, 1, 2, 1,
            new DateOnly(2026, 1, 10), new DateOnly(2026, 6, 20),
            (StatusSolicitacao)99, null);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "SolicitacaoCertificado: AdicionarCursoInteresse -> adiciona até 3 cursos")]
    public void Deve_Adicionar_Ate_Tres_CursosInteresse()
    {
        var solicitacao = SolicitacaoCertificado.Criar(
            1, 1, 2, 1,
            new DateOnly(2026, 1, 10), new DateOnly(2026, 6, 20),
            StatusSolicitacao.PendenteNotas, null).Value!;

        solicitacao.AdicionarCursoInteresse(1);
        solicitacao.AdicionarCursoInteresse(2);
        solicitacao.AdicionarCursoInteresse(3);

        Assert.Equal(3, solicitacao.CursosInteresseIds.Count);
    }

    [Fact(DisplayName = "SolicitacaoCertificado: AdicionarCursoInteresse -> lança exceção ao exceder 3")]
    public void Deve_Lancar_Excecao_Quando_Exceder_TresCursos()
    {
        var solicitacao = SolicitacaoCertificado.Criar(
            1, 1, 2, 1,
            new DateOnly(2026, 1, 10), new DateOnly(2026, 6, 20),
            StatusSolicitacao.PendenteNotas, null).Value!;

        solicitacao.AdicionarCursoInteresse(1);
        solicitacao.AdicionarCursoInteresse(2);
        solicitacao.AdicionarCursoInteresse(3);

        Assert.Throws<DomainException>(() => solicitacao.AdicionarCursoInteresse(4));
    }
}