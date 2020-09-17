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
    public class TransactionsUseCaseTest
    {
        private Mock<ITransactionsGateway> _mockTransactionsGateway;
        private GetTransactionsUseCase _classUnderTest;
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _mockTransactionsGateway = new Mock<ITransactionsGateway>();
            _classUnderTest = new GetTransactionsUseCase(_mockTransactionsGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenTransactionExists()
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

            _mockTransactionsGateway.Setup(x => x.GetTransactions(accountNumber, postCode)).ReturnsAsync(transactionsResponse);

            var response = await _classUnderTest.Execute(accountNumber, postCode).ConfigureAwait(true);

            response.Should().NotBeNull();
            response.Should().BeOfType<TransactionsResponse>();
            response.Should().Equals(transactionsResponse);
        }

        [Test]
        public async Task ReturnsCorrectResponseWhenTransactionDoesNotExist()
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

            _mockTransactionsGateway.Setup(x => x.GetTransactions(accountNumber, postCode)).ReturnsAsync(transactionsResponse);

            var response = await _classUnderTest.Execute(accountNumber, postCode).ConfigureAwait(true);

            response.Should().NotBeNull();
            response.Should().BeOfType<TransactionsResponse>();
            response.Should().Equals(transactionsResponse);
            response.Transactions.Count.Should().Equals(0);
        }
    }
}
