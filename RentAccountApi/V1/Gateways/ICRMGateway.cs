using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public interface ICRMGateway
    {
        Task<CheckAccountExistsResponse> CheckAccountExists(string paymentReference, string postcode, string token);
        Task<CrmRentAccountResponse> GetRentAccount(string paymentReference, string token);
        Task<CrmLinkedAccountResponse> GetLinkedAccount(string cssoId, string token);
        Task<bool> DeleteLinkedAccount(string linkId);
        Task<bool> GenerateResidentAuditRecord(MyRentAccountResidentAudit myRentAccountResidentAudit, string token);
        Task<string> GetCrmAccountId(string rentAccountNumber, string token);
        Task<string> CreateLinkedAccount(string crmAccountID, string cssoId);
        Task<int?> GetUniqueAnonymousUsers(UsageReportRequest usageReportRequest, string token);
        Task<int?> GetTotalAnonymousLogins(UsageReportRequest usageReportRequest);
        Task<int?> GetUniqueCSSOUsers(UsageReportRequest usageReportRequest);
        Task<int?> GetTotalCSSOLogins(UsageReportRequest usageReportRequest);
        Task<int?> GetNewCSSOLinkedAccounts(UsageReportRequest usageReportRequest);
        Task<int?> GetTotalLogins(UsageReportRequest usageReportRequest);
    }
}
