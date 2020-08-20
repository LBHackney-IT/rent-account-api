using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase
{
    public class PostAuditUseCase : IPostAuditUseCase
    {
        private IAuditDatabaseGateway _gateway;
        public PostAuditUseCase(IAuditDatabaseGateway gateway)
        {
            _gateway = gateway;
        }

        public void Execute(AuditRequestObject auditRequest)
        {
            //TODO: workout what response we get from DynamoDB when we put an object
            _gateway.GenerateAuditRecord(auditRequest);
        }
    }
}
