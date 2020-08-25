using Bogus;
using Moq;
using NUnit.Framework;
using RentAccountApi.V1.Boundary;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    public class PostAuditUseCaseTest
    {
        private PostAuditUseCase _classUnderTest;

        private Mock<IAuditDatabaseGateway> _mockGateway;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IAuditDatabaseGateway>();
            _classUnderTest = new PostAuditUseCase(_mockGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void UseCaseShouldCallGatewayToInsertAuditData()
        {
            var auditRequest = GetAuditRequestObject();
            _mockGateway.Setup(x => x.GenerateAuditRecord(AuditFactory.ToAuditRequest(auditRequest)));
            _classUnderTest.Execute(GetAuditRequestObject());

            _mockGateway.Verify(x => x.GenerateAuditRecord(It.IsAny<MyRentAccountAudit>()), Times.Once);
        }

        private CreateAuditRequest GetAuditRequestObject()
        {
            return new CreateAuditRequest
            {
                User = _faker.Random.String(),
                RentAccountNumber = _faker.Random.String(),
                CSSOLogin = _faker.Random.Bool()
            };
        }
    }
}
