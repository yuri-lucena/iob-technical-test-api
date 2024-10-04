using Iob.Bank.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iob.Bank.Infra.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new List<User>
            {
                new()
                {
                    Id = 1,
                    Name = "Yuri",
                    Birthday = new DateTime(2002, 11, 30),
                    Identifier = "12312312323",
                    UserTypeId = 1,
                    Email = "yuri@iob.com",
                    Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3"
                },
                new()
                {
                    Id = 2,
                    Name = "Tata",
                    Birthday = new DateTime(2003, 04, 05),
                    Identifier = "12312312323",
                    UserTypeId = 1,
                    Email = "tata@iob.com",
                    Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3"
                }
            }
        );
    }
}
