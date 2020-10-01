using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
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

        public async Task<bool> DeleteLinkedAccount(string linkId)
        {
            var response = await _client.DeleteAsync(new Uri($"hackney_csso_linked_rent_accounts({linkId})", UriKind.Relative)).ConfigureAwait(true);
            var status = (int) response.StatusCode;
            return status == 204 ? true : false;
        }

        public async Task<bool> GenerateResidentAuditRecord(MyRentAccountResidentAudit myRentAccountResidentAudit, string token)
        {
            var content = new StringContent(JsonConvert.SerializeObject(myRentAccountResidentAudit), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _client.PostAsync(new Uri($"hackney_housingaccountaudits()", UriKind.Relative), content).ConfigureAwait(true);
            var status = (int) response.StatusCode;
            return status == 204 ? true : false;
        }

        public async Task<string> GetCrmAccountId(string rentAccountNumber, string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _client.GetAsync(new Uri($"accounts?$select=accountid&$filter=housing_u_saff_rentacc eq '{rentAccountNumber}'", UriKind.Relative)).ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["accountid"].ToString();
                return accountId;
            }
            return null;
        }

        public async Task<string> CreateLinkedAccount(string crmAccountID, string cssoId)
        {
            _client.DefaultRequestHeaders.Add("Prefer", "return=representation");
            var linkedAccountObject = CRMFactory.BuildLinkAccountObj(crmAccountID, cssoId);
            var content = new StringContent(linkedAccountObject.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"hackney_csso_linked_rent_accounts?$select=hackney_csso_linked_rent_accountid", UriKind.Relative), content).ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse.HasValues)
            {
                return jsonResponse["hackney_csso_linked_rent_accountid"].ToString();
            }
            return null;
        }

        public async Task<int?> GetUniqueAnonymousUsers(UsageReportRequest usageReportRequest, string token)
        {
            var fetchXML = FetchXMLBuilder.BuildResidentAuditFetchXML(true, "Anonymous", usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };
            _client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await _client.GetAsync(new Uri("hackney_housingaccountaudits" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var recordCount = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(recordCount);
            }
            return null;
        }

        public async Task<int?> GetTotalAnonymousLogins(UsageReportRequest usageReportRequest)
        {
            var fetchXML = FetchXMLBuilder.BuildResidentAuditFetchXML(false, "Anonymous", usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };

            var response = await _client.GetAsync(new Uri("hackney_housingaccountaudits" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(accountId);
            }
            return null;
        }

        public async Task<int?> GetUniqueCSSOUsers(UsageReportRequest usageReportRequest)
        {
            var fetchXML = FetchXMLBuilder.BuildResidentAuditFetchXML(true, "One Account", usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };

            var response = await _client.GetAsync(new Uri("hackney_housingaccountaudits" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(accountId);
            }
            return null;
        }

        public async Task<int?> GetTotalCSSOLogins(UsageReportRequest usageReportRequest)
        {
            var fetchXML = FetchXMLBuilder.BuildResidentAuditFetchXML(false, "One Account", usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };

            var response = await _client.GetAsync(new Uri("hackney_housingaccountaudits" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(accountId);
            }
            return null;
        }

        public async Task<int?> GetNewCSSOLinkedAccounts(UsageReportRequest usageReportRequest)
        {
            var fetchXML = FetchXMLBuilder.BuildLinkedAccountReportFetchXML(usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };

            var response = await _client.GetAsync(new Uri("hackney_csso_linked_rent_accounts" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(accountId);
            }
            return null;
        }

        public async Task<int?> GetTotalLogins(UsageReportRequest usageReportRequest)
        {
            var fetchXML = FetchXMLBuilder.BuildResidentAuditFetchXML(true, "", usageReportRequest);
            var builder = new UriBuilder()
            {
                Query = fetchXML
            };

            var response = await _client.GetAsync(new Uri("hackney_housingaccountaudits" + builder.Query, UriKind.Relative)).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            if (jsonResponse["value"].HasValues)
            {
                var accountId = jsonResponse["value"].FirstOrDefault()["recordcount"].ToString();
                return Int32.Parse(accountId);
            }
            return null;
        }
    }
}
