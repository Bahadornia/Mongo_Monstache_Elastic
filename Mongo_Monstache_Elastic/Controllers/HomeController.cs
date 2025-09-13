using Microsoft.AspNetCore.Mvc;
using Mongo_Monstache_Elastic.Data;
using Mongo_Monstache_Elastic.Data.Models;
using Mongo_Monstache_Elastic.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Nest;
using System.Diagnostics;

namespace Mongo_Monstache_Elastic.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContext _dbContext;

        public HomeController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term, CancellationToken ct)
        {
            var pattern = $"^{term}";
            var filter = Builders<User>.Filter.Regex(user => user.FirstName, new BsonRegularExpression(pattern));
            var users = await _dbContext.Users.Find(filter, null).ToListAsync(ct);
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> ElasticSearch([FromQuery] string term, CancellationToken ct)
        {
            ISearchResponse<UserDto> response;

            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    response = await _dbContext.ElasticClient.SearchAsync<UserDto>(u =>
                    u.Query(q => q.MatchAll()).Size(50));
                }
                else
                {
                    response = await _dbContext.ElasticClient.SearchAsync<UserDto>(u =>

                    u.Index("usersdb.users")
                   .Query(q => q
                        .Match(m => m
                    .Field(f => f.FirstName)
                    .Query(term)
                    .Fuzziness(Fuzziness.Auto)
                    .PrefixLength(1))));


                }
                if (!response.IsValid)
                {
                    return Ok(new List<UserDto>());
                }
                return Ok(response.Hits.Select(item => new UserDto
                {
                    Id = item.Id,
                    FirstName = item.Source.FirstName,
                    LastName = item.Source.LastName,
                    Address = item.Source.Address,
                    BirthDate = item.Source.BirthDate,
                    Email = item.Source.Email,
                    PhoneNumber = item.Source.PhoneNumber,
                }));
            }
            catch (Exception)
            {

                return Ok(new List<UserDto>());
            }

        }
    }
}
