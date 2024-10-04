namespace Iob.Bank.Domain.Interfaces.Services;

public interface IHashService
{
    string GetSha256(string valueToBeEncrypted);
}
