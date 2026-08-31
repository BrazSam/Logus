// Samuel
using Logus.Domain.Common;

namespace Logus.Domain.ValueObjects;

public record Nota
{
    public decimal Valor { get; }

    private Nota(decimal valor)
    {
        Valor = valor;
    }

    public static Result<Nota> Criar(decimal valor)
    {
        if (valor < 0 || valor > 10)
            return Result<Nota>.Failure("Nota", "NOTA_INVALIDA");

        return Result<Nota>.Success(new Nota(valor));
    }

    public override string ToString() => Valor.ToString("0.0");
}