using CodeChallenge.DataObjects.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CodeChallenge.Services.Customers.Api.Infrastructure.EntityConfigurations
{
    class CustomerEntityTypeConfiguration
        : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .IsRequired();

            builder.Property(cb => cb.Code)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(cb => cb.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(cb => cb.Lastname)
               .HasMaxLength(100);

            builder.Property(cb => cb.CurrentBalance)
                .IsRequired();
        }

    }
}
