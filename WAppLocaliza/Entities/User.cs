using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
