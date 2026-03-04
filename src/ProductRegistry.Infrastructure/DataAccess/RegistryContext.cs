using DotEmilu.EntityFrameworkCore.Extensions;
using ProductRegistry.Domain;

namespace ProductRegistry.Infrastructure.DataAccess;

public class RegistryContext(DbContextOptions<RegistryContext> options) : DbContext(options), IEntities
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyBaseAuditableEntityConfiguration(Assembly.GetExecutingAssembly())
            .ApplyBaseEntityConfiguration(Assembly.GetExecutingAssembly())
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Product> Products => Set<Product>();
}