using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using Amazon.DynamoDBv2.Model;

namespace RentAccountApi.V1.UseCase
{
    public class GetAuditByUserUseCase : IGetAuditByUserUseCase
    {
        private IAuditDatabaseGateway _gateway;

        public GetAuditByUserUseCase(IAuditDatabaseGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<GetAllAuditsResponse> GetAuditByUser(string userEmail)
        {            
            var envRecordLimit = Environment.GetEnvironmentVariable("AUDIT_RECORD_LIMIT");
            int recordLimit = envRecordLimit != null ? int.Parse(envRecordLimit) : 20;
            
            var auditRecords = await _gateway.GetAuditByUser(userEmail.ToLower(), recordLimit);
            //TODO: check querstring values are correct
            return AuditFactory.ToGetAllAuditsResponse(auditRecords);
        }
    }
}
