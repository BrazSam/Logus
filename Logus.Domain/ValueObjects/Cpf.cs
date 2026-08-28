using System.Linq;
using Logus.Domain.Common;
using Logus.Domain.Services;

namespace Logus.Domain.ValueObjects;

public record Cpf
{
    public string Valor { get; }

    private Cpf(string valor) => Valor = valor;

    public static Result<Cpf> Criar(string valor)
    {
        if (NormalizadoService.TextoVazioOuNulo(valor))
            return Result<Cpf>.Failure("Cpf", "CPF_OBRIGATORIO");

        var textoLimpo = NormalizadoService.LimparEDigitos(valor);
        if (textoLimpo.Length != 11)
            return Result<Cpf>.Failure("Cpf", "CPF_DIGITOS");

        if (!DigitosVerificadoresValidos(textoLimpo))
            return Result<Cpf>.Failure("Cpf", "CPF_INVALIDO");

        return Result<Cpf>.Success(new Cpf(textoLimpo));
    }

    private static bool DigitosVerificadoresValidos(string cpf)
    {
        if (cpf.Distinct().Count() == 1) return false; // todos os dígitos iguais

        for (int j = 9; j < 11; j++)
        {
            int soma = 0;
            for (int i = 0; i < j; i++)
                soma += (cpf[i] - '0') * (j + 1 - i);

            int digito = soma % 11;
            digito = digito < 2 ? 0 : 11 - digito;

            if (cpf[j] - '0' != digito) return false;
        }
        return true;
    }

    public override string ToString() => Valor;
}