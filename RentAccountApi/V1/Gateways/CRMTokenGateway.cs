using Newtonsoft.Json;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Gateways.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public class CRMTokenGateway : ICRMTokenGateway
    {
        private readonly HttpClient _client;

        public CRMTokenGateway(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetCRMToken()
        {
            var builder = new UriBuilder(_client.BaseAddress);
            var response = await _client.GetAsync(builder.Uri).ConfigureAwait(true);
            var key = await response.Content.ReadAsStringAsync();
            return key;
        }
    }
}
