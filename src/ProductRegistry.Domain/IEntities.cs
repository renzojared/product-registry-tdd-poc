using ProductRegistry.Domain.Entities;

namespace ProductRegistry.Domain;

public interface IEntities
{
    DbSet<Product> Products { get; }
}

public interface ICommands : IEntities, IUnitOfWork;

public interface IQueries : IEntities;