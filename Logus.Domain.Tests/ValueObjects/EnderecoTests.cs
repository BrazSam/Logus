using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class EnderecoTests
{
    [Fact(DisplayName = "Endereco: Criar -> sucesso com dados válidos")]
    public void Deve_CriarEndereco_Quando_DadosValidos()
    {
        var result = Endereco.Criar("Rua das Flores", "123", "Lages", "Centro");
        Assert.True(result.IsSuccess);
        Assert.Equal("Rua das Flores", result.Value!.Logradouro);
    }

    [Theory(DisplayName = "Endereco: Criar -> valida campos obrigatórios")]
    [InlineData("", "123", "Lages", "Centro")]   // logradouro vazio
    [InlineData("Rua das Flores", "", "Lages", "Centro")] // numero vazio
    [InlineData("Rua das Flores", "123", "", "Centro")]   // cidade vazia
    [InlineData("Rua das Flores", "123", "Lages", "")]    // bairro vazio
    public void Deve_Falhar_Quando_CampoObrigatorioVazio(string logradouro, string numero, string cidade, string bairro)
    {
        var result = Endereco.Criar(logradouro, numero, cidade, bairro);
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Endereco: Criar -> normaliza campos com espaços")]
    public void Deve_Normalizar_Campos_Quando_Criar()
    {
        var result = Endereco.Criar("  Rua das Flores  ", " 123 ", " Lages ", " Centro ");
        Assert.True(result.IsSuccess);
        Assert.Equal("Rua das Flores", result.Value!.Logradouro);
        Assert.Equal("123", result.Value!.Numero);
    }

    [Fact(DisplayName = "Endereco: ToString -> formata corretamente")]
    public void Deve_Formatar_ToString_Quando_Chamado()
    {
        var endereco = Endereco.Criar("Rua das Flores", "123", "Lages", "Centro").Value!;
        Assert.Equal("Rua das Flores, 123 - Centro, Lages", endereco.ToString());
    }
}