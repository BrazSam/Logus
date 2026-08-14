using System;
using System.Collections.Generic;
using System.Text;

// Samuel
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
}
