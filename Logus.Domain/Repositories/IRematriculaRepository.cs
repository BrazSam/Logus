using System.Collections.Generic;
using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Repositories;

public interface IRematriculaRepository : IRepository<Rematricula>
{
    Task<IReadOnlyList<Rematricula>> ObterPorStatusAsync(
        StatusRematricula status, CancellationToken ct = default);
}