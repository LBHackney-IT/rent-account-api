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
    public class RentAccountCRMControllerTests
    {
        private RentAccountCRMController _classUnderTest;
        private Mock<ICheckRentAccountExistsUseCase> _checkRentAccountExistsUseCase;
        private Mock<IGetRentAccountUseCase> _getRentAccountUseCase;
        private Mock<IGetLinkedAccountUseCase> _getLinkedAccountUseCase;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _checkRentAccountExistsUseCase = new Mock<ICheckRentAccountExistsUseCase>();
            _getRentAccountUseCase = new Mock<IGetRentAccountUseCase>();
            _getLinkedAccountUseCase = new Mock<IGetLinkedAccountUseCase>();
            _classUnderTest = new RentAccountCRMController(_checkRentAccountExistsUseCase.Object, _getRentAccountUseCase.Object, _getLinkedAccountUseCase.Object);
            _faker = new Faker();
        }

        #region GET tests
        [Test]
        public async Task GetRentAccountAnd200()
        {
            var rentAccountResponse = TestHelpers.CreateRentAccountResponseObject(_faker);
            var paymentReference = "1234567";
            var privacy = true;

            _getRentAccountUseCase.Setup(x => x.Execute(paymentReference, privacy)).ReturnsAsync(rentAccountResponse);
            var response = (await _classUnderTest.GetRentAccount(paymentReference, privacy).ConfigureAwait(true) as IActionResult) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(rentAccountResponse);
        }

        [Test]
        public async Task GetRentAccountWithInvalidRefReturns404()
        {
            var rentAccountResponse = new RentAccountResponse();
            rentAccountResponse = null;
            var paymentReference = "1234567";
            var privacy = true;

            _getRentAccountUseCase.Setup(x => x.Execute(paymentReference, privacy)).ReturnsAsync(rentAccountResponse);
            var response = (await _classUnderTest.GetRentAccount(paymentReference, privacy).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Account not found");
        }

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

        [Test]
        public async Task GetLinkedAccountAnd200()
        {
            var linkedAccountResponse = new LinkedAccountResponse
            {
                AccountNumber = "123",
                CSSOId = "456",
                LinkedAccountId = "789"
            };
            var cssoId = "456";

            _getLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(linkedAccountResponse);
            var response = (await _classUnderTest.GetLinkedAccount(cssoId).ConfigureAwait(true) as IActionResult) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(linkedAccountResponse);
        }

        [Test]
        public async Task GetLinkedAccountWithInvalidRefReturns404()
        {
            var linkedAccountResponse = new LinkedAccountResponse();
            linkedAccountResponse = null;
            var cssoId = "456";

            _getLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(linkedAccountResponse);
            var response = (await _classUnderTest.GetLinkedAccount(cssoId).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Linked account not found");
        }

        #endregion
    }
}
