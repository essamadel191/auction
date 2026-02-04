
using System.Text.Json;

using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class Dbinitializer
{
    public static async Task InitDb(WebApplication app)
    {
        
        // Initialize MongoDB database.
        // All Funtionality provided by MongoDB.Entities is effectively static class.
        // So we just need to create a new instance of mongoDB entities.
        // 
        var db = await DB.InitAsync("SearchServiceDB",
                MongoClientSettings.FromConnectionString(
                    app.Configuration.GetConnectionString("MongoDbConnection")));

        await db.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await db.CountAsync<Item>();
        
        Console.WriteLine($"Item count: {count}");
        
        if (count == 0)
        {
            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

            var items = await httpClient.GetItemsForSearchDb();

            Console.WriteLine($"Fetched {items.Count} items from Auction Service");

            if(items.Count > 0)
            {
                await db.SaveAsync(items);
                Console.WriteLine("Seeded SearchServiceDB with items from Auction Service");
            }
            
        }

        
    }
}
