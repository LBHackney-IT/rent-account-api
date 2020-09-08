using System.Collections.Generic;
using System.Threading.Tasks;
using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Domain;

namespace RentAccountApi.V1.Gateways
{
    public interface IAuditDatabaseGateway
    {
        Task GenerateAdminAuditRecord(MyRentAccountAudit myRentAccountAudit);
        Task<List<AuditRecord>> GetAuditByUser(string user, int recordLimit);
    }
}
