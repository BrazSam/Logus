using System;
using System.Collections.Generic;
using System.Text;

// Samuel
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
}
