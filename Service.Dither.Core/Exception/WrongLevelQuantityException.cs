namespace Service.Dither.Core.Exception;

public class WrongLevelQuantityException : System.Exception
{
    public WrongLevelQuantityException()
    {
    }

    public WrongLevelQuantityException(string? message) : base(message)
    {
    }

    public WrongLevelQuantityException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}