using System.Collections.Generic;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Infrastructure;

namespace RentAccountApi.V1.Gateways
{
    //TODO: Rename to match the data source that is being accessed in the gateway eg. MosaicGateway
    public class AuditDatabaseGateway : IAuditDatabaseGateway
    {
        public AuditDatabaseGateway()
        {
        }

        public void GenerateAuditRecord(AuditRequestObject auditRequestObject)
        {
            throw new System.NotImplementedException();
        }
    }
}
