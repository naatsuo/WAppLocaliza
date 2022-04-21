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
        [Required]
        public string Number { get; set; }
        [JsonIgnore]
        [Column(TypeName = "VARCHAR(128)")] // SHA512
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
        public string[] Roles { get; set; }
    }
}

