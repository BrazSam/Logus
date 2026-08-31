// Samuel
using Logus.Domain.Common;
using Logus.Domain.Services;

namespace Logus.Domain.ValueObjects;

public record Senha
{
    public string Valor { get; }

    private Senha(string valor)
    {
        Valor = valor;
    }

    public static Result<Senha> Criar(string valor)
    {
        if (NormalizadoService.TextoVazioOuNulo(valor))
            return Result<Senha>.Failure("Senha", "SENHA_OBRIGATORIA");

        var textoLimpo = NormalizadoService.LimparEspacos(valor);
        if (textoLimpo.Length < 6)
            return Result<Senha>.Failure("Senha", "SENHA_MINIMO_CARACTERES");

        return Result<Senha>.Success(new Senha(textoLimpo));
    }

    public override string ToString() => Valor;
}