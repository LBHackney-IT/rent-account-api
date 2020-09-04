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
    public class GetRentAccountUseCaseUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private Mock<ICRMTokenGateway> _mockCrmTokenGateway;
        private GetRentAccountUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _mockCrmTokenGateway = new Mock<ICRMTokenGateway>();
            _classUnderTest = new GetRentAccountUseCase(_mockCrmGateway.Object, _mockCrmTokenGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountExistsPositiveBalance()
        {
            var paymentReference = "1234567";
            var privacy = false;
            var token = "token";
            var positiveBalance = false;
            var crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(positiveBalance);

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetRentAccount(paymentReference, token)).ReturnsAsync(crmRentAccountResponse);

            var response = _classUnderTest.Execute(paymentReference, privacy);

            var crmRentAccount = crmRentAccountResponse.value[0];

            var fullName = $"{crmRentAccount.contact1_x002e_firstname} {crmRentAccount.contact1_x002e_lastname}";

            response.Should().NotBeNull();
            response.Result.Name.Should().BeEquivalentTo(fullName);
            response.Result.IsHackneyResponsible.Should().Be(bool.Parse(crmRentAccount.contact1_x002e_hackney_responsible));
            response.Result.Benefits.Should().Be(decimal.Parse(crmRentAccount.housing_anticipated));
            response.Result.Rent.Should().Be(decimal.Parse(crmRentAccount.housing_rent));
            response.Result.HasArrears.Should().BeFalse();
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountExistsNegativeBalance()
        {
            var paymentReference = "1234567";
            var privacy = false;
            var token = "token";
            var positiveBalance = true;
            var crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(positiveBalance);

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetRentAccount(paymentReference, token)).ReturnsAsync(crmRentAccountResponse);

            var response = _classUnderTest.Execute(paymentReference, privacy);

            var crmRentAccount = crmRentAccountResponse.value[0];

            var fullName = $"{crmRentAccount.contact1_x002e_firstname} {crmRentAccount.contact1_x002e_lastname}";

            response.Should().NotBeNull();
            response.Result.Name.Should().BeEquivalentTo(fullName);
            response.Result.IsHackneyResponsible.Should().Be(bool.Parse(crmRentAccount.contact1_x002e_hackney_responsible));
            response.Result.Benefits.Should().Be(decimal.Parse(crmRentAccount.housing_anticipated));
            response.Result.Rent.Should().Be(decimal.Parse(crmRentAccount.housing_rent));
            response.Result.HasArrears.Should().BeTrue();
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountDoesNotExist()
        {
            var paymentReference = "1234567";
            var privacy = false;
            var token = "token";
            var crmRentAccountResponse = new CrmRentAccountResponse { value = new List<CRMRentAccount>() };

            _mockCrmTokenGateway.Setup(x => x.GetCRMToken()).ReturnsAsync(token);
            _mockCrmGateway.Setup(x => x.GetRentAccount(paymentReference, token)).ReturnsAsync(crmRentAccountResponse);

            var response = _classUnderTest.Execute(paymentReference, privacy);
            response.Result.Should().BeNull();
        }
    }
}
