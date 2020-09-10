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
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IAuditDatabaseGateway>();
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new PostAuditUseCase(_mockGateway.Object, _mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void UseCaseShouldCallGatewayToInsertAuditData()
        {
            var auditRequest = TestHelpers.CreateAuditRequestObject(_faker);
            _mockGateway.Setup(x => x.GenerateAdminAuditRecord(AuditFactory.ToAdminAuditRequest(auditRequest)));
            _classUnderTest.CreateAdminAudit(auditRequest);

            _mockGateway.Verify(x => x.GenerateAdminAuditRecord(It.IsAny<MyRentAccountAdminAudit>()), Times.Once);
        }

        [Test]
        public async Task UseCaseShouldCallGatewayToInsertResidentAuditData()
        {
            var token = "12345";
            var auditRequest = TestHelpers.CreateResidentAuditRequestObject(_faker, true);
            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GenerateResidentAuditRecord(AuditFactory.ToResidentAuditRequest(auditRequest), token)).ReturnsAsync(true);
            await _classUnderTest.CreateResidentAudit(auditRequest).ConfigureAwait(true);

            _mockCrmGateway.Verify(x => x.GenerateResidentAuditRecord(It.IsAny<MyRentAccountResidentAudit>(), token), Times.Once);
        }
    }
}
