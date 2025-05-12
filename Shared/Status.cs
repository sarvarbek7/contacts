namespace Contacts.Shared;

[Flags]
public enum Status
{
    NotAssigned = 1,
    AssignedToUser = 2,
    AssignedToPosition = 4,
}