using System;
using System.Collections.Generic;
using System.Text;

// Samuel
using Logus.Domain.Exceptions;
namespace Logus.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    protected Entity(int id = 0)
    {
        if (id < 0) throw new Exception("ID_NEGATIVO");
        Id = id;
    }
}
