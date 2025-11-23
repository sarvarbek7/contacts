using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.PhoneNumbers;

internal class PositionAssignmentConfig : IEntityTypeConfiguration<PositionAssignment>
{
    public void Configure(EntityTypeBuilder<PositionAssignment> builder)
    {
        builder.HasMany(x => x.Users)
               .WithMany(x => x.PositionAssignments);
    }
}