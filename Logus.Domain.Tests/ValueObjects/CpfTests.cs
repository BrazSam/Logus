using Logus.Domain.ValueObjects;

namespace Logus.Domain.Tests.ValueObjects;

public class CpfTests
{
    [Fact]
    public void Criar_ComCpfValido_RetornaSucesso()
    {
        // Arrange
        var cpf = "529.982.247-25";

        // Act
        var result = Cpf.Criar(cpf);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("52998224725", result.Value!.ToString());
    }

    [Theory]
    [InlineData("123.456.789-00")] // dígitos verificadores inválidos
    [InlineData("111.111.111-11")]  // todos os dígitos iguais
    [InlineData("abc")]             // não numérico
    [InlineData("")]                // vazio
    public void Criar_ComCpfInvalido_RetornaFalha(string cpf)
    {
        // Arrange (dado no InlineData)

        // Act
        var result = Cpf.Criar(cpf);

        // Assert
        Assert.True(result.IsFailure);
    }
}