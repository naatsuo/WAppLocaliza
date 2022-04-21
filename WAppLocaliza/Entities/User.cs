using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public DateTime Birthday { get; set; }

        public string ZipCode { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Username { get; set; }//
        [JsonIgnore]
        public string Password { get; set; }
        public string[] Roles { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
    }
}

