using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Infrastructure;
using System;

namespace RentAccountApi.V1.Factories
{
    public static class AuditFactory
    {
        public static MyRentAccountAudit ToAuditRequest(AuditRequestObject auditRequestObject)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new MyRentAccountAudit
            {
                User = auditRequestObject.User,
                RentAccountNumber = auditRequestObject.RentAccountNumber,
                TimeStamp = DateTime.Now.ToUniversalTime().ToString("s")
            };
        }
    }
}
