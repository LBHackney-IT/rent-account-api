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
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    [TestFixture]
    public class CheckRentAccountExistsUseCaseTest
    {
        private Mock<ICRMGateway> _mockCrmGateway;
        private CheckRentAccountExistsUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockCrmGateway = new Mock<ICRMGateway>();
            _classUnderTest = new CheckRentAccountExistsUseCase(_mockCrmGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountExists()
        {
            var paymentReference = "1234567";
            var postCode = "E8 1DY";
            var checkAccountExistsResponse =
                new CheckAccountExistsResponse
                {
                    Exists = true
                };

            _mockCrmGateway.Setup(x => x.CheckAccountExists(paymentReference, postCode)).ReturnsAsync(checkAccountExistsResponse);

            var response = _classUnderTest.Execute(paymentReference,postCode);

            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(checkAccountExistsResponse);
        }

        [Test]
        public void ReturnsCorrectResponseWhenAccountDoesNotExist()
        {
            var paymentReference = "1234567";
            var postCode = "E8 1DY";
            var checkAccountExistsResponse =
                new CheckAccountExistsResponse
                {
                    Exists = false
                };

            _mockCrmGateway.Setup(x => x.CheckAccountExists(paymentReference, postCode)).ReturnsAsync(checkAccountExistsResponse);

            var response = _classUnderTest.Execute(paymentReference, postCode);

            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(checkAccountExistsResponse);
        }
    }
}
