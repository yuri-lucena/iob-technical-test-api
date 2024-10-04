using System;

namespace Iob.Bank.Domain.Exceptions;

public class CreateBankAccountException : Exception
{
    public CreateBankAccountException(string message) : base(message)
    {
    }
}
