using System;

namespace Iob.Bank.Domain.Exceptions;

public class BankAccountNotFoundException : NotFoundException
{
    public BankAccountNotFoundException(long id) : base($"Bank account not found: {id}")
    {
    }
}
