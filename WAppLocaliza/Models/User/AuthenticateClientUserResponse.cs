using WAppLocaliza.Entities;

namespace WAppLocaliza.Models
{
    public class AuthenticateClientUserResponse
    {
        public Guid Id { get; set; }
        public string Document { get; set; }
        public string Token { get; set; }
        public AuthenticateClientUserResponse(ClientUser user, string token)
        {
            Id = user.Id;
            Document = user.Document;
            Token = token;
        }
    }
}