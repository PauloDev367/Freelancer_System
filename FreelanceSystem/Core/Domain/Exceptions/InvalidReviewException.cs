namespace Domain.Exceptions;

public class InvalidReviewException : Exception
{
    public InvalidReviewException(string message) : base(message)
    {
    }
}