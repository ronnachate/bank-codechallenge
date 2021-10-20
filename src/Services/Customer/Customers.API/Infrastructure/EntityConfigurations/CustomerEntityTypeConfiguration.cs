using CodeChallenge.DataObjects.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CodeChallenge.Services.Customers.Api.Infrastructure.EntityConfigurations
{
    class CustomerAccountEntityTypeConfiguration
        : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .IsRequired();

            builder.Property(cb => cb.AccountNumber)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(cb => cb.AccountName)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(cb => cb.CurrentBalance)
                .IsRequired();
        }

    }
}
