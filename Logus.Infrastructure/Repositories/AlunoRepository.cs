using Logus.Domain.Entities;
using Logus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Logus.Domain.Repositories;

namespace Logus.Infrastructure.Repositories;

public class AlunoRepository(LogusDbContext context)
    : RepositoryBase<Aluno>(context), IAlunoRepository
{
    public async Task<Aluno?> ObterPorCpfAsync(string cpf, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(a => a.Cpf.Valor == cpf, ct);
}