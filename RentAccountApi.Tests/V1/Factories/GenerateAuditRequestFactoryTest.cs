using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using RentAccountApi.V1.Boundary.Request;
using Bogus;
using RentAccountApi.Tests.V1.Helper;
using RentAccountApi.V1.Domain;
using System.Collections.Generic;

namespace RentAccountApi.Tests.V1.Factories
{
    [TestFixture]
    public class GenerateAuditRequestFactoryTest
    {
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _faker = new Faker("en_GB");
        }
        [Test]
        public void CanMapInputToGenerateAuditRequestObject()
        {
            var auditRequest = TestHelpers.CreateAuditRequestObject(_faker);

            var factoryResponse = AuditFactory.ToAuditRequest(auditRequest);

            factoryResponse.User.Should().Be(auditRequest.User);
            factoryResponse.RentAccountNumber.Should().Be(auditRequest.RentAccountNumber);
            factoryResponse.AuditAction.Should().Be(auditRequest.AuditAction);
            factoryResponse.CSSOLogin.Should().Be(auditRequest.CSSOLogin.ToString());
            factoryResponse.TimeStamp.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void CanMapAuditRecordsToGetAllAuditsResponseObject()
        {
            var auditRecords = new List<AuditRecord>
            {
               TestHelpers.CreateAuditRecordObject(_faker),
               TestHelpers.CreateAuditRecordObject(_faker)
            };

            var factoryResponse = AuditFactory.ToGetAllAuditsResponse(auditRecords);

            factoryResponse.Should().NotBeNull();
            factoryResponse.AuditRecords.Count.Should().Equals(auditRecords.Count);
            factoryResponse.AuditRecords[0].User.Should().Equals(auditRecords[0].User);
            factoryResponse.AuditRecords[0].CSSOLogin.ToString().Should().Equals(auditRecords[0].CSSOLogin);
        }
    }
}
