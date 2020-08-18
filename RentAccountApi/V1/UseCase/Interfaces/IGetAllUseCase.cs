using RentAccountApi.V1.Boundary.Response;

namespace RentAccountApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        ResponseObjectList Execute();
    }
}
