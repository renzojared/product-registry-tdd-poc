using ProductRegistry.Domain;

namespace ProductRegistry.Infrastructure.DataAccess.Repositories;

internal sealed class Queries : RegistryContext, IQueries
{
    public Queries(DbContextOptions<RegistryContext> options) : base(options)
        => ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
}