using System.Text.Json.Serialization;
using MediatR;
using Scalar.AspNetCore;
using Service.Quote.Application.GetById;
using Service.Quote.Application.GetRandom;
using Service.Quote.Domain.Repository;
using Service.Quote.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetRandomCommand>();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IQuoteRepository>(provider => 
    new DapperQuoteRepository(connectionString));

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
}

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var group = app.MapGroup("/quote");

group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
    await mediator.Send(new GetByIdCommand(id)));

group.MapGet("/random", async (IMediator mediator) => 
    await mediator.Send(new GetRandomCommand()));

app.Run();