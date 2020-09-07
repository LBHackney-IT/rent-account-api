using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentAccountApi.V1.UseCase.Helpers;
using FluentAssertions;

namespace RentAccountApi.Tests.V1.Helper
{
    public class PrivacyFormattingTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CheckPrivacyFormatting()
        {
            var stringToTest = "Hello";
            var expectedResponse = "Hxxxx";
            var value = PrivacyFormatting.GetPrivacyString(stringToTest);
            value.Should().Equals(expectedResponse);
        }
    }
}
