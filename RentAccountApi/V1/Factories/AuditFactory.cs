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
        public static MyRentAccountAdminAudit ToAdminAuditRequest(CreateAdminAuditRequest auditRequestObject)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new MyRentAccountAdminAudit
            {
                User = auditRequestObject.User.ToLower(),
                RentAccountNumber = auditRequestObject.RentAccountNumber,
                TimeStamp = FormatToUkDate(DateTime.Now).ToString("o"),
                CSSOLogin = auditRequestObject.CSSOLogin.ToString(),
                AuditAction = auditRequestObject.AuditAction.ToString().ToLower()
            };
        }

        public static MyRentAccountResidentAudit ToResidentAuditRequest(CreateResidentAuditRequest residentAuditRequest)
        {
            return new MyRentAccountResidentAudit
            {
                hackney_accountnumber = residentAuditRequest.RentAccountNumber,
                hackney_postcode = residentAuditRequest.PostCode,
                hackney_name = residentAuditRequest.LoggedIn ? "One Account Rent Account Audit History" : "Anonymous Rent Account Audit History",
                hackney_accounttype = "1",
                hackney_tagreferencenumber = ""
            };
        }

        public static GetAllAuditsResponse ToGetAllAuditsResponse(List<AdminAuditRecord> auditRecords)
        {
            var auditResponse = auditRecords.Select(record => new AdminAuditResponse
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
