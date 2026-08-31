// Samuel
using Logus.Domain.Common;
using Logus.Domain.ValueObjects;

namespace Logus.Domain.Entities;

public class ModuloConcluido : Entity
{
    public int SolicitacaoCertificadoId { get; private set; }
    public int ModuloId { get; private set; }
    public decimal Nota { get; private set; }

    private ModuloConcluido(int id, int solicitacaoCertificadoId, int moduloId, decimal nota)
        : base(id)
    {
        SolicitacaoCertificadoId = solicitacaoCertificadoId;
        ModuloId = moduloId;
        Nota = nota;
    }

    public static Result<ModuloConcluido> Criar(int id, int solicitacaoCertificadoId, int moduloId, decimal nota)
    {
        var notifications = new List<Notification>();

        if (solicitacaoCertificadoId <= 0)
            notifications.Add(new Notification("SolicitacaoCertificadoId", "SOLICITACAO_INVALIDA"));

        if (moduloId <= 0)
            notifications.Add(new Notification("ModuloId", "MODULO_INVALIDO"));

        var notaResult = ValueObjects.Nota.Criar(nota);
        if (notaResult.IsFailure) notifications.AddRange(notaResult.Notifications);

        if (notifications.Count != 0)
            return Result<ModuloConcluido>.Failure(notifications);

        var moduloConcluido = new ModuloConcluido(id, solicitacaoCertificadoId, moduloId, notaResult.Value!.Valor);
        return Result<ModuloConcluido>.Success(moduloConcluido);
    }
}