using Iob.Bank.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iob.Bank.Infra.Persistence.Configurations;

public class BankLaunchConfiguration : IEntityTypeConfiguration<BankLaunch>
{
    public void Configure(EntityTypeBuilder<BankLaunch> builder)
    {
        builder
            .HasOne(bl => bl.OriginAccount)
            .WithMany()
            .HasPrincipalKey(a => a.Id)
            .HasForeignKey(bl => bl.OriginAccountId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder
            .HasOne(bl => bl.DestinationAccount)
            .WithMany()
            .HasPrincipalKey(a => a.Id)
            .HasForeignKey(bl => bl.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder
            .HasOne(bl => bl.OperationType)
            .WithMany()
            .HasForeignKey(bl => bl.OperationTypeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
