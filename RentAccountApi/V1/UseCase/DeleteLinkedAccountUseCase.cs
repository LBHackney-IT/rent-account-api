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
    public class DeleteLinkedAccountUseCase : IDeleteLinkedAccountUseCase
    {
        private readonly ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public DeleteLinkedAccountUseCase(ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public async Task<DeleteLinkedAccountResponse> Execute(string cssoId)
        {
            var token = await _crmTokenGateway.GetCRMToken();
            var crmResponse = await _crmGateway.GetLinkedAccount(cssoId, token);
            if (crmResponse == null || crmResponse.value.Count == 0)
            {
                return null;
            }
            var linkId = crmResponse.value[0].hackney_csso_linked_rent_accountid;
            var crmDeleteResponse = await _crmGateway.DeleteLinkedAccount(linkId);
            var deleteLinkedAccountResponse = new DeleteLinkedAccountResponse
            {
                success = crmDeleteResponse
            };
            return deleteLinkedAccountResponse;
        }
    }
}
