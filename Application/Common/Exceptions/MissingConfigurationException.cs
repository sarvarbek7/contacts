namespace Contacts.Application.Common.Exceptions;

public class MissingConfigurationException : Exception
{
    public MissingConfigurationException() : base("Configuration is missing") { }

    public MissingConfigurationException(string parameterName) : base($"Configuration is missing of paramater {parameterName}") { }
}