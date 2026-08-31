using Logus.Domain.Services; //Samuel Braz dos Santos

namespace Logus.Domain.Tests.Services;

public class NormalizadoServiceTests
{
    [Theory(DisplayName = "NormalizadoService: TextoVazioOuNulo -> valida nulo/vazio")]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData("texto", false)]
    public void Deve_TextoVazioOuNulo_RetornarEsperado(string? input, bool expected)
    {
        var result = NormalizadoService.TextoVazioOuNulo(input);
        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizadoService: LimparEspacos -> normaliza espaços")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" a b c ", "a b c")]
    [InlineData("a\tb\nc", "a b c")]
    public void Deve_Normalizar_Espacos_Quando_LimparEspacosChamado(string? input, string expected)
    {
        var result = NormalizadoService.LimparEspacos(input);
        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizadoService: LimparTodosEspacos -> remove todos os espaços")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a b c", "abc")]
    [InlineData(" a b ", "ab")]
    public void Deve_Remover_Todos_Espacos_Quando_LimparTodosEspacosChamado(string? input, string expected)
    {
        var result = NormalizadoService.LimparTodosEspacos(input);
        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizadoService: ParaMaiusculo -> converte para maiúsculo")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc", "ABC")]
    [InlineData("áéíõç", "ÁÉÍÕÇ")]
    public void Deve_Converter_Para_Maiusculo_Quando_ParaMaiusculoChamado(string? input, string expected)
    {
        var result = NormalizadoService.ParaMaiusculo(input);
        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizadoService: LimparEDigitos -> mantém apenas dígitos")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a1b2c3", "123")]
    [InlineData("(11) 91234-5678", "11912345678")]
    [InlineData("no-digits", "")]
    public void Deve_Manter_Apenas_Digitos_Quando_LimparEDigitosChamado(string? input, string expected)
    {
        var result = NormalizadoService.LimparEDigitos(input);
        Assert.Equal(expected, result);
    }
}