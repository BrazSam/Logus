using Logus.Domain.Entities;

namespace Logus.Domain.Tests.Entities;

public class AlunoTests
{
    // Menor de 18 COM responsável → sucesso
    [Fact]
    public void Criar_MenorComResponsavel_RetornaSucesso()
    {
        var result = Aluno.Criar(
            1,
            "João Souza",
            "529.982.247-25",
            new DateOnly(2010, 5, 10),   // menor de 18
            "Maria Souza",                  // responsável presente
            "(49) 99999-1234",
            "Rua das Flores", "123", "Lages", "Centro");

        Assert.True(result.IsSuccess);
    }

    // Menor de 18 SEM responsável → falha
    [Fact]
    public void Criar_MenorSemResponsavel_RetornaFalha()
    {
        var result = Aluno.Criar(
            1,
            "João Souza",
            "529.982.247-25",
            new DateOnly(2010, 5, 10),
            null,                           // responsável ausente
            "(49) 99999-1234",
            "Rua das Flores", "123", "Lages", "Centro");

        Assert.True(result.IsFailure);
    }

    // Adulto (18+) SEM responsável → sucesso
    [Fact]
    public void Criar_AdultoSemResponsavel_RetornaSucesso()
    {
        var result = Aluno.Criar(
            1,
            "João Souza",
            "529.982.247-25",
            new DateOnly(2000, 5, 10),    // adulto
            null,
            "(49) 99999-1234",
            "Rua das Flores", "123", "Lages", "Centro");

        Assert.True(result.IsSuccess);
    }

    // Campos obrigatórios faltando → falha
    [Theory]
    [InlineData("", "529.982.247-25", "(49) 99999-1234")] // nome vazio
    [InlineData("João Souza", "", "(49) 99999-1234")]     // cpf vazio
    [InlineData("João Souza", "529.982.247-25", "")]      // telefone vazio
    public void Criar_ComCampoObrigatorioFaltando_RetornaFalha(
        string nome, string cpf, string telefone)
    {
        var result = Aluno.Criar(
            1, nome, cpf, new DateOnly(2000, 5, 10), null,
            telefone, "Rua das Flores", "123", "Lages", "Centro");

        Assert.True(result.IsFailure);
    }
}