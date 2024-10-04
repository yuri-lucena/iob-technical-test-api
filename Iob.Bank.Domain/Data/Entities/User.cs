using System.ComponentModel.DataAnnotations.Schema;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Domain.Data.Entities;

[Table("tb_user")]
public class User : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }
    [Column("identifier")]
    public string Identifier { get; set; }
    [Column("birthday")]
    public DateTime Birthday { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("password")]
    public string Password { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("address")]
    public string? Address { get; set; }
    [Column("user_type_id")]
    public long UserTypeId { get; set; }
    public UserType? UserType { get; set; }
    [Column("active")]
    public bool Active { get; set; } = true;
}
