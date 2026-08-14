using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.ValueObjects;

public record Cpf
{
    public string Valor { get; }
    private Cpf(string valor)
    {
        Valor = valor;
    }
}
