using RentAccountApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase.Interfaces
{
    public interface IGetLinkedAccountUseCase
    {
        Task<LinkedAccountResponse> Execute(string cssoId);
    }
}
