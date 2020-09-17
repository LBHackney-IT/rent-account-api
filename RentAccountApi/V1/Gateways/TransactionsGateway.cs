using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public class TransactionsGateway : ITransactionsGateway
    {
        private readonly HttpClient _client;

        public TransactionsGateway(HttpClient client)
        {
            _client = client;
        }

        public async Task<TransactionsResponse> GetTransactions(string accountNumber, string postCode)
        {
            var response = await _client.GetAsync(new Uri($"transactions/payment-ref/{accountNumber}/post-code/{postCode}", UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<TransactionsResponse>(content);
            return results;
        }
    }
}
