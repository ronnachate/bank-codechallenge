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

        [HttpGet]
        [ProducesResponseType(typeof(ResultSet<Customer>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultSet<Customer>>> GetCustomerResultSetAsync(
            [FromQuery] string q,
            [FromQuery] int? page = 1,
            [FromQuery] int? rows = 10)
        {
            try
            {
                var customers = _customerContext.Customers
                    .AsQueryable();
                var resultSet = new CustomerResult(customers, (int)rows);
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Customer>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerContext.Customers
                    .SingleOrDefaultAsync(r => r.Id == id);
                if (customer == null)
                {
                    return NotFound(new { Message = $"Customer with id {id} not found." });
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR get Customer by id: {Exception} with {Customer}", Program.AppName, ex, id);
                return BadRequest();
            }
        }

    }
}
