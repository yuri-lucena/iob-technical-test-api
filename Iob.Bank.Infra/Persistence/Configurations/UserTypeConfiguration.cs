using Iob.Bank.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iob.Bank.Infra.Persistence.Configurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.HasData(new List<UserType>{
            new UserType
            {
                Id = 1,
                Type = "Administrator",
                Description = "System administrator",
                CreatedBy = 1
            },
            new UserType
            {
                Id = 2,
                Type = "Customer",
                Description = "Customer",
                CreatedBy = 1
            }
        });
    }
}
