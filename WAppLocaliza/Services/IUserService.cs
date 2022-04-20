using WAppLocaliza.Entities;
using WAppLocaliza.Models;

namespace WAppLocaliza.Services
{
    public interface IUserService
    {
        AuthenticateResponse? Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User? GetById(Guid userId);
    }
}