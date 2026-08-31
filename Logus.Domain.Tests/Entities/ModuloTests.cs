using Logus.Domain.Entities;

namespace Logus.Domain.Tests.Entities;

public class ModuloTests
{
    [Theory(DisplayName = "Modulo: Criar -> valida nome")]
    [InlineData(1, "Lógica de Programação", 1, true)]
    [InlineData(1, "", 1, false)]        // nome vazio
    [InlineData(1, "   ", 1, false)]     // nome só espaços
    [InlineData(1, null, 1, false)]      // nome nulo
    public void Deve_CriarModulo_Quando_NomeValido(int id, string? nome, int? cursoId, bool esperado)
    {
        var result = Modulo.Criar(id, nome!, cursoId);
        Assert.Equal(esperado, result.IsSuccess);
    }

    [Fact(DisplayName = "Modulo: Criar -> normaliza nome com espaços")]
    public void Deve_Normalizar_Nome_Quando_CriarModulo()
    {
        var result = Modulo.Criar(1, "  Lógica  ", 1);
        Assert.True(result.IsSuccess);
        Assert.Equal("Lógica", result.Value!.Nome);
    }

    [Fact(DisplayName = "Modulo: Criar -> aceita cursoId nulo")]
    public void Deve_Aceitar_CursoIdNulo()
    {
        var result = Modulo.Criar(1, "Lógica", null);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value!.CursoId);
    }
}