using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.PhoneNumbers;

internal class PhoneNumberConfig : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.Property(x => x.Type)
               .HasDefaultValue(PhoneNumberType.Railway);

        builder.HasMany(x => x.UsersHistory)
            .WithOne(h => h.PhoneNumber)
            .HasForeignKey(h => h.PhoneNumberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PositionHistory)
            .WithOne(h => h.PhoneNumber)
            .HasForeignKey(h => h.PhoneNumberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PositionUsersHistory)
            .WithOne(h => h.PhoneNumber)
            .HasForeignKey(h => h.PhoneNumberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ActiveAssignedUser)
            .WithMany(x => x.ActivePhoneNumbers)
            .HasForeignKey(x => x.ActiveAssignedUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.ActiveAssignedPositionUser)
            .WithMany(x => x.ActivePhonePositionNumbers)
            .HasForeignKey(x => x.ActiveAssignedPositionUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
