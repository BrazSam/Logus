// Samuel
using Logus.Domain.Exceptions;

namespace Logus.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }

    protected Entity(int id = 0)
    {
        if (id < 0) throw new DomainException("ID_NEGATIVO");
        Id = id;
    }
}