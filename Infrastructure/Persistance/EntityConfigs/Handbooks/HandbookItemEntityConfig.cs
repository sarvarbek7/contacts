using Contacts.Domain.Handbook;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.Handbooks;

public class HandbookItemEntityConfig : IEntityTypeConfiguration<HandbookItem>
{
    public void Configure(EntityTypeBuilder<HandbookItem> builder)
    {
        builder.HasOne(x => x.PhoneNumber)
               .WithMany()
               .HasForeignKey(x => x.PhoneNumberId);

    }
}
