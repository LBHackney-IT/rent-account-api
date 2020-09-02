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

        public async Task<CheckAccountExistsResponse> CheckAccountExists(string paymentReference, string postcode)
        {
            var fetchXML = FetchXMLBuilder.BuildGetTenancyRefFetchXML(paymentReference, postcode);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };
            _client.DefaultRequestHeaders.Add("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImppYk5ia0ZTU2JteFBZck45Q0ZxUms0SzRndyIsImtpZCI6ImppYk5ia0ZTU2JteFBZck45Q0ZxUms0SzRndyJ9.eyJhdWQiOiJodHRwczovL2xiaGFja25leS5jcm0xMS5keW5hbWljcy5jb20vIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMWM2ODUyNTItZmI5MC00MDQyLWE5YzQtMjk0YmI3YWJhMjYzLyIsImlhdCI6MTU5OTA0NzQ4MywibmJmIjoxNTk5MDQ3NDgzLCJleHAiOjE1OTkwNTEzODMsImFpbyI6IkUyQmdZRGljUGZNbm8wMUluOE9EOHp2Tkw1dHlBZ0E9IiwiYXBwaWQiOiI2ZDA5OTUxZC1jZDg0LTRjMDQtYmYyZC1iN2E0N2NmYjEzZWEiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8xYzY4NTI1Mi1mYjkwLTQwNDItYTljNC0yOTRiYjdhYmEyNjMvIiwib2lkIjoiZjlkM2E2N2UtMGFjMC00NmUzLTgwYjgtMmRhMTU2ZDU4NDEwIiwicmgiOiIwLkFBQUFVbEpvSEpEN1FrQ3B4Q2xMdDZ1aVl4MlZDVzJFelFSTXZ5MjNwSHo3RS1wY0FBQS4iLCJzdWIiOiJmOWQzYTY3ZS0wYWMwLTQ2ZTMtODBiOC0yZGExNTZkNTg0MTAiLCJ0aWQiOiIxYzY4NTI1Mi1mYjkwLTQwNDItYTljNC0yOTRiYjdhYmEyNjMiLCJ1dGkiOiIxWVdKSDZQR1NFR1BhMEpRRi1VdkFBIiwidmVyIjoiMS4wIn0.iJdrcnRSHPQUvZXjew_vKT2SVsWzRrOr7CV5iSoHAhTyTZXPKaJAK2YFmxJxHkiFY-v01dRIUldfj5mgNsP3yswLu9mb22B27QFB2-bmqPXFjW7CF1KRPBLNy72l8ORA0zwvOOjDp_VuCvlq6YWhJkrrXOwGh3agCsf2_rV9m1yS2_MEHMpBa5ua8w0j1HwPaWpZrKI_q8BWYXgc-lU-oNwbgTL1uKDSbabBOvywejrdU4b-FtRvFrlSvfku9kTWZqEw27Wl8fWBkEGDaEJygnUOIr6szOiEyfYygBYyzfoon-ThLx1V2dsT0IvY_4WQoRSdKcIA64cprEZ-ONQLxQ");// need to get the CRMToken here.
            
            var response = await _client.GetAsync(new Uri("accounts" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<CrmResponse>(content);

            if(results.value.Count > 0)
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
    }
}
