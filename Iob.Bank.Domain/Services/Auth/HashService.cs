using System.Security.Cryptography;
using System.Text;
using Iob.Bank.Domain.Interfaces.Services;

namespace Iob.Bank.Domain.Services.Auth;

public class HashService : IHashService
{
    public string GetSha256(string valueToBeEncrypted)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueToBeEncrypted));

            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
    }
}
