using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record AssignPhoneNumberToPositionUserRequest(HrmUser User, int PositionId);