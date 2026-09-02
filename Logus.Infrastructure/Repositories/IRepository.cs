using Logus.Domain.Common;

namespace Logus.Infrastructure.Repositories;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> ObterTodosAsync(CancellationToken ct = default);
    Task AdicionarAsync(T entidade, CancellationToken ct = default);
    void Atualizar(T entidade);
    void Remover(T entidade);
}