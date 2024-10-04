using Iob.Bank.Domain.Data.Dtos.Base;

namespace Iob.Bank.Domain.Data.Dtos;

public class BankLaunchDto : BaseDto
{
    public decimal Value { get; set; }
    public long? OriginAccountId { get; set; }
    public BankAccountDto? OriginAccount { get; set; }
    public long? DestinationAccountId { get; set; }
    public BankAccountDto? DestinationAccount { get; set; }
    public long OperationTypeId { get; set; }
    public OperationTypeDto? OperationType { get; set; }
}
