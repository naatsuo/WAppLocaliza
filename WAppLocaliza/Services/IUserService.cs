using WAppLocaliza.Entities;
using WAppLocaliza.Models;

namespace WAppLocaliza.Services
{
    public interface IUserService
    {
        AuthenticateClientUserResponse? AuthenticateClient(AuthenticateClientUserRequest model);
        
        AuthenticateOperatorUserResponse? AuthenticateOperator(AuthenticateOperatorUserRequest model);
        
        void CreateClient(CreateClientUserRequest model);
        
        IEnumerable<ClientUser>? GetAll();
        
        User? GetUserById(Guid userId);

        void CreateBrand(CreateBrandRequest model);

        void CreateModel(CreateModelRequest model);
        
        void CreateCar(CreateCarRequest model);

        IEnumerable<CarBrand>? GetAllBrand();
        
        IEnumerable<CarModel>? GetAllModel();
        
        IEnumerable<Car>? GetAllCar();

        SimulateCarResponse? SimulateCar(SimulateCarRequest model);

        ScheduleCarResponse? ScheduleCar(ScheduleCarRequest model);

        void WithdrawCar(WithdrawCarRequest model);

        ReturnedCarResponse? ReturnedCar(ReturnedCarRequest model);


    }
}