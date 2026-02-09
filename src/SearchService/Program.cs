using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

//app.Lifetime.ApplicationStarted

try
{
    await Dbinitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine($"An error occurred seeding the DB: {e.Message}");
}

app.Run();

// That what will handle the exception if the auction service is down
// and will keep retrying every 3 seconds 
// then it will make a sucess request and stop trying

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
