using CodeChallenge.Services.Transactions.Api.Infrastructure;
using CodeChallenge.Services.Transactions.Api.Controllers;
using CodeChallenge.Services.Transactions.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Transactions;
using CodeChallenge.Services.Transactions.Api;
using CodeChallenge.Services.Transactions.Api.IntegrationEvents;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Customer.Application
{
    public class TransactionControllerTest
    {
        private readonly DbContextOptions<TransactionContext> _dbOptions;

        private readonly Mock<ILogger<TransactionController>> _loggerMock;

        private readonly Mock<ITransactionIntegrationEventService> _transactionIntegrationEventService;

        private readonly Mock<ICustomerService> _serviceMock;

        public TransactionControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<TransactionContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new TransactionContext(_dbOptions))
            {
                if (dbContext.Transactions.Count() == 0)
                {
                    dbContext.Transactions.AddRange(GetMockTransaction());
                    dbContext.SaveChanges();
                }
            }
            _loggerMock = new Mock<ILogger<TransactionController>>();
            _transactionIntegrationEventService = new Mock<ITransactionIntegrationEventService>();
            _serviceMock = new Mock<ICustomerService>();
        }

        [Fact]
        public async Task Get_transaction_resultset_success()
        {
            try
            {
                var setting = new TestTransactionSettings();
                var transactionContext = new TransactionContext(_dbOptions);
                var transactionController = new TransactionController(
                    _loggerMock.Object, transactionContext, _serviceMock.Object, setting, _transactionIntegrationEventService.Object);
                var actionResult = await transactionController.GetTransactionResultSetAsync(string.Empty, null, 1, 15);

                //Assert
                Assert.IsType<ActionResult<ResultSet<Transaction>>>(actionResult);
                var result = actionResult.Result as OkObjectResult;
                var resultset = Assert.IsAssignableFrom<ResultSet<Transaction>>(result.Value);
                Assert.Equal(2, resultset.Total);
                Assert.Equal(1, resultset.Page);
                Assert.Equal(15, resultset.ItemPerPage);
                Assert.Equal(2, resultset.Items.Count());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Fact]
        public async Task Get_transaction_by_number_success()
        {
            try
            {
                var setting = new TestTransactionSettings();
                var transactionContext = new TransactionContext(_dbOptions);
                var transactionController = new TransactionController(
                    _loggerMock.Object, transactionContext, _serviceMock.Object, setting, _transactionIntegrationEventService.Object);
                var actionResult = await transactionController.GetTransactionByNumberAsync("31000000123");

                //Assert
                Assert.IsType<ActionResult<Transaction>>(actionResult);
                var result = actionResult.Result as OkObjectResult;
                var transaction = Assert.IsAssignableFrom<Transaction>(result.Value);
                Assert.Equal(12, transaction.Id);
                Assert.Equal("MockCustomer2", transaction.AccountName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<Transaction> GetMockTransaction()
        {
            return new List<Transaction>()
            {
                new Transaction()
                {
                    Id = 11,
                    TransactionTypeId = 1,
                    TransactionNumber = "32000000112",
                    AccountName = "MockCustomer1",
                    AccountNumber = "MOCK1",
                    Amount = 100,
                    Fee = 0,
                    Created= DateTime.Now
                },
                new Transaction()
                {
                    Id = 12,
                    TransactionTypeId = 1,
                    TransactionNumber = "31000000123",
                    AccountName = "MockCustomer2",
                    AccountNumber = "MOCK2",
                    Amount = 200,
                    Fee = 0,
                    Created= DateTime.Now
                }
            };

        }

    }
    
    public class TestTransactionSettings : IOptionsSnapshot<TransactionSettings>
    {
        public TransactionSettings Value => new TransactionSettings
        {
            PercentageOfFeeCharged = 0.1M,
            ConnectionString = "",
            GatewayUrl = ""
        };

        public TransactionSettings Get(string name) => Value;
    }
}