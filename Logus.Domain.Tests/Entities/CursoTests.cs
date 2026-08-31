using Logus.Domain.Entities;
using Logus.Domain.Exceptions;

namespace Logus.Domain.Tests.Entities;

public class CursoTests
{
    [Theory(DisplayName = "Curso: Criar -> valida nome e carga horária")]
    [InlineData(1, "Informática", 120, "Curso de informática", true)]
    [InlineData(1, "", 120, "Curso", false)]          // nome vazio
    [InlineData(1, "Informática", 0, "Curso", false)] // carga horária zero
    [InlineData(1, "Informática", -10, "Curso", false)] // carga horária negativa
    public void Deve_CriarCurso_Quando_DadosValidos(int id, string nome, int cargaHoraria, string? descricao, bool esperado)
    {
        var result = Curso.Criar(id, nome, cargaHoraria, descricao);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "Curso: Criar -> normaliza nome com espaços")]
    public void Deve_Normalizar_Nome_Quando_CriarCurso()
    {
        var result = Curso.Criar(1, "  Informática  ", 120, null);
        Assert.True(result.IsSuccess);
        Assert.Equal("Informática", result.Value!.Nome);
    }

    [Fact(DisplayName = "Curso: AdicionarModulo -> adiciona módulo válido")]
    public void Deve_Adicionar_Modulo_Quando_Valido()
    {
        var curso = Curso.Criar(1, "Informática", 120, null).Value!;
        var modulo = Modulo.Criar(1, "Lógica", 1).Value!;

        curso.AdicionarModulo(modulo);

        Assert.Single(curso.Modulos);
        Assert.Equal("Lógica", curso.Modulos[0].Nome);
    }

    [Fact(DisplayName = "Curso: AdicionarModulo -> lança DomainException quando módulo nulo")]
    public void Deve_Lancar_Excecao_Quando_ModuloNulo()
    {
        var curso = Curso.Criar(1, "Informática", 120, null).Value!;
        Assert.Throws<DomainException>(() => curso.AdicionarModulo(null!));
    }
}