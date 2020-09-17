using AutoFixture;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RentAccountApi.Tests.V1.Helper;
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
    public class GetLinkedAccountUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private GetLinkedAccountUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new GetLinkedAccountUseCase(_mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ReturnsCorrectResponseWhenLinkedAccountExists()
        {
            var cssoId = "456";
            var token = "token";

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

            var response = _classUnderTest.Execute(cssoId);

            var crmLinkedAccount = crmLinkedAccountResponse.value[0];

            response.Should().NotBeNull();
            response.Result.csso_id.Should().BeEquivalentTo(crmLinkedAccount.csso_id);
            response.Result.rent_account_number.Should().BeEquivalentTo(crmLinkedAccount.rent_account_number);
            response.Result.hackney_csso_linked_rent_accountid.Should().BeEquivalentTo(crmLinkedAccount.hackney_csso_linked_rent_accountid);
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountDoesNotExist()
        {
            var cssoId = "456";
            var token = "token";
            var crmLinkedAccountResponse = new CrmLinkedAccountResponse { value = new List<CRMLinkedAccount>() };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetLinkedAccount(cssoId, token)).ReturnsAsync(crmLinkedAccountResponse);

            var response = _classUnderTest.Execute(cssoId);
            response.Result.Should().BeNull();
        }
    }
}
