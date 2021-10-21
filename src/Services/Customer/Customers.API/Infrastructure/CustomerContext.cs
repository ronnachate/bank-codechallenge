namespace CodeChallenge.Services.Customers.Api.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    //using EntityConfigurations;
    using CodeChallenge.Services.Customers.Api.Infrastructure.EntityConfigurations;
    using CodeChallenge.DataObjects.Customers;


    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {}

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerAccountEntityTypeConfiguration());

            builder.HasDefaultSchema("public");
            base.OnModelCreating(builder);
        }
    }

    public class CustomerContextDesignFactory : IDesignTimeDbContextFactory<CustomerContext>
    {
        public CustomerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>()
                .UseNpgsql("Host=127.0.0.1;User ID=postgres;Password=abc1234;Database=customer;Pooling=true;");

            return new CustomerContext(optionsBuilder.Options);
        }
    }
}
