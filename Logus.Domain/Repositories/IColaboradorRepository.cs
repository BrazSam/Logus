using Logus.Domain.Entities;
using Logus.Domain.Enums;

namespace Logus.Domain.Repositories;

public interface IColaboradorRepository : IRepository<Colaborador>
{
    Task<Colaborador?> ObterPorCpfAsync(string cpf, CancellationToken ct = default);
}