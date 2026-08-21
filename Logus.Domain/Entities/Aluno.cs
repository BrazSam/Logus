// Samuel
using Logus.Domain.Common;
using Logus.Domain.Services;
using Logus.Domain.ValueObjects;

namespace Logus.Domain.Entities;

public class Aluno : Pessoa
{
    public DateOnly DataNascimento { get; private set; }
    public string? NomeResponsavel { get; private set; }
    public Endereco Endereco { get; private set; }

    private Aluno(int id, string nome, Cpf cpf, DateOnly dataNascimento, string? nomeResponsavel, Telefone telefone, Endereco endereco)
        : base(id, nome, cpf, telefone)
    {
        DataNascimento = dataNascimento;
        NomeResponsavel = nomeResponsavel;
        Endereco = endereco;
    }

    public static Result<Aluno> Criar(int id, string nome, string cpf, DateOnly dataNascimento, string? nomeResponsavel, string telefone, string logradouro, string numero, string cidade, string bairro)
    {
        var notifications = new List<Notification>();

        if (NormalizadoService.TextoVazioOuNulo(nome))
            notifications.Add(new Notification("Nome", "NOME_OBRIGATORIO"));
        else
            nome = NormalizadoService.LimparEspacos(nome);

        if (dataNascimento == default)
            notifications.Add(new Notification("DataNascimento", "DATA_NASCIMENTO_OBRIGATORIA"));

        if (!NormalizadoService.TextoVazioOuNulo(nomeResponsavel))
            nomeResponsavel = NormalizadoService.LimparEspacos(nomeResponsavel);

        var cpfResult = Cpf.Criar(cpf);
        if (cpfResult.IsFailure) notifications.AddRange(cpfResult.Notifications);

        var telefoneResult = Telefone.Criar(telefone);
        if (telefoneResult.IsFailure) notifications.AddRange(telefoneResult.Notifications);

        var enderecoResult = Endereco.Criar(logradouro, numero, cidade, bairro);
        if (enderecoResult.IsFailure) notifications.AddRange(enderecoResult.Notifications);

        if (notifications.Count != 0)
            return Result<Aluno>.Failure(notifications);

        var aluno = new Aluno(id, nome, cpfResult.Value!, dataNascimento, nomeResponsavel, telefoneResult.Value!, enderecoResult.Value!);
        return Result<Aluno>.Success(aluno);
    }
}