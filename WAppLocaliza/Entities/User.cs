using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(60)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(14)")] // 000.000.000-00
        public string Document { get; set; }
        public DateTime Birthday { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Required]
        public string Email { get; set; }
        [Required][Column(TypeName = "BIT")] 
        public bool EmailConfirmed { get; set; }
        
        [Required]
        [JsonIgnore]
        [Column(TypeName = "VARCHAR(128)")] // SHA512
        public string Password { get; set; }
        public string[] Roles { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
    }
}

