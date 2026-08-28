using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class SenhaTests
{
    [Theory]
    [InlineData("Logus@2026")]
    [InlineData("abc123")]
    [InlineData("123456")]   // 6+ chars é válido na sua regra
    public void Criar_ComSenhaValida_RetornaSucesso(string senha)
    {
        var result = Senha.Criar(senha);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("   ")]
    public void Criar_ComSenhaInvalida_RetornaFalha(string senha)
    {
        var result = Senha.Criar(senha);
        Assert.True(result.IsFailure);
    }

}