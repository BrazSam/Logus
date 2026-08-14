using System;
using System.Collections.Generic;
using System.Text;

// Samuel
using Logus.Domain.ValueObjects;
namespace Logus.Domain.Entities;

public class Aluno : Pessoa
{
    public DateOnly DataNascimento { get; private set; }
    public string? NomeResponsavel { get; private set; }
    public Endereco Endereco { get; private set; }
    private Aluno(int id, string nome, Cpf cpf, DateOnly dataNascimento, string? nomeResponsavel, Telefone telefone, Endereco endereco)
        : base(id, nome, cpf, telefone)
    {
        DataNascimento = dataNascimento;
        NomeResponsavel = nomeResponsavel;
        Endereco = endereco;
    }
}
