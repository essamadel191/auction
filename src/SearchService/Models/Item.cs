using System;
using MongoDB.Entities;

namespace SearchService.Models;

public class Item : Entity
{
    // We don't need to provide id as " : Entity " will create one 
    // that related to mongodb feature itself
    //public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; } // If you use the object directly, it will result an integer in the JSON response

    // Item
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }
}
