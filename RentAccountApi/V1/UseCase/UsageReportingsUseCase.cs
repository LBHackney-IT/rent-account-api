using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase
{
    public class UsageReportingsUseCase : IUsageReportingsUseCase
    {
        private readonly ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public UsageReportingsUseCase(ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public async Task<UsageReportResponse> Execute(UsageReportRequest usageReportRequest)
        {
            CRMFactory.CheckUsageReportRequest(usageReportRequest);
            var token = await _crmTokenGateway.GetCRMToken();
            var uniqueAnonymousUsers = await _crmGateway.GetUniqueAnonymousUsers(usageReportRequest, token);
            var totalAnonymousLogins = await _crmGateway.GetTotalAnonymousLogins(usageReportRequest);
            var uniqueCSSOUsers = await _crmGateway.GetUniqueCSSOUsers(usageReportRequest);
            var totalCSSOLogins = await _crmGateway.GetTotalCSSOLogins(usageReportRequest);
            var newCSSOLinkedAccounts = await _crmGateway.GetNewCSSOLinkedAccounts(usageReportRequest);
            var totalLogins = await _crmGateway.GetTotalLogins(usageReportRequest);

            return new UsageReportResponse
            {
                StartDate = usageReportRequest.StartDate.ToString("yyyy-MM-dd"),
                EndDate = usageReportRequest.EndDate.ToString("yyyy-MM-dd"),
                UniqueAnonymousUsers = uniqueAnonymousUsers,
                TotalAnonymousLogins = totalAnonymousLogins,
                UniqueCSSOUsers = uniqueCSSOUsers,
                TotalCSSOLogins = totalCSSOLogins,
                NewCSSOLinkedAccounts = newCSSOLinkedAccounts,
                TotalLogins = totalLogins
            };
        }
    }
}
