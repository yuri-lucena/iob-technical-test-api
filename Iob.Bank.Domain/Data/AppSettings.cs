namespace Iob.Bank.Domain.Data;

public class AppSettings
{
    public required string Salt { get; set; }
    public required Jwt Jwt { get; set; }
}

public class Jwt
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Subject { get; set; }
    public required ushort Expiration { get; set; }
}