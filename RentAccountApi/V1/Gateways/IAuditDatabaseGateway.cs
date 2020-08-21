using System.Collections.Generic;
using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Domain;

namespace RentAccountApi.V1.Gateways
{
    public interface IAuditDatabaseGateway
    {
        void GenerateAuditRecord(MyRentAccountAudit myRentAccountAudit);
    }
}
