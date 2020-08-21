using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using RentAccountApi.V1.Boundary.Request;
using Bogus;

namespace RentAccountApi.Tests.V1.Factories
{
    [TestFixture]
    public class GenerateAuditRequestFactoryTest
    {
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _faker = new Faker();
        }
        [Test]
        public void CanMapInputToGenerateAuditRequestObject()
        {
            var auditRequest = new AuditRequestObject
            {
                User = _faker.Random.String(),
                RentAccountNumber = _faker.Random.Int(5).ToString()
            };

            var factoryResponse = AuditFactory.ToAuditRequest(auditRequest);

            factoryResponse.User.Should().Be(auditRequest.User);
            factoryResponse.RentAccountNumber.Should().Be(auditRequest.RentAccountNumber);
            factoryResponse.TimeStamp.Should().NotBe(null);
        }
    }
}