using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase
{
    public class CheckRentAccountExistsUseCase : ICheckRentAccountExistsUseCase
    {
        private readonly ICRMGateway _crmGateway;

        public CheckRentAccountExistsUseCase(ICRMGateway crmGateway)
        {
            _crmGateway = crmGateway;
        }

        public async Task<CheckAccountExistsResponse> Execute(string paymentReference, string postCode)
        {
            var normalizedPostcode = CRMFactory.NormalizePostcode(postCode);
            var accountExistsResponse =  await _crmGateway.CheckAccountExists(paymentReference, normalizedPostcode);
            return accountExistsResponse;
        }
    }
}
