using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Iob.Bank.Infra;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        SetData();

        return base.SaveChanges();
    }

    private void SetData()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
                entry.Property("Created").CurrentValue = DateTime.Now;
            else if (entry.State == EntityState.Modified)
                entry.Property("Modified").CurrentValue = DateTime.Now;
        }
    }
}