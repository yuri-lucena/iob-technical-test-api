namespace Iob.Bank.Domain.Data.Dtos.Base;

public class BaseDto
{
    public virtual long Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public long CreatedBy { get; set; }
    public DateTime? Modified { get; set; } = null;
    public long? ModifiedBy { get; set; }
}
