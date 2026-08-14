using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.ValueObjects;

public record Senha
{
    public string Valor { get; }
    private Senha(string valor)
    {
        Valor = valor;
    }
}
