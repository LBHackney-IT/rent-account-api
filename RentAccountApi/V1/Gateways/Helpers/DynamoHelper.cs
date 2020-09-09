using Amazon.DynamoDBv2.DocumentModel;
using RentAccountApi.V1.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways.Helpers
{
    public static class DynamoHelper
    {
        public static Document ConstructAdminAuditDynamoDocument(MyRentAccountAdminAudit generateAuditRequest)
        {
            return new Document
            {
                ["User"] = generateAuditRequest.User,
                ["TimeStamp"] = generateAuditRequest.TimeStamp,
                ["RentAccountNumber"] = generateAuditRequest.RentAccountNumber,
                ["CSSOLogin"] = generateAuditRequest.CSSOLogin,
                ["AuditAction"] = generateAuditRequest.AuditAction
            };
        }
    }
}
