using System;

namespace Logus.Dominio.ObjetosDeValor;

public sealed record Frequencia
{
    public decimal Percentual { get; }

    public Frequencia(decimal percentual)
    {
        if (percentual < 0m || percentual > 100m)
        {
            throw new ArgumentException(
                "A frequência deve estar entre 0 e 100.",
                nameof(percentual));
        }

        Percentual = percentual;
    }

    public bool AtendeAoMinimo()
    {
        return Percentual >= 75m;
    }
}