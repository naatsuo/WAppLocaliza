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
        
        int CreateBrand(CreateBrandRequest model);
        
        int CreateModel(CreateModelRequest model);
        
        int CreateCar(CreateCarRequest model);

        IEnumerable<CarBrand>? GetAllBrand();
        
        IEnumerable<CarModel>? GetAllModel();
        
        IEnumerable<Car>? GetAllCar();

    }
}