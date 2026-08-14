using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.ValueObjects;

public record Nota
{
    public decimal Valor { get; }
    private Nota(decimal valor)
    {
        Valor = valor;
    }
}
