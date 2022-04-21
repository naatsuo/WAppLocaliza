using WAppLocaliza.Entities;

namespace WAppLocaliza.Models
{
    public class AuthenticateOperatorUserResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Token { get; set; }
        public AuthenticateOperatorUserResponse(OperatorUser user, string token)
        {
            Id = user.Id;
            Number = user.Number;
            Token = token;
        }
    }
}