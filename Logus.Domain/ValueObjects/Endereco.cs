// Samuel
using Logus.Domain.Common;
using Logus.Domain.Services;

namespace Logus.Domain.ValueObjects;

public record Endereco
{
    public string Logradouro { get; }
    public string Numero { get; }
    public string Cidade { get; }
    public string Bairro { get; }

    private Endereco(string logradouro, string numero, string cidade, string bairro)
    {
        Logradouro = logradouro;
        Numero = numero;
        Cidade = cidade;
        Bairro = bairro;
    }

    public static Result<Endereco> Criar(string logradouro, string numero, string cidade, string bairro)
    {
        var notifications = new List<Notification>();

        if (NormalizadoService.TextoVazioOuNulo(logradouro))
            notifications.Add(new Notification("Logradouro", "LOGRADOURO_OBRIGATORIO"));
        else
            logradouro = NormalizadoService.LimparEspacos(logradouro);

        if (NormalizadoService.TextoVazioOuNulo(numero))
            notifications.Add(new Notification("Numero", "NUMERO_OBRIGATORIO"));
        else
            numero = NormalizadoService.LimparEspacos(numero);

        if (NormalizadoService.TextoVazioOuNulo(cidade))
            notifications.Add(new Notification("Cidade", "CIDADE_OBRIGATORIA"));
        else
            cidade = NormalizadoService.LimparEspacos(cidade);

        if (NormalizadoService.TextoVazioOuNulo(bairro))
            notifications.Add(new Notification("Bairro", "BAIRRO_OBRIGATORIO"));
        else
            bairro = NormalizadoService.LimparEspacos(bairro);

        if (notifications.Count != 0)
            return Result<Endereco>.Failure(notifications);

        return Result<Endereco>.Success(new Endereco(logradouro, numero, cidade, bairro));
    }

    public override string ToString() => $"{Logradouro}, {Numero} - {Bairro}, {Cidade}";
}