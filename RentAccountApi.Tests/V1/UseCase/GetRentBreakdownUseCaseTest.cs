using Amazon.DynamoDBv2.Model;
using AutoFixture;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using RentAccountApi.Tests.V1.Helper;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.UseCase;
using RentAccountApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetRentBreakdownUseCaseTest
    {
        private Mock<IRentBreakdownGateway> _mockRentBreakdownGateway;
        private GetRentBreakdownUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockRentBreakdownGateway = new Mock<IRentBreakdownGateway>();
            _classUnderTest = new GetRentBreakdownUseCase(_mockRentBreakdownGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenRentBreakdownExists()
        {
            var tagRef = "12345/01";

            var rentBreakdownResponse = new List<RentBreakdown>();

            _mockRentBreakdownGateway.Setup(x => x.GetRentBreakdown(tagRef)).ReturnsAsync(rentBreakdownResponse);

            var response = await _classUnderTest.Execute(tagRef).ConfigureAwait(true);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<RentBreakdown>>();
        }

        [Test]
        public async Task ReturnsCorrectResponseValuesWhenRentBreakdownExists()
        {
            var tagRef = "12345/01";

            var rentBreakdownResponse = new List<RentBreakdown>()
            {
                new RentBreakdown()
                {
                    code = _faker.Random.String(),
                    description = _faker.Random.String(),
                    effectiveDate = _faker.Random.String(),
                    lastChargeDate = _faker.Random.String(),
                    value = _faker.Random.String()
                }
            };

            _mockRentBreakdownGateway.Setup(x => x.GetRentBreakdown(tagRef)).ReturnsAsync(rentBreakdownResponse);

            var response = await _classUnderTest.Execute(tagRef).ConfigureAwait(true);

            response.Should().NotBeNull();
            response[0].Should().Equals(rentBreakdownResponse[0]);
            response.Should().BeOfType<List<RentBreakdown>>();
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenRentBreakdownDoesNotExist()
        {
            var tagRef = "12345/01";

            var rentBreakdownResponse = new List<RentBreakdown>();
            rentBreakdownResponse = null;

            _mockRentBreakdownGateway.Setup(x => x.GetRentBreakdown(tagRef)).ReturnsAsync(rentBreakdownResponse);

            var response = await _classUnderTest.Execute(tagRef).ConfigureAwait(true);

            response.Should().BeNull();
        }
    }
}
