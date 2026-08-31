// Samuel
using Logus.Domain.Common;
using Logus.Domain.Services;

namespace Logus.Domain.ValueObjects;

public record Telefone
{
    public string Numero { get; }

    private Telefone(string numero)
    {
        Numero = numero;
    }

    public static Result<Telefone> Criar(string numero)
    {
        if (NormalizadoService.TextoVazioOuNulo(numero))
            return Result<Telefone>.Failure("Telefone", "TELEFONE_OBRIGATORIO");

        var textoLimpo = NormalizadoService.LimparEDigitos(numero);
        if (textoLimpo.Length != 11)
            return Result<Telefone>.Failure("Telefone", "TELEFONE_DIGITOS");

        return Result<Telefone>.Success(new Telefone(textoLimpo));
    }

    public override string ToString() => Numero;
}