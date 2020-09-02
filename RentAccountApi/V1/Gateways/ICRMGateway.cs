using RentAccountApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public interface ICRMGateway
    {
        Task<CheckAccountExistsResponse> CheckAccountExists(string paymentReference, string postcode);
    }
}
