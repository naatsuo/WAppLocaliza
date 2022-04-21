using WAppLocaliza.Entities;
using WAppLocaliza.Models;

namespace WAppLocaliza.Services
{
    public interface IUserService
    {
        AuthenticateClientUserResponse? AuthenticateClient(AuthenticateClientUserRequest model);
        AuthenticateOperatorUserResponse? AuthenticateOperator(AuthenticateOperatorUserRequest model);
        int CreateClient(CreateClientUserRequest model);
        IEnumerable<ClientUser>? GetAll();
        User? GetById(Guid userId);
    }
}