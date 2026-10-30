using System;

namespace Logus.Dominio.ObjetosDeValor;

public sealed record Nota
{
    public decimal Valor { get; }

    public Nota(decimal valor)
    {
        if (valor < 0m || valor > 10m)
        {
            throw new ArgumentException(
                "A nota deve estar entre 0 e 10.",
                nameof(valor));
        }

        Valor = valor;
    }
}