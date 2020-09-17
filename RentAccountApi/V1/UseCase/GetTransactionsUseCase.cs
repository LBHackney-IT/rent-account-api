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
    public class GetTransactionsUseCase : IGetTransactionsUseCase
    {
        private readonly ITransactionsGateway _transactionsGateway;

        public GetTransactionsUseCase(ITransactionsGateway transactionsGateway)
        {
            _transactionsGateway = transactionsGateway;
        }

        public async Task<TransactionsResponse> Execute(string accountNumber, string postCode)
        {
            var transactionsResponse = await _transactionsGateway.GetTransactions(accountNumber, postCode);
            return transactionsResponse;
        }
    }
}
