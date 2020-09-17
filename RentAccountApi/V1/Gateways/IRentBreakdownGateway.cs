using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public interface IRentBreakdownGateway
    {
        Task<List<RentBreakdown>> GetRentBreakdown(string tagRef);
    }
}
