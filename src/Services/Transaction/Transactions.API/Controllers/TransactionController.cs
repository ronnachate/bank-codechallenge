using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.Services.Transactions.Api.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Events;
using CodeChallenge.DataObjects.Transactions;
using CodeChallenge.Services.Transactions.Api.Infrastructure;
using CodeChallenge.Services.Transactions.Api.Models;
using CodeChallenge.Services.Transactions.Api.Services;
using CodeChallenge.Util;

namespace CodeChallenge.Services.Transactions.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionContext _transactionContext;
        private readonly ITransactionIntegrationEventService _transactionIntegrationEventService;
        private readonly ILogger<TransactionController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IOptions<TransactionSettings> _settings;

        public TransactionController(
            ILogger<TransactionController> logger,
            TransactionContext transactionContext,
            ICustomerService customerService,
            IOptions<TransactionSettings> settings,
            ITransactionIntegrationEventService transactionIntegrationEventService)
        {
            _logger = logger;
            _transactionContext = transactionContext;
            _customerService = customerService;
            _transactionIntegrationEventService = transactionIntegrationEventService;
            _settings = settings;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultSet<Transaction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultSet<Transaction>>> GetTransactionResultSetAsync(
            [FromQuery] string accountNumber,
            [FromQuery] int? typeId,
            [FromQuery] int page = 1,
            [FromQuery] int rows = 10)
        {
            try
            {
                var transactions = _transactionContext.Transactions
                    .AsQueryable();
                var resultSet = new TransactionResult(transactions, rows);
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    resultSet.ApplyAccountFilter(accountNumber);
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

        [HttpGet("{transactionNumber}")]
        [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Transaction>> GetTransactionByNumberAsync(string transactionNumber)
        {
            try
            {
                if (CodeManager.IsValidTransactionCode(transactionNumber))
                {
                    var transaction = await _transactionContext.Transactions
                        .SingleOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
                    if (transaction != null)
                    {
                        return Ok(transaction);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Invalid transactionNumber");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR get transaction by number: {Exception}", Program.AppName, ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewTransactionResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<NewTransactionResponse>> NewTransactionAsync([FromBody] NewTransactionRequest request)
        {
            try
            {
                if (request.Amount > 0)
                {
                    var allowTypeId = new List<int> {
                        (int)TransactionTypeEnum.Deposit,
                        (int)TransactionTypeEnum.Withdraw,
                        (int)TransactionTypeEnum.Transfer
                    };
                    //templary put here like this, due to test time limit
                    if(!allowTypeId.Contains(request.TransactionTypeId))
                    {
                        _logger.LogError("[{AppName}] ERROR create new transaction: invalid type id {NewAccountRequest}", Program.AppName, request);
                        return BadRequest();
                    }

                    var transaction = new Transaction
                    {
                        TransactionTypeId = request.TransactionTypeId,
                        AccountNumber = request.AccountNumber,
                        Amount = request.Amount,
                        Fee = 0M,
                        Created = DateTime.Now
                    };
                    try
                    {
                        var customerAccount = await _customerService.GetAccountByNumberAsync(request.AccountNumber);
                        if(request.TransactionTypeId == (int)TransactionTypeEnum.Withdraw)
                        {
                            if(request.Amount > customerAccount.CurrentBalance)
                            {
                                _logger.LogError("[{AppName}] ERROR create new transaction: invalid withdaraw amount {NewAccountRequest}", Program.AppName, request);
                                return BadRequest();
                            }
                        }
                        transaction.AccountName = customerAccount.AccountName;
                    }
                    catch (Exception)
                    {
                        _logger.LogError("[{AppName}] ERROR create new transaction: No account found with {NewAccountRequest}", Program.AppName, request);
                        return BadRequest();
                    }


                    if (request.TransactionTypeId == (int)TransactionTypeEnum.Transfer)
                    {
                        try
                        {
                            var recieverAccount = await _customerService.GetAccountByNumberAsync(request.RecieverAccountNumber);
                            transaction.RecieverAccountNumber = recieverAccount.AccountNumber;
                            transaction.RecieverAccountName = recieverAccount.AccountName;
                        }
                        catch (Exception)
                        {
                            _logger.LogError("[{AppName}] ERROR create new transaction: No reciever account found with {NewAccountRequest}", Program.AppName, request);
                            return BadRequest();
                        }
                    }
                    else if(request.TransactionTypeId == (int)TransactionTypeEnum.Deposit)
                    {
                        transaction.Fee = TransactionHelper.GetTransactionFee(transaction.Amount, _settings.Value.PercentageOfFeeCharged);
                    }

                    await _transactionContext.Transactions.AddAsync(transaction);
                    await _transactionContext.SaveChangesAsync();

                    transaction.TransactionNumber = CodeManager.GenerateTransactionCode(transaction.Id);
                    await _transactionContext.SaveChangesAsync();

                    var notifyEvent = new TransactionCreatedEvent(
                        transaction.TransactionTypeId, transaction.GetNetAmout(), transaction.AccountNumber, transaction.RecieverAccountNumber
                    );
                    _transactionIntegrationEventService.PublishThroughEventBusAsync(notifyEvent);

                    var res = new NewTransactionResponse().ToSuccess("Create new transaction success.");
                    res.TransactionNumber = transaction.TransactionNumber;
                    return Ok(res);
                }
                else
                {
                    return BadRequest("Invalid amount");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{AppName}] ERROR create new transaction: {Exception} with {NewAccountRequest}", Program.AppName, ex, request);
                return BadRequest();
            }
        }
    }
}
