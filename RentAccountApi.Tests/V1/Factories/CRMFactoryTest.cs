using FluentAssertions;
using NUnit.Framework;
using RentAccountApi.Tests.V1.Helper;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.Factories
{
    public class CRMFactoryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CheckPrivateResponseFormatForName()
        {
            var paymentReference = "12345";
            var privacy = true;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(true);

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                Name = "Bxx Bxxxxx"
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.Name.Should().Equals(rentAccountResponse.Name);
        }

        [Test]
        public void CheckResponseFormatForName()
        {
            var paymentReference = "12345";
            var privacy = false;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(true);

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                Name = "Bob Bobson"
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.Name.Should().Equals(rentAccountResponse.Name);
        }

        [Test]
        public void CheckResponseForCurrentBalanceAndNoArrears()
        {
            var paymentReference = "12345";
            var privacy = false;
            var positiveBalance = true;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(positiveBalance);

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                CurrentBalance = 123.45M,
                HasArrears = false
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.CurrentBalance.Should().Equals(rentAccountResponse.CurrentBalance);
            response.HasArrears.Should().Equals(rentAccountResponse.HasArrears);
        }

        [Test]
        public void CheckResponseForNegativeCurrentBalanceAndArrears()
        {
            var paymentReference = "12345";
            var privacy = false;
            var positiveBalance = false;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(positiveBalance);

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                CurrentBalance = -123.45M,
                HasArrears = true
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.CurrentBalance.Should().Equals(rentAccountResponse.CurrentBalance);
            response.HasArrears.Should().Equals(rentAccountResponse.HasArrears);
        }

        [Test]
        public void CheckResponseForToPayCalculation()
        {
            var paymentReference = "12345";
            var privacy = false;
            var positiveBalance = true;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateRentAccountResponseObject(positiveBalance);

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                ToPay = 800.00M
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.ToPay.Should().Equals(rentAccountResponse.ToPay);
        }

        [Test]
        public void CheckResponseForToPayCalculationIsZero()
        {
            var paymentReference = "12345";
            var privacy = false;
            CrmRentAccountResponse crmRentAccountResponse = TestHelpers.CreateNothingToPayResponseObject();

            RentAccountResponse rentAccountResponse = new RentAccountResponse
            {
                ToPay = 0.00M
            };

            var response = CRMFactory.ToResponse(paymentReference, crmRentAccountResponse, privacy);

            response.ToPay.Should().Equals(rentAccountResponse.ToPay);
        }
    }
}
