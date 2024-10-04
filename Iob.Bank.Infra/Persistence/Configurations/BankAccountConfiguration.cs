using Iob.Bank.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iob.Bank.Infra.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasData(new List<BankAccount>
        {
            new BankAccount
            {
                Id = 1,
                Balance = 0,
                UserId = 1,
                CreatedBy = 1,
                Type = "Conta Corrente",
                Active = true
            },
            new BankAccount
            {
                Id = 2,
                Balance = 1000,
                UserId = 2,
                CreatedBy = 1,
                Type = "Conta Corrente",
                Active = true
            }
        });
    }
}
