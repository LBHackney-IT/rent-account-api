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
    public class GetLinkedAccountUseCase : IGetLinkedAccountUseCase
    {
        private readonly ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public GetLinkedAccountUseCase(ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public async Task<LinkedAccountResponse> Execute(string cssoId)
        {
            var token = await _crmTokenGateway.GetCRMToken();
            var crmResponse = await _crmGateway.GetLinkedAccount(cssoId, token);
            var linkedAccountResponse = crmResponse.value.Count > 0 ? CRMFactory.ToLinkedAccountResponse(crmResponse) : null;
            return linkedAccountResponse;
        }
    }
}
