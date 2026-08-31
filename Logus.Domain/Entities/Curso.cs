// Samuel
using Logus.Domain.Common;
using Logus.Domain.Exceptions;
using Logus.Domain.Services;

namespace Logus.Domain.Entities;

public class Curso : Entity
{
    public string Nome { get; private set; }
    public int CargaHoraria { get; private set; }
    public string? Descricao { get; private set; }
    public IReadOnlyList<Modulo> Modulos => _modulos;

    private readonly List<Modulo> _modulos = new();

    private Curso(int id, string nome, int cargaHoraria, string? descricao)
        : base(id)
    {
        Nome = nome;
        CargaHoraria = cargaHoraria;
        Descricao = descricao;
    }

    public static Result<Curso> Criar(int id, string nome, int cargaHoraria, string? descricao)
    {
        var notifications = new List<Notification>();

        if (NormalizadoService.TextoVazioOuNulo(nome))
            notifications.Add(new Notification("Nome", "NOME_OBRIGATORIO"));
        else
            nome = NormalizadoService.LimparEspacos(nome);

        if (cargaHoraria <= 0)
            notifications.Add(new Notification("CargaHoraria", "CARGA_HORARIA_INVALIDA"));

        if (!NormalizadoService.TextoVazioOuNulo(descricao))
            descricao = NormalizadoService.LimparEspacos(descricao);

        if (notifications.Count != 0)
            return Result<Curso>.Failure(notifications);

        var curso = new Curso(id, nome, cargaHoraria, descricao);
        return Result<Curso>.Success(curso);
    }

    public void AdicionarModulo(Modulo modulo)
    {
        if (modulo == null)
            throw new DomainException("MODULO_INVALIDO");
        _modulos.Add(modulo);
    }
}