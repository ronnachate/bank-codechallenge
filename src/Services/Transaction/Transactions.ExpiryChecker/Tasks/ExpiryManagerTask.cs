using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using WeDev.Payment.Services.Transactions.ExpiryChecker.Infrastructure;
using WeDev.Payment.BusinessLogic;
using WeDev.Payment.DataObjects.Payments;

namespace WeDev.Payment.Services.Transactions.ExpiryChecker.Tasks
{
    public class ExpiryManagerTask : BackgroundService
    {
        private readonly ILogger<ExpiryManagerTask> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly TransactionContext _transactionContext;

        public ExpiryManagerTask(
            IOptions<BackgroundTaskSettings> settings,
            TransactionContext transactionContext,
            ILogger<ExpiryManagerTask> logger
        )
        {
            _transactionContext = transactionContext;
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"ExpiryManagerTask is starting at {DateTime.Now.ToString()}");

            stoppingToken.Register(() => _logger.LogDebug("#ExpiryManagerTask background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug($"#ExpiryManagerTask background task is doing background work at {DateTime.Now.ToString()}");
                    var pendingStatuses = new List<int>()
                    {
                        (int)TransactionStatusEnum.NoAction,
                        (int)TransactionStatusEnum.Pending
                    };

                    var expiredTransactions = _transactionContext.Transactions
                        .Where(t => pendingStatuses.Contains(t.StatusId) && t.ExpiredDatetime < DateTime.Now);

                    foreach(var transaction in await expiredTransactions.ToListAsync())
                    {
                        transaction.StatusId = (int)TransactionStatusEnum.Expired;
                        //notify to merchant
                        try
                        {
                            MerchantNotifyManger.NotifyMerchant(transaction);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug($"ExpiryManagerTask notify to merchant with url {transaction.NotificationUrl} failed at {DateTime.Now.ToString()} {ex.Message}");
                        }
                    }
                    await _transactionContext.SaveChangesAsync();
                    _logger.LogDebug($"#ExpiryManagerTask background task is stoped work at {DateTime.Now.ToString()}");

                }
                catch ( Exception ex)
                {
                    _logger.LogDebug($"#ExpiryManagerTask background task is error with {ex.Message}");
                }

                // don some background process here
                await Task.Delay(_settings.ExpiryDurationInMinute * 60000, stoppingToken);
            }

            _logger.LogDebug("#ExpiryManagerTask background task is stopping.");

            await Task.CompletedTask;
        }
    }

}
