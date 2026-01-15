
using System.Text.Json;

using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

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

        var count = await db.CountEstimatedAsync<Item>();
        
        Console.WriteLine($"Item count: {count}");
        
        if (count == 0)
        {
            Console.WriteLine("No data - attempting to seed");
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            if (items != null && items.Count > 0)
            {
                await db.SaveAsync(items);
                Console.WriteLine($"Seeded {items.Count} items to database");
            }
        }

        
    }
}
