namespace Domain.Exceptions;

public class InvalidProposalException : Exception
{
    public InvalidProposalException(string message) : base(message)
    {
    }
}