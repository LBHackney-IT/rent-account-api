using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
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
    public class DeleteLinkedAccountUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private DeleteLinkedAccountUseCase _classUnderTest;
        private Faker _faker;
        private Mock<IGetLinkedAccountUseCase> _getLinkedAccountUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new DeleteLinkedAccountUseCase(_mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _getLinkedAccountUseCase = new Mock<IGetLinkedAccountUseCase>();
            _faker = new Faker();
        }

        [Test]
        public async Task DeleteReturnsCorrectResponseWhenLinkedAccountExists()
        {
            var cssoId = "456";
            var token = "token";

            var crmResponse = true;

            var crmLinkedAccountResponse = new CrmLinkedAccountResponse
            {
                value = new List<CRMLinkedAccount>()
                {
                    new CRMLinkedAccount
                    {
                        csso_id = "456",
                        hackney_csso_linked_rent_accountid = "12345",
                        rent_account_number = "7890"
                    }
                }
            };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetLinkedAccount(cssoId, token)).ReturnsAsync(crmLinkedAccountResponse);
            _mockCrmGateway.Setup(x => x.DeleteLinkedAccount(crmLinkedAccountResponse.value[0].hackney_csso_linked_rent_accountid)).ReturnsAsync(crmResponse);

            var response = await _classUnderTest.Execute(cssoId).ConfigureAwait(true);


            response.Should().NotBeNull();
            response.success.Should().Be(crmResponse);
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenAccountDoesNotExist()
        {
            var cssoId = "456";
            var token = "token";

            var crmResponse = true;

            var crmLinkedAccountResponse = new CrmLinkedAccountResponse();
            crmLinkedAccountResponse = null;

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetLinkedAccount(cssoId, token)).ReturnsAsync(crmLinkedAccountResponse);
            _mockCrmGateway.Setup(x => x.DeleteLinkedAccount(cssoId)).ReturnsAsync(crmResponse);

            var response = await _classUnderTest.Execute(cssoId).ConfigureAwait(true);


            response.Should().BeNull();
        }
    }
}
