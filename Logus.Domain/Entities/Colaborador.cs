// Samuel
using Logus.Domain.Common;
using Logus.Domain.Enums;
using Logus.Domain.Services;
using Logus.Domain.ValueObjects;

namespace Logus.Domain.Entities;

public class Colaborador : Pessoa
{
    public Senha Senha { get; private set; }
    public TipoPerfil Perfil { get; private set; }

    private Colaborador(int id, string nome, Cpf cpf, Telefone telefone, Senha senha, TipoPerfil perfil)
        : base(id, nome, cpf, telefone)
    {
        Senha = senha;
        Perfil = perfil;
    }

    public static Result<Colaborador> Criar(int id, string nome, string cpf, string telefone, string senha, TipoPerfil perfil)
    {
        var notifications = new List<Notification>();

        if (NormalizadoService.TextoVazioOuNulo(nome))
            notifications.Add(new Notification("Nome", "NOME_OBRIGATORIO"));
        else
            nome = NormalizadoService.LimparEspacos(nome);

        if (!Enum.IsDefined(perfil))
            notifications.Add(new Notification("Perfil", "PERFIL_INVALIDO"));

        var cpfResult = Cpf.Criar(cpf);
        if (cpfResult.IsFailure) notifications.AddRange(cpfResult.Notifications);

        var telefoneResult = Telefone.Criar(telefone);
        if (telefoneResult.IsFailure) notifications.AddRange(telefoneResult.Notifications);

        var senhaResult = Senha.Criar(senha);
        if (senhaResult.IsFailure) notifications.AddRange(senhaResult.Notifications);

        if (notifications.Count != 0)
            return Result<Colaborador>.Failure(notifications);

        var colaborador = new Colaborador(id, nome, cpfResult.Value!, telefoneResult.Value!, senhaResult.Value!, perfil);
        return Result<Colaborador>.Success(colaborador);
    }
}