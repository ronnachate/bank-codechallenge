using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.BuildingBlocks.EventBus.Abstractions;
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
        private readonly IEventBus _eventBus;
        private readonly ILogger<TransactionController> _logger;
        private readonly ICustomerService _customerService;

        public TransactionController(
            ILogger<TransactionController> logger,
            TransactionContext transactionContext,
            ICustomerService customerService,
            IEventBus eventBus)
        {
            _logger = logger;
            _transactionContext = transactionContext;
            _customerService = customerService;
            _eventBus = eventBus;
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
                var transaction = _transactionContext.Transactions
                    .Include(t => t.TransactionType)
                    .AsQueryable();
                var resultSet = new TransactionResult(transaction, rows);
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

        [HttpPost]
        [ProducesResponseType(typeof(NewTransactionResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<NewTransactionResponse>> NewAccountAsync([FromBody] NewTransactionRequest request)
        {
            try
            {
                if (request.Amount > 0)
                {
                    var allowTypeId = new List<int> { 1, 2, 3 };
                    //templary put here like this, due to test time limit
                    if(!allowTypeId.Contains(request.TransactionTypeId))
                    {
                        _logger.LogError("[{AppName}] ERROR create new transaction: invalid type id {NewAccountRequest}", Program.AppName, request);
                        return BadRequest();
                    }

                    var transaction = new Transaction
                    {
                        TransactionTypeId = request.TransactionTypeId,
                        Amount = request.Amount,
                        Created = DateTime.Now
                    };
                    try
                    {
                        var customerAccount = await _customerService.GetAccountByNumberAsync(request.AccountNumber);
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


                    await _transactionContext.Transactions.AddAsync(transaction);
                    await _transactionContext.SaveChangesAsync();

                    transaction.TransactionNumber = CodeManager.GenerateTransactionCode(transaction.Id);
                    await _transactionContext.SaveChangesAsync();

                    var notifyEvent = new TransactionCreatedEvent(
                        transaction.TransactionTypeId, transaction.Amount, transaction.AccountNumber, transaction.RecieverAccountNumber
                    );
                    _eventBus.Publish(notifyEvent);

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
