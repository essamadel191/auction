using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

try
{
    await Dbinitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine($"An error occurred seeding the DB: {e.Message}");
}

app.Run();
