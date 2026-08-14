using System;
using System.Collections.Generic;
using System.Text;

// Samuel
using Logus.Domain.Enums;
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
}
