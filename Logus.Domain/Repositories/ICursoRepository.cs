using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Repositories;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorNomeAsync(string nome, CancellationToken ct = default);
}