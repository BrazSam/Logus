// Samuel
using Logus.Domain.Common;
using Logus.Domain.Enums;
using Logus.Domain.Services;

namespace Logus.Domain.Entities;

public class Rematricula : Entity
{
    public int AlunoId { get; private set; }
    public int SolicitacaoCertificadoId { get; private set; }
    public StatusRematricula Status { get; private set; }
    public string? ObservacaoComercial { get; private set; }
    public int? ComercialId { get; private set; }

    private Rematricula(int id, int alunoId, int solicitacaoCertificadoId, StatusRematricula status, string? observacaoComercial, int? comercialId)
        : base(id)
    {
        AlunoId = alunoId;
        SolicitacaoCertificadoId = solicitacaoCertificadoId;
        Status = status;
        ObservacaoComercial = observacaoComercial;
        ComercialId = comercialId;
    }

    public static Result<Rematricula> Criar(int id, int alunoId, int solicitacaoCertificadoId, StatusRematricula status, string? observacaoComercial, int? comercialId)
    {
        var notifications = new List<Notification>();

        if (alunoId <= 0)
            notifications.Add(new Notification("AlunoId", "ALUNO_INVALIDO"));

        if (solicitacaoCertificadoId <= 0)
            notifications.Add(new Notification("SolicitacaoCertificadoId", "SOLICITACAO_INVALIDA"));

        if (!Enum.IsDefined(status))
            notifications.Add(new Notification("Status", "STATUS_REMATRICULA_INVALIDO"));

        if (!NormalizadoService.TextoVazioOuNulo(observacaoComercial))
            observacaoComercial = NormalizadoService.LimparEspacos(observacaoComercial);

        if (notifications.Count != 0)
            return Result<Rematricula>.Failure(notifications);

        var rematricula = new Rematricula(id, alunoId, solicitacaoCertificadoId, status, observacaoComercial, comercialId);
        return Result<Rematricula>.Success(rematricula);
    }
}