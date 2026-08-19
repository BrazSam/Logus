using System;
using System.Collections.Generic;
using System.Text;

// Samuel
namespace Logus.Domain.Exceptions;

public sealed class DomainException(string message) : Exception(message)
{
}