using Logus.Domain.Services;
using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class TelefoneTests
{
    [Theory]
    [InlineData("(49) 99999-1234")]
    [InlineData("49999991234")]
    public void Criar_ComTelefoneValido_RetornaSucesso(string telefone)
    {
        var result = Telefone.Criar(telefone);

        Assert.True(result.IsSuccess);
        Assert.Equal(NormalizadoService.LimparEDigitos(telefone), result.Value!.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData("493322-4455")]  // 10 dígitos → fixo, inválido (só celular)
    [InlineData("abc")]
    public void Criar_ComTelefoneInvalido_RetornaFalha(string telefone)
    {
        var result = Telefone.Criar(telefone);

        Assert.True(result.IsFailure);
    }
}