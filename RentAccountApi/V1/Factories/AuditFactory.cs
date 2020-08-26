using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentAccountApi.V1.Factories
{
    public static class AuditFactory
    {
        public static MyRentAccountAudit ToAuditRequest(CreateAuditRequest auditRequestObject)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new MyRentAccountAudit
            {
                User = auditRequestObject.User.ToLower(),
                RentAccountNumber = auditRequestObject.RentAccountNumber,
                TimeStamp = FormatToUkDate(DateTime.Now).ToString("o"),
                CSSOLogin = auditRequestObject.CSSOLogin.ToString(),
                AuditAction = auditRequestObject.AuditAction.ToString().ToLower()
            };
        }

        public static GetAllAuditsResponse ToGetAllAuditsResponse(List<AuditRecord> auditRecords)
        {
            var auditResponse = auditRecords.Select(record => new AuditResponse
            {
                User = record.User,
                TimeStamp = record.TimeStamp,
                RentAccountNumber = record.RentAccountNumber,
                CSSOLogin = bool.Parse(record.CSSOLogin),
                AuditAction = record.AuditAction

            }).ToList();

            return new GetAllAuditsResponse
            {
                AuditRecords = auditResponse
            };
        }

        private static DateTime FormatToUkDate(DateTime date)
        {
            //try to convert if the OSVersion isn't Windows
            if (!Environment.OSVersion.VersionString.ToLower().Contains("windows"))
            {
                var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
                date = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, ukTimeZone);
            }

            return date;
        }
    }
}
