namespace Shared.Exceptions;

public class InvalidCredentialsException : Exception
{
    public string Message { get; set; }
    public string Code { get; set; }

    public InvalidCredentialsException(string message, string code = null)
    {
        Message = $"";
    }
}