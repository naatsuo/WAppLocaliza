using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public abstract class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [JsonIgnore]
        [Column(TypeName = "VARCHAR(128)")] // SHA512
        public string Password { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(60)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
        public string[] Roles { get; set; }

    }
}