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
    public class GetRentAccountUseCase : IGetRentAccountUseCase
    {
        private readonly ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public GetRentAccountUseCase(ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public async Task<RentAccountResponse> Execute(string paymentReference, bool privacy)
        {
            var token = await _crmTokenGateway.GetCRMToken();
            var crmResponse = await _crmGateway.GetRentAccount(paymentReference, token);
            var rentAccountResponse = CRMFactory.ToResponse(paymentReference, crmResponse, privacy);
            return rentAccountResponse;
        }
    }
}
