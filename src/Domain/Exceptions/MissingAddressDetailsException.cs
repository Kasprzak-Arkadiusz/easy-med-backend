namespace EasyMed.Domain.Exceptions;

public class MissingAddressDetailsException : Exception
{
    public MissingAddressDetailsException(string message) : base(message) { }
}