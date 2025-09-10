namespace compiler_construction.Tokenization.Symbols;

public class StatementSeparator : Token
{
    public StatementSeparator(string representation)
    {
        this.sourceText = representation;
    }
}
