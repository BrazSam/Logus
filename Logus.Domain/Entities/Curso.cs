using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// Samuel
namespace Logus.Domain.Entities;

public class Curso : Entity
{
    public string Nome { get; private set; }
    public int CargaHoraria { get; private set; }
    public string? Descricao { get; private set; }
    public IReadOnlyList<Modulo> Modulos => _modulos;
    private readonly List<Modulo> _modulos = new();
    private Curso(int id, string nome, int cargaHoraria, string? descricao)
        : base(id)
    {
        Nome = nome;
        CargaHoraria = cargaHoraria;
        Descricao = descricao;
    }
}