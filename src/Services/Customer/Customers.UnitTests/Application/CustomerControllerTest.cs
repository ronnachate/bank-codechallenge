using CodeChallenge.Services.Customers.Api.Infrastructure;
using CodeChallenge.Services.Customers.Api.Controllers;
using CodeChallenge.Services.Customers.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Customers;
using CodeChallenge.Services.Customers.Api.Models;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Customer.Application
{
    public class CustomerControllerTest
    {
        private readonly DbContextOptions<CustomerContext> _dbOptions;

        private readonly Mock<IIBANService> _iBANServiceMock;
        private readonly Mock<ILogger<CustomerController>> _loggerMock;

        public CustomerControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new CustomerContext(_dbOptions))
            {
                if(dbContext.CustomerAccounts.Count() == 0)
                {
                    dbContext.CustomerAccounts.AddRange(GetMockCustomer());
                    dbContext.SaveChanges();
                }
            }

            _iBANServiceMock = new Mock<IIBANService>();
            _loggerMock = new Mock<ILogger<CustomerController>>();
        }

        [Fact]
        public async Task Get_account_resultset_success()
        {
            var customerContext = new CustomerContext(_dbOptions);
            var customerController = new CustomerController(_iBANServiceMock.Object, _loggerMock.Object, customerContext);
            var actionResult = await customerController.GetAccountResultSetAsync(string.Empty, null, 15);


            //Assert
            Assert.IsType<ActionResult<ResultSet<CustomerAccount>>>(actionResult);
            var result = actionResult.Result as OkObjectResult;
            var resultset = Assert.IsAssignableFrom<ResultSet<CustomerAccount>>(result.Value);
            Assert.Equal(3, resultset.Total);
            Assert.Equal(1, resultset.Page);
            Assert.Equal(15, resultset.ItemPerPage);
            Assert.Equal(3, resultset.Items.Count());

        }

        [Fact]
        public async Task Get_account_by_number_success()
        {
            var customerContext = new CustomerContext(_dbOptions);
            var customerController = new CustomerController(_iBANServiceMock.Object, _loggerMock.Object, customerContext);
            var actionResult = await customerController.GetAccountByNumberAsync("MOCK2");

            //Assert
            Assert.IsType<ActionResult<CustomerAccount>>(actionResult);
            var result = actionResult.Result as OkObjectResult;
            var account = Assert.IsAssignableFrom<CustomerAccount>(result.Value);
            Assert.Equal(2, account.Id);
            Assert.Equal("MockCustomer2", account.AccountName);
        }

        private List<CustomerAccount> GetMockCustomer()
        {
            return new List<CustomerAccount>()
            {
                new CustomerAccount()
                {
                    Id = 1,
                    AccountName = "MockCustomer1",
                    AccountNumber = "MOCK1",
                    CurrentBalance = 1,
                },
                new CustomerAccount()
                {
                    Id = 2,
                    AccountName = "MockCustomer2",
                    AccountNumber = "MOCK2",
                    CurrentBalance = 2,
                },
                new CustomerAccount()
                {
                    Id = 3,
                    AccountName = "MockCustomer2",
                    AccountNumber = "MOCK3",
                    CurrentBalance = 0,
                },
            };

        }

    }
}
