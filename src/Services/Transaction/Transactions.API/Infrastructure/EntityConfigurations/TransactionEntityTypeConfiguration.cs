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

            builder.Property(cb => cb.AccountNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(cb => cb.AccountName)
                .HasMaxLength(150);

            builder.Property(cb => cb.RecieverAccountNumber)
                .HasMaxLength(30);

            builder.Property(cb => cb.RecieverAccountName)
                .HasMaxLength(150);

         }
    }
}
