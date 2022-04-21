using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public sealed class ClientUser : User
    {
        [Required]
        [Column(TypeName = "VARCHAR(14)")] // 000.000.000-00
        public string Document { get; set; }

        public DateTime? Birthday { get; set; }
        public string? ZipCode { get; set; }
        public string? Address { get; set; }
        public int? AddressNumber { get; set; }
        public string? Complement { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [Required]
        public string? Email { get; set; }
        [Required][Column(TypeName = "BIT")] 
        public bool EmailConfirmed { get; set; }
    }
}

