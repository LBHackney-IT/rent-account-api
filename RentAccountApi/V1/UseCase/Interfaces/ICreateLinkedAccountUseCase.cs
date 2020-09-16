using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase.Interfaces
{
    public interface ICreateLinkedAccountUseCase
    {
        Task<CreateLinkedAccountResponse> Execute(CreateLinkedAccountRequest createLinkedAccountRequest);
    }
}
