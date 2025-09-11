using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mongo_Debezium_Elastic.Data;
using Mongo_Debezium_Elastic.Data.Models;
using Mongo_Debezium_Elastic.Models;
using MongoDB.Driver;

namespace Mongo_Debezium_Elastic.Controllers
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
            var filter = Builders<User>.Filter.Empty;
            var products = await _dbContext.Users.Find(filter, null).ToListAsync(ct);
            return View(products);
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
    }
}
