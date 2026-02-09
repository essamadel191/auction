using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var db = await DB.InitAsync("SearchServiceDB");
        var lastUpdated = await db.Find<Item, string>()
            .Sort(x=> x.Descending(x => x.UpdatedAt))
            .Project(x=> x.UpdatedAt.ToString("O"))
            .ExecuteFirstAsync();

        var url = _config["AuctionServiceUrl"] + "api/auctions";
        if (!string.IsNullOrWhiteSpace(lastUpdated))
        {
            url += "?date=" + Uri.EscapeDataString(lastUpdated);
        }

        return await _httpClient.GetFromJsonAsync<List<Item>>(
            url);
    }

}
