using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string mensagem) : base(mensagem) { }
}
