using Microsoft.AspNetCore.Mvc;
using CodeChallenge.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using CodeChallenge.DataObjects;
using CodeChallenge.Services.Customers.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.DataObjects.Customers;
using CodeChallenge.Services.Customers.Api.Models;
using CodeChallenge.Services.Customers.Api.IntegrationEvents;


namespace CodeChallenge.Services.Customers.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext _customerContext;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            ILogger<CustomerController> logger,
            CustomerContext customerContext)
        {
            _logger = logger;
            _customerContext = customerContext;
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(ResultSet<CustomerAccount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultSet<CustomerAccount>>> GetAccountResultSetAsync(
            [FromQuery] string q,
            [FromQuery] int? page = 1,
            [FromQuery] int? rows = 10)
        {
            try
            {
                var accounts = _customerContext.CustomerAccounts
                    .AsQueryable();
                var resultSet = new CustomerAccountResult(accounts, (int)rows);
                if( !string.IsNullOrEmpty(q))
                {
                    resultSet.ApplySearchFilter(q);
                }
                return Ok(await resultSet.GetItemsByPageAsync((int)page));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR get customer list with page: {Exception}", Program.AppName, ex);
                return BadRequest();
            }
        }

        [HttpGet("account/{number}")]
        [ProducesResponseType(typeof(CustomerAccount), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerAccount>> GetAccountByNumberAsync(string number)
        {
            try
            {
                var customer = await _customerContext.CustomerAccounts
                    .SingleOrDefaultAsync(a => a.AccountNumber == number);
                if (customer == null)
                {
                    return NotFound(new { Message = $"Customer's account with number {number} not found." });
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR get Customer's account by number: {Exception} with {number}", Program.AppName, ex, number);
                return BadRequest();
            }
        }

    }
}
