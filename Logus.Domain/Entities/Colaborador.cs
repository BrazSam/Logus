using System;
using System.Collections.Generic;
using System.Text;

// Samuel
using Logus.Domain.Enums;
using Logus.Domain.ValueObjects;
namespace Logus.Domain.Entities;

public class Colaborador : Pessoa
{
    public Senha Senha { get; private set; }
    public TipoPerfil Perfil { get; private set; }
    private Colaborador(int id, string nome, Cpf cpf, Telefone telefone, Senha senha, TipoPerfil perfil)
        : base(id, nome, cpf, telefone)
    {
        Senha = senha;
        Perfil = perfil;
    }
}
