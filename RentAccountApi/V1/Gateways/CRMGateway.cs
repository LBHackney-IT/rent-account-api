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
    public class CRMGateway : ICRMGateway
    {
        private readonly HttpClient _client;

        public CRMGateway(HttpClient client)
        {
            _client = client;
        }

        public async Task<CheckAccountExistsResponse> CheckAccountExists(string paymentReference, string postcode, string token)
        {
            var fetchXML = FetchXMLBuilder.BuildCheckAccountExistsFetchXML(paymentReference, postcode);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };
            _client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await _client.GetAsync(new Uri("accounts" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<CrmResponse>(content);

            if (results.value.Count > 0)
            {
                return new CheckAccountExistsResponse
                {
                    Exists = true
                };
            }
            else
            {
                return new CheckAccountExistsResponse
                {
                    Exists = false
                };
            }
        }

        public async Task<CrmRentAccountResponse> GetRentAccount(string paymentReference, string token)
        {
            var fetchXML = FetchXMLBuilder.BuildGetRentAccountFetchXML(paymentReference);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };
            _client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await _client.GetAsync(new Uri("accounts" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var result = JsonConvert.DeserializeObject<CrmRentAccountResponse>(content);
            return result;
        }

        public async Task<CrmLinkedAccountResponse> GetLinkedAccount(string cssoId, string token)
        {
            var fetchXML = FetchXMLBuilder.BuildGetLinkedAccountFetchXML(cssoId);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };
            _client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await _client.GetAsync(new Uri("hackney_csso_linked_rent_accounts" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var result = JsonConvert.DeserializeObject<CrmLinkedAccountResponse>(content);
            return result;
        }
    }
}
