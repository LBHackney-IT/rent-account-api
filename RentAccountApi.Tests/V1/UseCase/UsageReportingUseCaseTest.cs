using AutoFixture;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RentAccountApi.Tests.V1.Helper;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    [TestFixture]
    public class UsageReportingUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private UsageReportingsUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new UsageReportingsUseCase(_mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ReturnsCorrectResponseWhenEverythingIsWorking()
        {
            var token = "token";

            var usageReportResponse = new UsageReportResponse
            {
                StartDate = "2020-08-01",
                EndDate = "2020-08-31",
                UniqueAnonymousUsers = 1,
                TotalAnonymousLogins = 2,
                UniqueCSSOUsers = 3,
                TotalCSSOLogins = 4,
                NewCSSOLinkedAccounts = 5,
                TotalLogins = 6
            };

            var usageReportRequest = new UsageReportRequest
            {
                StartDate = Convert.ToDateTime("2020-08-01"),
                EndDate = Convert.ToDateTime("2020-08-31")
            };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetUniqueAnonymousUsers(usageReportRequest, token)).ReturnsAsync(1);
            _mockCrmGateway.Setup(x => x.GetTotalAnonymousLogins(usageReportRequest)).ReturnsAsync(2);
            _mockCrmGateway.Setup(x => x.GetUniqueCSSOUsers(usageReportRequest)).ReturnsAsync(3);
            _mockCrmGateway.Setup(x => x.GetTotalCSSOLogins(usageReportRequest)).ReturnsAsync(4);
            _mockCrmGateway.Setup(x => x.GetNewCSSOLinkedAccounts(usageReportRequest)).ReturnsAsync(5);
            _mockCrmGateway.Setup(x => x.GetTotalLogins(usageReportRequest)).ReturnsAsync(6);

            var response = _classUnderTest.Execute(usageReportRequest);

            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(usageReportResponse);
        }
    }
}
