using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase
{
    public class GetRentBreakdownUseCase : IGetRentBreakdownUseCase
    {
        private readonly IRentBreakdownGateway _rentBreakdownGateway;

        public GetRentBreakdownUseCase(IRentBreakdownGateway rentBreakdownGateway)
        {
            _rentBreakdownGateway = rentBreakdownGateway;
        }

        public async Task<List<RentBreakdown>> Execute(string tagRef)
        {

            var rentBreakdownResponse = await _rentBreakdownGateway.GetRentBreakdown(tagRef);

            return rentBreakdownResponse;
        }
    }
}
