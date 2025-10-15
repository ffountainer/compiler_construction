namespace compiler_construction.Syntax;

public class UnexpectedTokenException : Exception
{
    public UnexpectedTokenException(string message) : base(message)
    {
    }
}
