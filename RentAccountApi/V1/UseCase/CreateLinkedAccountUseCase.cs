using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
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
    public class CreateLinkedAccountUseCase : ICreateLinkedAccountUseCase
    {
        private readonly ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public CreateLinkedAccountUseCase(ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public async Task<CreateLinkedAccountResponse> Execute(CreateLinkedAccountRequest createLinkedAccountRequest)
        {
            var token = await _crmTokenGateway.GetCRMToken();

            //GetAccountID from crm
            var crmAccountID = await _crmGateway.GetCrmAccountId(createLinkedAccountRequest.RentAccountNumber, token);
            //Create linked account
            if (crmAccountID == null)
            { return null; }

            //Return linked account ID
            var linkedAccountID = await _crmGateway.CreateLinkedAccount(crmAccountID, createLinkedAccountRequest.CssoId);

            if (linkedAccountID == null)
            { return null; }

            return new CreateLinkedAccountResponse
            {
                LinkedAccountId = linkedAccountID
            };
        }
    }
}
