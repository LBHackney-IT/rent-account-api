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
    public class RentBreakdownGateway : IRentBreakdownGateway
    {
        private readonly HttpClient _client;

        public RentBreakdownGateway(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<RentBreakdown>> GetRentBreakdown(string tagRef)
        {
            var queryDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(tagRef)) queryDictionary.Add("tenancyAgreementId", tagRef);

            var rqpString = new FormUrlEncodedContent(queryDictionary).ReadAsStringAsync().Result;
            var builder = new UriBuilder
            {
                Query = rqpString
            };

            var response = await _client.GetAsync(new Uri("GetAllRentBreakdowns" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<List<RentBreakdown>>(content);

            if (results.Count > 0)
            {
                return results;
            }
            else
            {
                return null;
            }
        }
    }
}
