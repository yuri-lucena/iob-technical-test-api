using System.ComponentModel.DataAnnotations.Schema;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Domain.Data.Entities;

[Table("tb_operation_type")]
public class OperationType : BaseEntity
{
    [Column("name")]
    public required string Name { get; set; }
}
