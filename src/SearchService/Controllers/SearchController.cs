using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Entities;
using SearchService.Models;

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
        public async Task <ActionResult<List<Item>>> SearchItmes(string searchTerm, int pageNumber = 1 , int pageSize = 4)
        {
            var db = await DB.InitAsync("SearchServiceDB");
            
            var query = db.PagedSearch<Item>();

            query.Sort(x => x.Ascending(a => a.Make));


            if(!string.IsNullOrEmpty(searchTerm))
            {
                // Perform text search across Make, Model, and Color fields.
                query.Match(Search.Full,searchTerm).SortByTextScore();
            }

            query.PageNumber(pageNumber);
            query.PageSize(pageSize);

            var result = await query.ExecuteAsync();

            return Ok(new
                {
                    results = result.Results,
                    pageCount = result.PageCount,
                    totalCount = result.TotalCount
                }
            );
        }
    }
}
