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
        private Mock<IDeleteLinkedAccountUseCase> _deleteLinkedAccountUseCase;
        private Mock<ICreateLinkedAccountUseCase> _createLinkedAccountUseCase;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _checkRentAccountExistsUseCase = new Mock<ICheckRentAccountExistsUseCase>();
            _getRentAccountUseCase = new Mock<IGetRentAccountUseCase>();
            _getLinkedAccountUseCase = new Mock<IGetLinkedAccountUseCase>();
            _deleteLinkedAccountUseCase = new Mock<IDeleteLinkedAccountUseCase>();
            _createLinkedAccountUseCase = new Mock<ICreateLinkedAccountUseCase>();
            _classUnderTest = new RentAccountCRMController(_checkRentAccountExistsUseCase.Object, _getRentAccountUseCase.Object, _getLinkedAccountUseCase.Object, _deleteLinkedAccountUseCase.Object, _createLinkedAccountUseCase.Object);
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
                rent_account_number = "123",
                csso_id = "456",
                hackney_csso_linked_rent_accountid = "789"
            };
            var cssoId = "456";

            _getLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(linkedAccountResponse);
            var response = await _classUnderTest.GetLinkedAccount(cssoId).ConfigureAwait(true) as IActionResult as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeOfType<List<LinkedAccountResponse>>();
            var listResponse = (List<LinkedAccountResponse>) response.Value;
            listResponse[0].Should().BeEquivalentTo(linkedAccountResponse);
        }

        [Test]
        public async Task GetLinkedAccountWithInvalidRefReturns200AndEmptyList()
        {
            var linkedAccountResponse = new LinkedAccountResponse();
            var cssoId = "456";

            _getLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(linkedAccountResponse);
            var response = (await _classUnderTest.GetLinkedAccount(cssoId).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeOfType<List<LinkedAccountResponse>>();
            var listResponse = (List<LinkedAccountResponse>) response.Value;
            listResponse[0].Should().BeEquivalentTo(linkedAccountResponse);
        }

        #endregion

        #region Delete tests

        [Test]
        public async Task DeleteLinkedAccountAnd204()
        {
            var deleteAccountResponse = new DeleteLinkedAccountResponse
            {
                success = true
            };
            var cssoId = "456";

            _deleteLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(deleteAccountResponse);
            var response = (await _classUnderTest.UnlinkAccount(cssoId).ConfigureAwait(true) as IActionResult) as NoContentResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(204);
        }

        [Test]
        public async Task DeleteLinkedAccountWithInvalidRefReturns404()
        {
            var deleteAccountResponse = new DeleteLinkedAccountResponse();
            deleteAccountResponse = null;
            var cssoId = "456";

            _deleteLinkedAccountUseCase.Setup(x => x.Execute(cssoId)).ReturnsAsync(deleteAccountResponse);
            var response = (await _classUnderTest.UnlinkAccount(cssoId).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Linked account not found");
        }

        #endregion

        #region Post tests

        [Test]
        public async Task CreateLinkedAccountWithInvalidValuesReturns404()
        {
            var createLinkedAccountResponse = new CreateLinkedAccountResponse();
            createLinkedAccountResponse = null;

            var createLinkedAccountRequest = new CreateLinkedAccountRequest();
            {

            };

            _createLinkedAccountUseCase.Setup(x => x.Execute(createLinkedAccountRequest)).ReturnsAsync(createLinkedAccountResponse);
            var response = (await _classUnderTest.CreateLinkedAccount(createLinkedAccountRequest).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Linked account could not be created, please check values");
        }

        [Test]
        public async Task CreateLinkedAccountCorrectTypeAnd200()
        {
            var createLinkedAccountResponse = new CreateLinkedAccountResponse
            {

            };

            var createLinkedAccountRequest = new CreateLinkedAccountRequest
            {

            };

            _createLinkedAccountUseCase.Setup(x => x.Execute(createLinkedAccountRequest)).ReturnsAsync(createLinkedAccountResponse);
            var response = (await _classUnderTest.CreateLinkedAccount(createLinkedAccountRequest).ConfigureAwait(true) as IActionResult) as OkObjectResult;
            response.Should().NotBeNull();
            response.Value.Should().BeOfType<CreateLinkedAccountResponse>();
            response.StatusCode.Should().Be(200);
        }

        #endregion
    }
}
