using System;

namespace Iob.Bank.Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(long id) : base($"User not found: {id}")
    {
    }
}
