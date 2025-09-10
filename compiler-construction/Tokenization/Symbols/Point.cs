namespace compiler_construction.Tokenization.Symbols;

public class Point : Token
{
    public Point(string representation)
    {
        this.sourceText = representation;
    }
}
