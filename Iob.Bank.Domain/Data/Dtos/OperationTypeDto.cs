using Iob.Bank.Domain.Data.Dtos.Base;

namespace Iob.Bank.Domain.Data.Dtos;

public class OperationTypeDto : BaseDto
{
    public required string Name { get; set; }
}
