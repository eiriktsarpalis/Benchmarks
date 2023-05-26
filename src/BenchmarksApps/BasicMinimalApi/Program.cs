using System.Text.Json.Serialization;
using BasicMinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
});

var app = builder.Build();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => Todos.AllTodos);

// Keeping because it is in the template but not actually benchmarked.
todosApi.MapGet("/{id}", (int id) =>
    Todos.AllTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}