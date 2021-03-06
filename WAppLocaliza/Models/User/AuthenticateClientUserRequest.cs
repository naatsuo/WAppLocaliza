using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class AuthenticateClientUserRequest
    {
        [Required]
        public string Document { get; set; }
        [Required]
        public string Password { get; set; }
    }
}