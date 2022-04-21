using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class Operator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Number { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
        public string[] Roles { get; set; }
    }
}

