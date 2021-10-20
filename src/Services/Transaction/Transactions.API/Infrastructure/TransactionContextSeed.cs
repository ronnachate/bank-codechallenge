namespace CodeChallenge.Services.Transactions.Api.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using CodeChallenge.Services.Transactions.Api;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Npgsql;
    using CodeChallenge.DataObjects.Transactions;

    public class TransactionContextSeed
    {
        public async Task SeedAsync(TransactionContext context, IWebHostEnvironment env, IOptions<TransactionSettings> settings, ILogger<TransactionContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(TransactionContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var contentRootPath = env.ContentRootPath;

                context.Database.EnsureCreated();

                if (!context.TransactionTypes.Any())
                {
                    foreach (var type in GetPreconfiguredTransactionTypes().OrderBy(o => o.Id))
                    {
                        await context.TransactionTypes.AddAsync(type);
                    }
                    await context.SaveChangesAsync();
                }

            });
        }

        private IEnumerable<TransactionType> GetPreconfiguredTransactionTypes()
        {
            return new List<TransactionType>()
            {
                new TransactionType() { Id = 1, Name = "Deposit"},
                new TransactionType() { Id = 2, Name = "Withdraw"},
                new TransactionType() { Id = 3, Name = "Transfer" },
            };
        }


        private AsyncRetryPolicy CreatePolicy(ILogger<TransactionContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<NpgsqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
