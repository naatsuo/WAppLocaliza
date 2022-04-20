using WAppLocaliza.Entities;

namespace WAppLocaliza.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}