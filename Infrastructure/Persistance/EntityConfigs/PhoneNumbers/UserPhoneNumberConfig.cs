using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserPhoneNumberConfig : IEntityTypeConfiguration<UserPhoneNumber>
{
    public void Configure(EntityTypeBuilder<UserPhoneNumber> builder)
    {
        builder.HasQueryFilter(x => x.IsActive);
        builder.HasQueryFilter(x => !x.PhoneNumber!.IsDeleted);
    }
}
