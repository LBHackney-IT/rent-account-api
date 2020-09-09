using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Boundary.Response;

namespace RentAccountApi.V1.UseCase
{
    public class PostAuditUseCase : IPostAuditUseCase
    {
        private IAuditDatabaseGateway _gateway;
        private ICRMGateway _crmGateway;
        private readonly ICRMTokenGateway _crmTokenGateway;

        public PostAuditUseCase(IAuditDatabaseGateway gateway, ICRMGateway crmGateway, ICRMTokenGateway crmTokenGateway)
        {
            _gateway = gateway;
            _crmGateway = crmGateway;
            _crmTokenGateway = crmTokenGateway;
        }

        public void CreateAdminAudit(CreateAdminAuditRequest auditRequest)
        {
            _gateway.GenerateAdminAuditRecord(AuditFactory.ToAdminAuditRequest(auditRequest));
        }

        public async Task<AddResidentAuditResponse> CreateResidentAudit(CreateResidentAuditRequest residentAuditRequest)
        {
            var token = await _crmTokenGateway.GetCRMToken();
            var residentAuditResponse = await _crmGateway.GenerateResidentAuditRecord(AuditFactory.ToResidentAuditRequest(residentAuditRequest), token);
            var addResidentAuditResponse = new AddResidentAuditResponse
            {
                success = residentAuditResponse
            };
            return addResidentAuditResponse;
        }
    }
}
