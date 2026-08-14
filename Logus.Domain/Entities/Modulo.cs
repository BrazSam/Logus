using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.Entities;

public class Modulo : Entity
{
    public string Nome { get; private set; }
    public int? CursoId { get; private set; }
    private Modulo(int id, string nome, int? cursoId)
        : base(id)
    {
        Nome = nome;
        CursoId = cursoId;
    }
}
