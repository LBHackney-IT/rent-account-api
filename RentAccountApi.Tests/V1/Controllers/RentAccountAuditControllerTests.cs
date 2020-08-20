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

namespace RentAccountApi.Tests.V1.Controllers
{
    public class RentAccountAuditControllerTests
    {
        private RentAccountAuditController _classUnderTest;
        private Mock<IPostAuditUseCase> _mockPostAuditUseCase;

        [SetUp]
        public void Setup()
        {
            _mockPostAuditUseCase = new Mock<IPostAuditUseCase>();
            _classUnderTest = new RentAccountAuditController(_mockPostAuditUseCase.Object);
        }

        [Test]
        public void EnsureControllerPostMethodCallsPostAuditUseCase()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<AuditRequestObject>()));
            _classUnderTest.GenerateToken(It.IsAny<AuditRequestObject>());

            _mockPostAuditUseCase.Verify(x => x.Execute(It.IsAny<AuditRequestObject>()), Times.Once);
        }

        [Test]
        public void ControllerPostMethodShouldReturnResponseOfTypeNoContentResult()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<AuditRequestObject>()));
            var result = _classUnderTest.GenerateToken(It.IsAny<AuditRequestObject>()) as NoContentResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void ControllerPostMethodShouldReturn204StatusCode()
        {
            _mockPostAuditUseCase.Setup(x => x.Execute(It.IsAny<AuditRequestObject>()));
            var result = _classUnderTest.GenerateToken(It.IsAny<AuditRequestObject>()) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }
    }
}
