using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Transactions;
using CodeChallenge.Services.Transactions.Api.Infrastructure;
using CodeChallenge.Services.Transactions.Api.Models;

namespace CodeChallenge.Services.Transactions.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionContext _transactionContext;
        private readonly IEventBus _eventBus;
        private readonly IOptions<TransactionSettings> _settings;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            ILogger<TransactionController> logger,
            TransactionContext paymentContext,
            IOptions<TransactionSettings> settings,
            IEventBus eventBus)
        {
            _logger = logger;
            _transactionContext = paymentContext;
            _settings = settings;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultSet<Transaction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultSet<Transaction>>> GetTransactionResultSetAsync(
            [FromQuery] int? customerId,
            [FromQuery] int? typeId,
            [FromQuery] int page = 1,
            [FromQuery] int rows = 10)
        {
            try
            {
                var transaction = _transactionContext.Transactions
                    .Include(t => t.TransactionType)
                    .AsQueryable();
                var resultSet = new TransactionResult(transaction, rows);
                if (customerId != null)
                {
                    resultSet.ApplyCustomerFilter((int)customerId);
                }
                if (typeId != null)
                {
                    resultSet.ApplyTypeFilter((int)typeId);
                }
                return Ok(await resultSet.GetItemsByPageAsync(page));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR get transaction list: {Exception}", Program.AppName, ex);
                return BadRequest();
            }
        }
    }
}
