using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class GetAllAuditsResponse
    {
        public List<AdminAuditResponse> AuditRecords { get; set; }
    }
}
