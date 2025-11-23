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

        builder.HasIndex(x => x.Number)
               .IsUnique();
        
        builder.HasMany(x => x.AssignedUsers)
               .WithOne(x => x.PhoneNumber)
               .HasForeignKey(x => x.PhoneNumberId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AssignedPositions)
               .WithOne(x => x.PhoneNumber)
               .HasForeignKey(x => x.PhoneNumberId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
