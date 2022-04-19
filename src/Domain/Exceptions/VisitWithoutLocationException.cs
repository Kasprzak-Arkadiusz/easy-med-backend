namespace EasyMed.Domain.Exceptions;

public class VisitWithoutLocationException : Exception
{
    public VisitWithoutLocationException(string message) : base(message) { }
}