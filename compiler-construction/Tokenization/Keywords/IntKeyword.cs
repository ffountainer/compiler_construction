namespace compiler_construction.Tokenization.Keywords;

public class IntKeyword : Token
{
    public IntKeyword(string representation)
    {
        this.sourceText = representation;
    }
}
