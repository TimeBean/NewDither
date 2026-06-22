namespace Service.Dither.Core.Exception;

public class WrongQuantityException : System.Exception
{
    public WrongQuantityException()
    {
    }

    public WrongQuantityException(string? message) : base(message)
    {
    }

    public WrongQuantityException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}