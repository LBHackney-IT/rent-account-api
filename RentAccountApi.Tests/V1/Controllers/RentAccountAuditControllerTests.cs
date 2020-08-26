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

namespace RentAccountApi.Tests.V1.Controllers
{
    public class RentAccountAuditControllerTests
    {
        private RentAccountAuditController _classUnderTest;
        private Mock<IPostAuditUseCase> _mockPostAuditUseCase;
        private Mock<IGetAuditByUserUseCase> _mockGetAuditByUserUseCase;

        [SetUp]
        public void Setup()
        {
            _mockPostAuditUseCase = new Mock<IPostAuditUseCase>();
            _mockGetAuditByUserUseCase = new Mock<IGetAuditByUserUseCase>();
            _classUnderTest = new RentAccountAuditController(_mockPostAuditUseCase.Object, _mockGetAuditByUserUseCase.Object);
        }

        #region POST tests
        [Test]
        public void EnsureControllerPostMethodCallsPostAuditUseCase()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<CreateAuditRequest>()));
            _classUnderTest.GenerateAuditLog(It.IsAny<CreateAuditRequest>());

            _mockPostAuditUseCase.Verify(x => x.Execute(It.IsAny<CreateAuditRequest>()), Times.Once);
        }

        [Test]
        public void ControllerPostMethodShouldReturnResponseOfTypeNoContentResult()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<CreateAuditRequest>()));
            var result = _classUnderTest.GenerateAuditLog(It.IsAny<CreateAuditRequest>()) as NoContentResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void ControllerPostMethodShouldReturn204StatusCode()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<CreateAuditRequest>()));
            var result = _classUnderTest.GenerateAuditLog(It.IsAny<CreateAuditRequest>()) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }
        #endregion

        #region GET tests
        [Test]
        public async Task GetAuditsReturnsAuditAnd200()
        {
            var audits = new List<AuditResponse>()
            {
                new AuditResponse()
                {
                    User = "john.doe@missing.com",
                    TimeStamp = "2020-08-26T08:16:19.1571481",
                    RentAccountNumber = "123456",
                    CSSOLogin = false
                }
            };

            var auditRecords = new GetAllAuditsResponse()
            {
                AuditRecords = audits
            };

            var userEmail = "john.doe@missing.com";

            _mockGetAuditByUserUseCase.Setup(x => x.GetAuditByUser(userEmail)).ReturnsAsync(auditRecords);
            var response = (await _classUnderTest.GetAuditByUser(userEmail).ConfigureAwait(true) as IActionResult) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(auditRecords);
        }

        [Test]
        public async Task GetAuditWithNoQuerystringReturns400AndException()
        {
            var audits = new List<AuditResponse>()
            {
                new AuditResponse()
                {
                    User = "john.doe@missing.com",
                    TimeStamp = "2020-08-26T08:16:19.1571481",
                    RentAccountNumber = "123456",
                    CSSOLogin = false
                }
            };

            var auditRecords = new GetAllAuditsResponse()
            {
                AuditRecords = audits
            };

            var userEmail = "";

            _mockGetAuditByUserUseCase.Setup(x => x.GetAuditByUser(userEmail)).ReturnsAsync(auditRecords);
            var response = await _classUnderTest.GetAuditByUser(userEmail).ConfigureAwait(true) as ObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
            response.Value.Should().Be("Parameter useremail must be provided.");
        }

        #endregion
    }
}
