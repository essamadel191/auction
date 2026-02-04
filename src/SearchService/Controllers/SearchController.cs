using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHandlers;

namespace SearchService.Controllers
{
    [Route("api/Search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("Search Service is healthy");
        }
        
        [HttpGet]
        public async Task <ActionResult<List<Item>>> SearchItmes([FromQuery]SearchParams searchParams)
        {
            var db = await DB.InitAsync("SearchServiceDB");
            
            var query = db.PagedSearch<Item,Item>();

            query.Sort(x => x.Ascending(a => a.Make));


            if(!string.IsNullOrEmpty(searchParams.SearchTerms))
            {
                // Perform text search across Make, Model, and Color fields.
                query.Match(Search.Full,searchParams.SearchTerms).SortByTextScore();
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(a => a.Make)),
                "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)) 
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(x => x.AuctionEnd < DateTime.Now),
                "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.Now.AddHours(6) && x.AuctionEnd > DateTime.Now),
                _ => query.Match(x => x.AuctionEnd > DateTime.Now)
            };

            if(!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Seller == searchParams.Seller);
            }
            if(!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Winner == searchParams.Winner);
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);
            
            var result = await query.ExecuteAsync();

            return Ok(new
                {
                    results = result.Results ?? new List<Item> { new Item { Make = "Not Found", Model = "", Color = "", Mileage = 0, Year = 0, ImageUrl = "" } },
                    pageCount = result.PageCount,
                    totalCount = result.TotalCount
                }
            );
        }
    }
}
