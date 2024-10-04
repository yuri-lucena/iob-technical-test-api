using System;
using Iob.Bank.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iob.Bank.Infra.Persistence.Configurations;

public class OperationTypeConfiguration : IEntityTypeConfiguration<OperationType>
{
    public void Configure(EntityTypeBuilder<OperationType> builder)
    {
        builder.HasData(new List<OperationType>
        {
            new()
            {
                Id = 1,
                Name = "Credit",
                CreatedBy = 1
            },
            new()
            {
                Id = 2,
                Name = "Debit",
                CreatedBy = 1
            },
            new()
            {
                Id = 3,
                Name = "Transfer",
                CreatedBy = 1
            }
        });
    }
}
