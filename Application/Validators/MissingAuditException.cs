namespace Contacts.Application.Validators;

[Serializable]
internal class MissingAuditException : Exception
{
    public MissingAuditException()
    {
    }

    public MissingAuditException(string? message) : base(message)
    {
    }

    public MissingAuditException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}