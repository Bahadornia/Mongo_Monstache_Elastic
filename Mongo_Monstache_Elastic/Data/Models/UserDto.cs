using Nest;

namespace Mongo_Monstache_Elastic.Data.Models
{
    public class UserDto
    {
        [PropertyName("Id")]
        public string Id
        {
            get; 
            
            set;
        }  // Mongo will use ObjectId, but you can also use Guid


        [PropertyName("FirstName")]
        public string FirstName { get; set; } = default!;

        [PropertyName("LastName")]
        public string LastName { get; set; } = default!;

        [PropertyName("Email")]
        public string Email { get; set; } = default!;

        [PropertyName("BirthDate")]
        public DateTime BirthDate { get; set; }

        [PropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; } = default!;

        [PropertyName("Address")]
        public string Address { get; set; } = default!;
    }
}
