using ProductRegistry.Api;
using ProductRegistry.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogMiddleware();

await app.RunAsync();
