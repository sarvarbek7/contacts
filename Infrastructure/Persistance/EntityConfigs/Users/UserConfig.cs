using Contacts.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.Users;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(x => x.Account)
            .WithOne()
            .HasForeignKey<User>(x => x.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Pinfl)
            .HasMaxLength(14)
            .IsFixedLength();

        builder.Property(x => x.FirstName)
            .HasMaxLength(64);
        
        builder.Property(x => x.LastName)
            .HasMaxLength(64);
        
        builder.Property(x => x.MiddleName)
            .HasMaxLength(64);

        builder.HasMany(x => x.PhoneNumbers)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
