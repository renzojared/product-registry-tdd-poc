namespace ProductRegistry.Application;

public static class DiContainer
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddVerifier()
            .AddHandlers(Assembly.GetExecutingAssembly())
            .AddChainHandlers(Assembly.GetExecutingAssembly());
}