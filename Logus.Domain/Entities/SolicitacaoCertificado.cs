// Samuel
using Logus.Domain.Enums;
namespace Logus.Domain.Entities;

public class SolicitacaoCertificado : Entity
{
    public int AlunoId { get; private set; }
    public int ProfessorId { get; private set; }

    // Etapa 3 — curso concluído
    public int CursoId { get; private set; }
    public DateOnly DataInicio { get; private set; }
    public DateOnly DataTermino { get; private set; }

    public StatusSolicitacao Status { get; private set; }
    public string? DescricaoProfessor { get; private set; }

    // Etapa 2 — cursos de interesse
    public IReadOnlyList<int> CursosInteresseIds => _cursosInteresseIds;
    private readonly List<int> _cursosInteresseIds = new();

    // Etapa 3 — módulos e notas
    public IReadOnlyList<ModuloConcluido> ModulosConcluidos => _modulosConcluidos;
    private readonly List<ModuloConcluido> _modulosConcluidos = new();

    private SolicitacaoCertificado(int id, int alunoId, int professorId, int cursoId,
        DateOnly dataInicio, DateOnly dataTermino, StatusSolicitacao status,
        string? descricaoProfessor)
        : base(id)
    {
        AlunoId = alunoId;
        ProfessorId = professorId;
        CursoId = cursoId;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        Status = status;
        DescricaoProfessor = descricaoProfessor;
    }
}