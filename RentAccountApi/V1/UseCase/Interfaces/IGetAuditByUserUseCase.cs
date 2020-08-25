using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase.Interfaces
{
    public interface IGetAuditByUserUseCase
    {
        Task<GetAllAuditsResponse> GetAuditByUser(string userEmail);
    }
}
