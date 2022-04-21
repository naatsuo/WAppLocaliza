using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class CreateClientUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Document { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}