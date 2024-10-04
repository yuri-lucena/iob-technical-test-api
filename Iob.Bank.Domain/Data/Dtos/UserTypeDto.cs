namespace Iob.Bank.Domain.Data.Dtos;

public class UserTypeDto
{
    public required string Type { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
