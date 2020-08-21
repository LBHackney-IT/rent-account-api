using System.Collections.Generic;
using RentAccountApi.V1.Boundary.Request;
using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Infrastructure;
using System;
using Amazon.DynamoDBv2.DocumentModel;
using RentAccountApi.V1.Boundary;

namespace RentAccountApi.V1.Gateways
{
    public class AuditDatabaseGateway : IAuditDatabaseGateway
    {
        private IDynamoDBContext<MyRentAccountAudit> _auditDbContext;

        public AuditDatabaseGateway(IDynamoDBContext<MyRentAccountAudit> auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }

        public void GenerateAuditRecord(MyRentAccountAudit generateAuditRequest)
        {
            _auditDbContext.Save(generateAuditRequest);
        }
    }
}
