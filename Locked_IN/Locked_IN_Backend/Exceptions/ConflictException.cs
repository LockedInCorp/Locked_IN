namespace Locked_IN_Backend.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
