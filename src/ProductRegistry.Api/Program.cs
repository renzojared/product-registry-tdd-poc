using System.Globalization;
using ProductRegistry.Api;
using ProductRegistry.Application;
using ProductRegistry.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration, Assembly.GetExecutingAssembly())
        .AddApi(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        await app.Services.ApplyMigrationsAsync();
    }

    app.UseHttpsRedirection();
    app.UseSerilogMiddleware();
    app.MapEndpoints();

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}