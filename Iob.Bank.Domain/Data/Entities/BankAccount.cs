using System.ComponentModel.DataAnnotations.Schema;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Domain.Data.Entities;

[Table("tb_bank_account")]
public class BankAccount : BaseEntity
{
    [Column("balance")]
    public decimal Balance { get; set; }
    [Column("type")]
    public string Type { get; set; } = "Conta corrente";

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("opening_date")]
    public DateTime OpeningDate { get; set; }

    [Column("last_transaction_date")]
    public DateTime? LastTransactionDate { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }
    public User? User { get; set; }
}
