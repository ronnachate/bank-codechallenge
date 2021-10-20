namespace WeDev.Payment.Services.Transactions.ExpiryChecker.Infrastructure
{
    using WeDev.Payment.DataObjects.Payments;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {}

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("public");
            base.OnModelCreating(builder);
        }

    }

    public class MemberContextDesignFactory : IDesignTimeDbContextFactory<TransactionContext>
    {
        public TransactionContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransactionContext>()
                .UseNpgsql("Host=127.0.0.1;User ID=postgres;Password=1q2w3e4r;Database=payment;Pooling=true;");

            return new TransactionContext(optionsBuilder.Options);
        }
    }
}
