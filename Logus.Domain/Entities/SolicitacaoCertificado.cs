// Samuel
using Logus.Domain.Common;
using Logus.Domain.Enums;
using Logus.Domain.Exceptions;
using Logus.Domain.Services;

namespace Logus.Domain.Entities;

public class SolicitacaoCertificado : Entity, IAggregateRoot
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

    public static Result<SolicitacaoCertificado> Criar(int id, int alunoId, int professorId,
        int cursoId, DateOnly dataInicio, DateOnly dataTermino,
        StatusSolicitacao status, string? descricaoProfessor)
    {
        var notifications = new List<Notification>();

        if (alunoId <= 0)
            notifications.Add(new Notification("AlunoId", "ALUNO_INVALIDO"));

        if (professorId <= 0)
            notifications.Add(new Notification("ProfessorId", "PROFESSOR_INVALIDO"));

        if (cursoId <= 0)
            notifications.Add(new Notification("CursoId", "CURSO_INVALIDO"));

        if (dataInicio == default)
            notifications.Add(new Notification("DataInicio", "DATA_INICIO_OBRIGATORIA"));

        if (dataTermino == default)
            notifications.Add(new Notification("DataTermino", "DATA_TERMINO_OBRIGATORIA"));
        else if (dataInicio != default && dataTermino < dataInicio)
            notifications.Add(new Notification("DataTermino", "DATA_TERMINO_MENOR_INICIO"));

        if (!Enum.IsDefined(status))
            notifications.Add(new Notification("Status", "STATUS_SOLICITACAO_INVALIDO"));

        if (!NormalizadoService.TextoVazioOuNulo(descricaoProfessor))
            descricaoProfessor = NormalizadoService.LimparEspacos(descricaoProfessor);

        if (notifications.Count != 0)
            return Result<SolicitacaoCertificado>.Failure(notifications);

        var solicitacao = new SolicitacaoCertificado(id, alunoId, professorId, cursoId,
            dataInicio, dataTermino, status, descricaoProfessor);
        return Result<SolicitacaoCertificado>.Success(solicitacao);
    }

    // Etapa 2 — adiciona curso de interesse (limite 1 a 3)
    public void AdicionarCursoInteresse(int cursoId)
    {
        if (cursoId <= 0)
            throw new DomainException("CURSO_INVALIDO");
        if (_cursosInteresseIds.Count >= 3)
            throw new DomainException("LIMITE_CURSOS_INTERESSE");
        if (_cursosInteresseIds.Contains(cursoId))
            throw new DomainException("CURSO_INTERESSE_DUPLICADO");
        _cursosInteresseIds.Add(cursoId);
    }

    // Etapa 3 — adiciona módulo concluído com nota
    public void AdicionarModuloConcluido(ModuloConcluido moduloConcluido)
    {
        if (moduloConcluido == null)
            throw new DomainException("MODULO_CONCLUIDO_INVALIDO");
        _modulosConcluidos.Add(moduloConcluido);
    }
}