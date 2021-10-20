using CodeChallenge.DataObjects.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeChallenge.Services.Transactions.Api.Infrastructure.EntityConfigurations
{
    class TransactionEntityTypeConfiguration
        : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .IsRequired();

            builder.Property(cb => cb.TransactionNumber)
                .HasMaxLength(20);

            builder.Property(cb => cb.Amount)
                .IsRequired();

            builder.Property(cb => cb.Fee)
                .IsRequired();

            builder.Property(cb => cb.CustomerId)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(cb => cb.CustomerName)
                .HasMaxLength(150);

            builder.Property(cb => cb.RecieverId)
                .HasMaxLength(11);

            builder.Property(cb => cb.RecieverName)
                .HasMaxLength(150);

         }
    }
}
