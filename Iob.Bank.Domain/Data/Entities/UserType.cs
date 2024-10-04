using System.ComponentModel.DataAnnotations.Schema;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Domain.Data.Entities;

[Table("tb_user_type")]
public class UserType : BaseEntity
{
    [Column("type")]
    public required string Type { get; set; }
    [Column("description")]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}
