using Microsoft.EntityFrameworkCore;
using Logus.Domain.Entities;

namespace Logus.Infrastructure.Data;

public class LogusDbContext : DbContext
{
    public LogusDbContext(DbContextOptions<LogusDbContext> options)
        : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Colaborador> Colaboradores => Set<Colaborador>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Modulo> Modulos => Set<Modulo>();
    public DbSet<ModuloConcluido> ModulosConcluidos => Set<ModuloConcluido>();
    public DbSet<Rematricula> Rematriculas => Set<Rematricula>();
    public DbSet<SolicitacaoCertificado> Solicitacoes => Set<SolicitacaoCertificado>();
}