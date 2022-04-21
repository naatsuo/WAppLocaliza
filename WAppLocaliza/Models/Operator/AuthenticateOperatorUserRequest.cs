using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class AuthenticateOperatorUserRequest
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Password { get; set; }
    }
}