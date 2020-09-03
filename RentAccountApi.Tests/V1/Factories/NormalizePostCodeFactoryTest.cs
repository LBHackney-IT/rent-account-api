using FluentAssertions;
using NUnit.Framework;
using RentAccountApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.Tests.V1.Factories
{
    public class NormalizePostCodeFactoryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CheckPostCodeWithSpacesFormattedCorrectly()
        {
            var postCode = "E8 1DY";

            var factoryResponse = CRMFactory.NormalizePostcode(postCode);

            factoryResponse.Should().Equals(postCode);
        }

        [Test]
        public void CheckPostCodeWithoutSpacesFormattedCorrectly()
        {
            var postCode = "E81DY";
            var expectedPostCode = "E8 1DY";

            var factoryResponse = CRMFactory.NormalizePostcode(postCode);

            factoryResponse.Should().Equals(expectedPostCode);
        }

        [Test]
        public void CheckPostCodeCaseFormattedCorrectly()
        {
            var postCode = "e81dy";
            var expectedPostCode = "E8 1DY";

            var factoryResponse = CRMFactory.NormalizePostcode(postCode);

            factoryResponse.Should().Equals(expectedPostCode);
        }
    }
}
