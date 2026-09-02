using Microsoft.EntityFrameworkCore;
using Logus.Domain.Common;
using Logus.Infrastructure.Data;

namespace Logus.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(LogusDbContext context) : IRepository<T>
    where T : class, IAggregateRoot
{
    protected readonly LogusDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => await DbSet.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<T>> ObterTodosAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().ToListAsync(ct);

    public virtual async Task AdicionarAsync(T entidade, CancellationToken ct = default)
        => await DbSet.AddAsync(entidade, ct);

    public virtual void Atualizar(T entidade) => DbSet.Update(entidade);

    public virtual void Remover(T entidade) => DbSet.Remove(entidade);
}