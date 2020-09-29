using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RentAccountApi.V1.Controllers;
using RentAccountApi.V1.Boundary.Response;
using Bogus;
using RentAccountApi.Tests.V1.Helper;

namespace RentAccountApi.Tests.V1.Controllers
{
    public class RentAccountUsageReportingControllerTests
    {
        private RentAccountUsageReportingController _classUnderTest;
        private Mock<IUsageReportingsUseCase> _usageReportingUseCase;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _usageReportingUseCase = new Mock<IUsageReportingsUseCase>();
            _classUnderTest = new RentAccountUsageReportingController(_usageReportingUseCase.Object);
            _faker = new Faker();
        }

        #region GET tests
        [Test]
        public async Task GetUsageReportAnd200()
        {
            var usageReportResponse = new UsageReportResponse
            {
                StartDate = "2020-08-01",
                EndDate = "2020-08-31",
                UniqueAnonymousUsers = _faker.Random.Number(0, 3000),
                TotalAnonymousLogins = _faker.Random.Number(0, 3000),
                UniqueCSSOUsers = _faker.Random.Number(0, 3000),
                TotalCSSOLogins = _faker.Random.Number(0, 3000),
                NewCSSOLinkedAccounts = _faker.Random.Number(0, 3000),
                TotalLogins = _faker.Random.Number(0, 3000)
            };

            var usageReportRequest = new UsageReportRequest
            {
                StartDate = Convert.ToDateTime("2020-08-01"),
                EndDate = Convert.ToDateTime("2020-08-31")
            };

            _usageReportingUseCase.Setup(x => x.Execute(usageReportRequest)).ReturnsAsync(usageReportResponse);
            var response = (await _classUnderTest.RunUsageReport(usageReportRequest).ConfigureAwait(true) as IActionResult) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(usageReportResponse);
        }

        [Test]
        public async Task GetUsageReportWithInvalidDateReturns400()
        {
            var usageReportResponse = new UsageReportResponse
            {
                StartDate = "2020-08-01",
                EndDate = "2020-08-31",
                UniqueAnonymousUsers = _faker.Random.Number(0, 3000),
                TotalAnonymousLogins = _faker.Random.Number(0, 3000),
                UniqueCSSOUsers = _faker.Random.Number(0, 3000),
                TotalCSSOLogins = _faker.Random.Number(0, 3000),
                NewCSSOLinkedAccounts = _faker.Random.Number(0, 3000),
                TotalLogins = _faker.Random.Number(0, 3000)
            };

            var usageReportRequest = new UsageReportRequest
            {
                StartDate = Convert.ToDateTime("2020-09-01"),
                EndDate = Convert.ToDateTime("2020-08-31")
            };

            _usageReportingUseCase.Setup(x => x.Execute(usageReportRequest)).Throws(new UsageReportRequestException("Start date must be before end date"));
            var response = (await _classUnderTest.RunUsageReport(usageReportRequest).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
            response.Value.Should().BeEquivalentTo("Start date must be before end date");
        }


        #endregion
    }
}
