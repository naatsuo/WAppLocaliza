using WAppLocaliza.Entities;

namespace WAppLocaliza.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Document { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Document = user.Document;
            Token = token;
        }
    }
}