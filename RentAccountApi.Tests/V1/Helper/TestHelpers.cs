using Bogus;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
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

        public static RentAccountResponse CreateRentAccountResponseObject(Faker faker)
        {
            return new RentAccountResponse
            {
                Name = faker.Person.FullName,
                AccountNumber = faker.Random.Int(5).ToString(),
                IsHackneyResponsible = faker.Random.Bool(),
                Postcode = faker.PickRandomParam(new[] { "N8 0DY", "RM3 0FS", "E8 1DY" }),
                CurrentBalance = faker.Random.Decimal(),
                Benefits = faker.Random.Decimal(),
                Rent = faker.Random.Decimal(),
                HasArrears = faker.Random.Bool(),
                ToPay = faker.Random.Decimal(),
                TenancyAgreementId = faker.Random.Int(5).ToString(),
                NextPayment = ""
            };
        }

        public static CrmRentAccountResponse CreateRentAccountResponseObject(bool positiveBalance)
        {
            CrmRentAccountResponse crmRentAccountResponse = new CrmRentAccountResponse
            {
                value = new List<CRMRentAccount>
                {
                   new CRMRentAccount
                   {
                       contact1_x002e_firstname = "Bob",
                       contact1_x002e_lastname = "Bobson",
                       contact1_x002e_address1_postalcode = "E8 1DY",
                       contact1_x002e_hackney_responsible = "true",
                       housing_anticipated = "200.00",
                       housing_prop_ref = "1234567",
                       housing_house_ref = "1234567",
                       housing_cur_bal = positiveBalance ? "123.45" : "-123.45",
                       housing_rent = "1000.00",
                       housing_tag_ref = "12345/01"
                   }
                }
            };
            return crmRentAccountResponse;
        }

        public static CrmRentAccountResponse CreateNothingToPayResponseObject()
        {
            CrmRentAccountResponse crmRentAccountResponse = CreateRentAccountResponseObject(true);
            crmRentAccountResponse.value[0].housing_rent = "200.00";
            return crmRentAccountResponse;
        }
    }
}
