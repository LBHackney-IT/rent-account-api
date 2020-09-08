using Bogus;
using Moq;
using NUnit.Framework;
using RentAccountApi.Tests.V1.Helper;
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
            var auditRequest = TestHelpers.CreateAuditRequestObject(_faker);
            _mockGateway.Setup(x => x.GenerateAdminAuditRecord(AuditFactory.ToAuditRequest(auditRequest)));
            _classUnderTest.CreateAdminAudit(auditRequest);

            _mockGateway.Verify(x => x.GenerateAdminAuditRecord(It.IsAny<MyRentAccountAdminAudit>()), Times.Once);
        }
    }
}
