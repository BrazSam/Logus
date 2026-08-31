using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Tests.Entities;

public class ColaboradorTests
{
    [Fact]
    public void Criar_ComDadosValidos_RetornaSucesso()
    {
        // Arrange
        var id = 1;
        var nome = "Maria Silva";
        var cpf = "529.982.247-25";
        var telefone = "(49) 99999-1234";
        var senha = "Logus@2026";
        var perfil = TipoPerfil.Professor;

        // Act
        var result = Colaborador.Criar(id, nome, cpf, telefone, senha, perfil);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(nome, result.Value!.Nome);
        Assert.Equal(perfil, result.Value!.Perfil);
    }

    [Theory]
    [InlineData("", "529.982.247-25", "(49) 99999-1234", "Logus@2026")] // nome vazio
    [InlineData("Maria Silva", "", "(49) 99999-1234", "Logus@2026")]     // cpf vazio
    [InlineData("Maria Silva", "529.982.247-25", "", "Logus@2026")]      // telefone vazio
    [InlineData("Maria Silva", "529.982.247-25", "(49) 99999-1234", "")] // senha vazia
    public void Criar_ComCampoObrigatorioInvalido_RetornaFalha(
        string nome, string cpf, string telefone, string senha)
    {
        // Act
        var result = Colaborador.Criar(1, nome, cpf, telefone, senha, TipoPerfil.Professor);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Criar_ComPerfilInvalido_RetornaFalha()
    {
        // Act
        var result = Colaborador.Criar(1, "Maria Silva", "529.982.247-25",
            "(49) 99999-1234", "Logus@2026", (TipoPerfil)99);

        // Assert
        Assert.True(result.IsFailure);
    }
}