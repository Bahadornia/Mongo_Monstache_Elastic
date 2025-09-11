using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo_Debezium_Elastic.Data.Models;

public class User
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }  // Mongo will use ObjectId, but you can also use Guid
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
}
