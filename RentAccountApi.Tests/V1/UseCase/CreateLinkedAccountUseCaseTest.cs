using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase;
using RentAccountApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    [TestFixture]
    public class CreateLinkedAccountUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private CreateLinkedAccountUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new CreateLinkedAccountUseCase(_mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public async Task CreateReturnsCorrectResponseWhenLinkedAccountExists()
        {
            var crmAccountID = "456";
            var cssoID = "44444";
            var token = "token";
            var rentAccountNumber = "12345";

            var crmResponse = "98765";

            var createLinkedAccountRequest = new CreateLinkedAccountRequest
            {
                CssoId = cssoID,
                RentAccountNumber = rentAccountNumber
            };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetCrmAccountId(rentAccountNumber, token)).ReturnsAsync(crmAccountID);
            _mockCrmGateway.Setup(x => x.CreateLinkedAccount(crmAccountID, cssoID)).ReturnsAsync(crmResponse);

            var response = await _classUnderTest.Execute(createLinkedAccountRequest).ConfigureAwait(true);


            response.Should().NotBeNull();
            response.LinkedAccountId.Should().Be(crmResponse);
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenAccountDoesNotExist()
        {
            string crmAccountID = null;
            var cssoID = "44444";
            var token = "token";
            var rentAccountNumber = "12345";

            var crmResponse = "98765";

            var createLinkedAccountRequest = new CreateLinkedAccountRequest
            {
                CssoId = cssoID,
                RentAccountNumber = rentAccountNumber
            };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetCrmAccountId(rentAccountNumber, token)).ReturnsAsync(crmAccountID);
            _mockCrmGateway.Setup(x => x.CreateLinkedAccount(crmAccountID, cssoID)).ReturnsAsync(crmResponse);

            var response = await _classUnderTest.Execute(createLinkedAccountRequest).ConfigureAwait(true);

            response.Should().BeNull();
        }
    }
}
