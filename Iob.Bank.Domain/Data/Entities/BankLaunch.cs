using System.ComponentModel.DataAnnotations.Schema;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Domain.Data.Entities;

[Table("tb_bank_launch")]
public class BankLaunch : BaseEntity
{
    [Column("value")]
    public required decimal Value { get; set; }
    [Column("origin_account_id"), ForeignKey("OriginAccount")]
    public long? OriginAccountId { get; set; }
    public BankAccount? OriginAccount { get; set; }

    [Column("destination_account_id"), ForeignKey("DestinationAccount")]
    public long? DestinationAccountId { get; set; }
    public BankAccount? DestinationAccount { get; set; }

    [Column("operation_type_id"), ForeignKey("OperationType")]
    public required long OperationTypeId { get; set; }
    public OperationType? OperationType { get; set; }
}
