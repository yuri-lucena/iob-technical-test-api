namespace Iob.Bank.Domain.Data.Dtos;

public class UserAuthRequestDto
{
    public required string Email { get; set; }
    public required string Pass { get; set; }
}
