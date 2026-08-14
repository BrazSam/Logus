using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.ValueObjects;

public record Telefone
{
    public string Numero { get; }
    private Telefone(string numero)
    {
        Numero = numero;
    }
}
