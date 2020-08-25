using AutoFixture;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetAuditByUserUseCaseTest
    {
        private Mock<IAuditDatabaseGateway> _mockAuditGateway;
        private GetAuditByUserUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockAuditGateway = new Mock<IAuditDatabaseGateway>();
            _classUnderTest = new GetAuditByUserUseCase(_mockAuditGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ReturnsResidentInformationList()
        {
            var stubbedAudits =
                new List<AuditRecord>
                {
                    new AuditRecord
                    {
                        User = _faker.Random.String(),
                        TimeStamp = _faker.Random.String(),
                        RentAccountNumber = _faker.Random.String(),
                        CSSOLogin = _faker.Random.Bool().ToString()
                    },
                    new AuditRecord
                    {
                        User = _faker.Random.String(),
                        TimeStamp = _faker.Random.String(),
                        RentAccountNumber = _faker.Random.String(),
                        CSSOLogin = _faker.Random.Bool().ToString()
                    }
                };

            _mockAuditGateway.Setup(x => x.GetAuditByUser("matt@matt.com")).ReturnsAsync(stubbedAudits);

            var response = _classUnderTest.GetAuditByUser("matt@matt.com");

            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(AuditFactory.ToGetAllAuditsResponse(stubbedAudits));
        }


    }
}
