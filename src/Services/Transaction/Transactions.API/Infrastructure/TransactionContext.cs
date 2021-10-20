namespace CodeChallenge.Services.Transactions.Api.Infrastructure
{
    using CodeChallenge.DataObjects.Transactions;
    using CodeChallenge.Services.Transactions.Api.Infrastructure.EntityConfigurations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {}

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
            builder.ApplyConfiguration(new TransactionTypeEntityTypeConfiguration());
            builder.HasDefaultSchema("public");
            base.OnModelCreating(builder);
        }

    }

    public class MemberContextDesignFactory : IDesignTimeDbContextFactory<TransactionContext>
    {
        public TransactionContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransactionContext>()
                .UseNpgsql("Host=127.0.0.1;User ID=postgres;Password=1q2w3e4r;Database=transaction;Pooling=true;");

            return new TransactionContext(optionsBuilder.Options);
        }
    }
}
