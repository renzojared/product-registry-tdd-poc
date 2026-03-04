using ProductRegistry.Domain;

namespace ProductRegistry.Infrastructure.DataAccess.Repositories;

internal sealed class Commands(DbContextOptions<RegistryContext> options) : RegistryContext(options), ICommands;