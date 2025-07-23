namespace Contacts.Infrastructure.ProcessingServices.Exceptions;

public class HrmNotWorkingException : Exception
{
    public HrmNotWorkingException()
    {
    }

    public HrmNotWorkingException(string? message) : base(message)
    {
    }

    public HrmNotWorkingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}