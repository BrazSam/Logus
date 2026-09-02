using System.Collections.Generic;
using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Repositories;

public interface ISolicitacaoCertificadoRepository : IRepository<SolicitacaoCertificado>
{
    Task<IReadOnlyList<SolicitacaoCertificado>> ObterPorStatusAsync(
        StatusSolicitacao status, CancellationToken ct = default);
}