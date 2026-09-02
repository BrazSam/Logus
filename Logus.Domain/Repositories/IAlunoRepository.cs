using Logus.Domain.Entities;
using Logus.Domain.Enums;


namespace Logus.Domain.Repositories;

public interface IAlunoRepository : IRepository<Aluno>
{
    Task<Aluno?> ObterPorCpfAsync(string cpf, CancellationToken ct = default);
}