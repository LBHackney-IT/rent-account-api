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
    public class RentAccountCRMControllerTests
    {
        private RentAccountCRMController _classUnderTest;
        private Mock<ICheckRentAccountExistsUseCase> _checkRentAccountExistsUseCase;

        [SetUp]
        public void Setup()
        {
            _checkRentAccountExistsUseCase = new Mock<ICheckRentAccountExistsUseCase>();
            _classUnderTest = new RentAccountCRMController(_checkRentAccountExistsUseCase.Object);
        }

        #region GET tests
        [Test]
        public async Task CheckAccountExistsReturnsTrueAnd200()
        {
            var checkAccountExistsResponse = new CheckAccountExistsResponse()
            {
                Exists = true
            };

            var paymentReference = "1234567";
            var postCode = "E8 1DY";

            _checkRentAccountExistsUseCase.Setup(x => x.Execute(paymentReference, postCode)).ReturnsAsync(checkAccountExistsResponse);
            var response = (await _classUnderTest.CheckRentAccountExists(paymentReference, postCode).ConfigureAwait(true) as IActionResult) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(checkAccountExistsResponse);
        }

        [Test]
        public async Task CheckAccountExistsReturnsFalseAnd404()
        {
            var checkAccountExistsResponse = new CheckAccountExistsResponse()
            {
                Exists = false
            };

            var paymentReference = "1234567";
            var postCode = "E8 1DY";

            _checkRentAccountExistsUseCase.Setup(x => x.Execute(paymentReference, postCode)).ReturnsAsync(checkAccountExistsResponse);
            var response = (await _classUnderTest.CheckRentAccountExists(paymentReference, postCode).ConfigureAwait(true) as IActionResult) as ObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Account not found");
        }

        #endregion
    }
}
