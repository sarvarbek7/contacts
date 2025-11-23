using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.PhoneNumbers;

internal class UserAssignmentConfig : IEntityTypeConfiguration<UserAssignment>
{
    public void Configure(EntityTypeBuilder<UserAssignment> builder)
    {
        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PhoneNumber)
               .WithMany(x => x.AssignedUsers)
               .HasForeignKey(x => x.PhoneNumberId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}