using System;
using System.Collections.Generic;
using System.Text;

// Samuel
using Logus.Domain.ValueObjects;
namespace Logus.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome { get; protected set; }
    public Cpf Cpf { get; protected set; }
    public Telefone Telefone { get; protected set; }
    protected Pessoa(int id, string nome, Cpf cpf, Telefone telefone)
        : base(id)
    {
        Nome = nome;
        Cpf = cpf;
        Telefone = telefone;
    }
}
