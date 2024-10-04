using Iob.Bank.Domain.Data.Dtos.Base;

namespace Iob.Bank.Domain.Data.Dtos;

public class BankAccountDto : BaseDto
{
    public decimal Balance { get; set; }
    public string? Type { get; set; }
    public bool Active { get; set; } = true;
    public DateTime OpeningDate { get; set; } = DateTime.Now;
    public DateTime? LastTransactionDate { get; set; }
    public long UserId { get; set; }
}
