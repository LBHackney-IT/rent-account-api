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
    public class RentAccountTenancyControllerTests
    {
        private RentAccountTenancyController _classUnderTest;
        private Mock<IGetRentBreakdownUseCase> _getRentBreakdownUseCase;
        private Mock<IGetTransactionsUseCase> _getTransactionsUseCase;

        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _getRentBreakdownUseCase = new Mock<IGetRentBreakdownUseCase>();
            _getTransactionsUseCase = new Mock<IGetTransactionsUseCase>();
            _classUnderTest = new RentAccountTenancyController(_getRentBreakdownUseCase.Object, _getTransactionsUseCase.Object);
            _faker = new Faker();
        }

        #region GET tests
        [Test]
        public async Task GetRentAccountAnd200()
        {
            var rentBreakdownResponse = new List<RentBreakdown>();
            var tagRef = "12345/01";

            _getRentBreakdownUseCase.Setup(x => x.Execute(tagRef)).ReturnsAsync(rentBreakdownResponse);
            var response = (await _classUnderTest.GetRentBreakdown(tagRef).ConfigureAwait(true) as IActionResult) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeOfType<List<RentBreakdown>>();
            response.Value.Should().BeEquivalentTo(rentBreakdownResponse);
        }

        [Test]
        public async Task GetRentAccountWithInvalidRefReturns404()
        {
            var rentBreakdownResponse = new List<RentBreakdown>();
            rentBreakdownResponse = null;
            var tagRef = "12345/01";

            _getRentBreakdownUseCase.Setup(x => x.Execute(tagRef)).ReturnsAsync(rentBreakdownResponse);
            var response = (await _classUnderTest.GetRentBreakdown(tagRef).ConfigureAwait(true) as IActionResult) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
            response.Value.Should().Be("Rent breakdown not found");
        }

        [Test]
        public async Task GetTransactionsAnd200()
        {
            var accountNumber = "12345";
            var postCode = "E8 1DY";
            var transactionsResponse = new TransactionsResponse
            {
                TransactionRequest = new TransactionRequest
                {
                    PaymentRef = accountNumber,
                    PostCode = postCode
                },
                Transactions = new List<LBHTransactions>
                {
                    new LBHTransactions
                    {
                        Balance = "123.00",
                        Date = DateTime.Now.ToString(),
                        Description = "Some description",
                        valueIn = "123.00"
                    }
                }
            };

            _getTransactionsUseCase.Setup(x => x.Execute(accountNumber, postCode)).ReturnsAsync(transactionsResponse);
            var response = await _classUnderTest.GetTransactions(accountNumber, postCode).ConfigureAwait(true) as IActionResult as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeOfType<TransactionsResponse>();
            response.Value.Should().BeEquivalentTo(transactionsResponse);
        }

        [Test]
        public async Task GetEmptyTransactions()
        {
            var accountNumber = "12345";
            var postCode = "E8 1DY";
            var transactionsResponse = new TransactionsResponse
            {
                TransactionRequest = new TransactionRequest
                {
                    PaymentRef = accountNumber,
                    PostCode = postCode
                },
                Transactions = new List<LBHTransactions>()
            };

            _getTransactionsUseCase.Setup(x => x.Execute(accountNumber, postCode)).ReturnsAsync(transactionsResponse);
            var response = await _classUnderTest.GetTransactions(accountNumber, postCode).ConfigureAwait(true) as IActionResult as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            var transactionResponse = (TransactionsResponse) response.Value;
            transactionResponse.Transactions.Count.Should().Equals(0);
        }
        #endregion

    }
}
