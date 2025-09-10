namespace compiler_construction.Tokenization.Symbols;

public class Range : Token
{
    public Range(string representation)
    {
        this.sourceText = representation;
    }
}
