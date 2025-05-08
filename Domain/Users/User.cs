using Contacts.Domain.Accounts;
using Contacts.Domain.Common;
using Contacts.Domain.PhoneNumbers;
using Domain.Entities;

namespace Contacts.Domain.Users;

public class User : IEntity<int>, IFromExternalSource<int>
{
    public int Id { get; init; }
    public int ExternalId { get; init; }
    public List<PhoneNumber> ActivePhoneNumbers { get; set; } = [];
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Photo { get; set; }
    public List<UserPhoneNumber> PhoneNumberHistory { get; set; } = [];
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
}
