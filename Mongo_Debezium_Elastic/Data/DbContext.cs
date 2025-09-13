using Bogus;
using Mongo_Monstache_Elastic.Data.Models;
using MongoDB.Driver;
using Nest;

namespace Mongo_Monstache_Elastic.Data;
public sealed class DbContext
{

    private readonly IMongoDatabase _db;
    public IMongoDatabase Database => _db;
    private readonly ElasticClient _elasticClient;
    public ElasticClient ElasticClient => _elasticClient;

    public DbContext(IMongoClient client, ElasticClient elasticClient)
    {
        _db = client.GetDatabase("UsersDb");
        _elasticClient = elasticClient;
    }

    public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
    public List<User> FakedUsers()
    {
        var faker = new Faker<User>()
    .RuleFor(u => u.Id, f => Guid.NewGuid())
    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
    .RuleFor(u => u.LastName, f => f.Name.LastName())
    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
    .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18))) // age 18–48
    .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
    .RuleFor(u => u.Address, f => f.Address.FullAddress());

        return faker.Generate(100);

    }
}
