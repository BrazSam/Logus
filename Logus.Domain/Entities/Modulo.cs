// Samuel
using Logus.Domain.Common;
using Logus.Domain.Services;

namespace Logus.Domain.Entities;

public class Modulo : Entity
{
    public string Nome { get; private set; }
    public int? CursoId { get; private set; }

    private Modulo(int id, string nome, int? cursoId)
        : base(id)
    {
        Nome = nome;
        CursoId = cursoId;
    }

    public static Result<Modulo> Criar(int id, string nome, int? cursoId)
    {
        if (NormalizadoService.TextoVazioOuNulo(nome))
            return Result<Modulo>.Failure("Nome", "NOME_OBRIGATORIO");

        nome = NormalizadoService.LimparEspacos(nome);
        return Result<Modulo>.Success(new Modulo(id, nome, cursoId));
    }
}