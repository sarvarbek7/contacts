using System.Security.Cryptography.X509Certificates;
using Contacts.Domain.Handbook;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistance.EntityConfigs.Handbooks;

public class HandbookEntityConfig : IEntityTypeConfiguration<Handbook>
{
    public void Configure(EntityTypeBuilder<Handbook> builder)
    {
        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey(x => x.HandbookId);

    }
}
