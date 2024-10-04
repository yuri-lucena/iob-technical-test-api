using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iob.Bank.Domain.Data.Entities.Base;

public class BaseEntity
{
    [Column("id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual long Id { get; set; }

    [Column("created_at")]
    public DateTime Created { get; set; } = DateTime.Now;

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? Modified { get; set; } = null;

    [Column("modified_by")]
    public long? ModifiedBy { get; set; }
}
