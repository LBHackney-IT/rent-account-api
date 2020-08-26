using Bogus;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.Helper
{
    public static class TestHelpers
    {
        public static CreateAuditRequest CreateAuditRequestObject(Faker faker)
        {
            return new CreateAuditRequest
            {
                User = faker.Person.Email.ToLower(),
                RentAccountNumber = faker.Random.Int(5).ToString(),
                CSSOLogin = faker.Random.Bool(),
                AuditAction = faker.PickRandomParam(new[] { "unlink", "view" })
            };
        }

        public static AuditRecord CreateAuditRecordObject(Faker faker)
        {
            return new AuditRecord
            {
                User = faker.Person.Email.ToLower(),
                RentAccountNumber = faker.Random.Int(5).ToString(),
                CSSOLogin = faker.Random.Bool().ToString(),
                TimeStamp = faker.Random.String(),
                AuditAction = faker.PickRandomParam(new[] { "unlink", "view" })
            };
        }
    }
}
